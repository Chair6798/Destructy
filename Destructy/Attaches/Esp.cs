using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Destructy.Attaches
{
    internal class EspBehaviour : MonoBehaviour
    {
        internal static GUIStyle white;
        internal static GUIStyle red;
        internal static GUIStyle lightred;
        internal static GUIStyle green;

        internal static bool playersEsp = false;
        internal static bool playersTracers = false;
        internal static bool playersBoxes = false;
        internal static bool weaponsEsp = false;
        
        internal static bool weaponsTracers = false;

        private static Material lineMaterial;

        private static void CreateLineMaterial()
        {
            if (lineMaterial != null) return;

            Shader shader = Shader.Find("Hidden/Internal-Colored");
            lineMaterial = new Material(shader);
            lineMaterial.hideFlags = HideFlags.HideAndDontSave;
            lineMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            lineMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            lineMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
            lineMaterial.SetInt("_ZWrite", 0);
        }
        internal static void Draw3DBox(Bounds bounds, Color color, float thickness = 1.5f)
        {
            Vector3[] corners = new Vector3[8];

            // 8 углов bounds
            corners[0] = bounds.min; // bottom-left-back
            corners[1] = new Vector3(bounds.min.x, bounds.min.y, bounds.max.z);
            corners[2] = new Vector3(bounds.max.x, bounds.min.y, bounds.min.z);
            corners[3] = new Vector3(bounds.max.x, bounds.min.y, bounds.max.z);

            corners[4] = new Vector3(bounds.min.x, bounds.max.y, bounds.min.z);
            corners[5] = new Vector3(bounds.min.x, bounds.max.y, bounds.max.z);
            corners[6] = new Vector3(bounds.max.x, bounds.max.y, bounds.min.z);
            corners[7] = bounds.max; // top-right-front

            // Проецируем все точки на экран
            Vector2[] screen = new Vector2[8];
            for (int i = 0; i < 8; i++)
            {
                Vector3 sp = Camera.main.WorldToScreenPoint(corners[i]);
                if (sp.z < 0) return; // если хотя бы один угол за камерой — не рисуем

                screen[i] = new Vector2(sp.x, Screen.height - sp.y);
            }

            // Нижнее основание
            DrawLine(screen[0], screen[1], color, thickness);
            DrawLine(screen[1], screen[3], color, thickness);
            DrawLine(screen[3], screen[2], color, thickness);
            DrawLine(screen[2], screen[0], color, thickness);

            // Верхнее основание
            DrawLine(screen[4], screen[5], color, thickness);
            DrawLine(screen[5], screen[7], color, thickness);
            DrawLine(screen[7], screen[6], color, thickness);
            DrawLine(screen[6], screen[4], color, thickness);

            // Вертикальные рёбра
            DrawLine(screen[0], screen[4], color, thickness);
            DrawLine(screen[1], screen[5], color, thickness);
            DrawLine(screen[2], screen[6], color, thickness);
            DrawLine(screen[3], screen[7], color, thickness);
        }
        internal static void DrawLine(Vector2 start, Vector2 end, Color color, float thickness = 1.5f)
        {
            if (lineMaterial == null) CreateLineMaterial();

            lineMaterial.SetPass(0);

            GL.PushMatrix();
            GL.LoadPixelMatrix(0, Screen.width, Screen.height, 0);

            GL.Begin(GL.LINES);
            GL.Color(color);

            // Для толщины просто рисуем несколько параллельных линий
            for (float i = -thickness / 2; i <= thickness / 2; i += 0.5f)
            {
                Vector2 offset = new Vector2(i, i); // можно улучшить, но для начала хватит
                GL.Vertex3(start.x + offset.x, start.y + offset.y, 0);
                GL.Vertex3(end.x + offset.x, end.y + offset.y, 0);
            }

            GL.End();
            GL.PopMatrix();
        }

        void Awake()
        {
            Logging.Log("Destructy EspBehaviour Awake called.");
            CreateLineMaterial();
            white = new GUIStyle();
            white.fontSize = 20;
            white.alignment = TextAnchor.MiddleCenter;
            white.normal.textColor = Color.white;
            red = new GUIStyle(white);
            red.normal.textColor = Color.red;
            lightred = new GUIStyle(white);
            lightred.normal.textColor = new Color(1f, 0.5f, 0.5f);
            green = new GUIStyle(white);
            green.normal.textColor = Color.green;
        }
        void OnGUI()
        {
            if(weaponsEsp || weaponsTracers)
            {
                foreach(var item in Lists.items)
                {
                    if (item == null) continue;

                    var pos = Camera.main.WorldToViewportPoint(item.transform.position);
                    if (pos.z > 0 )
                    {
                        if(weaponsEsp)
                            GUI.Label(GetRect(pos), item.ItemName, white);
                        if(weaponsTracers)
                            DrawLine(GetPosition(new Vector3(0.5f, 0.5f, 0)), GetPosition(pos), Color.white);
                    }
                     
                }
            }
            if (ElementCollector.GetBool("Visuals", "players_esp") || ElementCollector.GetBool("Visuals", "players_tracer"))
            {
                foreach (var plr in NetworkPlayer.players)
                {
                    if (plr == null || plr.IsLocalPlayer) continue;

                    var pos = Camera.main.WorldToViewportPoint(plr.transform.position);
                    if (pos.z > 0)
                    {
                        if (ElementCollector.GetBool("Visuals", "players_esp"))
                        {
                            var pd = Networking.instance.GetPlayer(plr.OwnerClientId);
                            if (pd.HasValue)
                            {
                                GUI.Label(GetRect(pos), pd.Value.GetName(), lightred);
                            }
                            
                        }
                        if (ElementCollector.GetBool("Visuals", "players_tracer"))
                            DrawLine(GetPosition(new Vector3(0.5f, 0.5f, 0)), GetPosition(pos), Color.red);

                    }
                    

                }
            }
            if(ElementCollector.GetBool("Visuals", "players_box"))
            {
                foreach (var plr in NetworkPlayer.players)
                {
                    if (plr == null || plr.IsLocalPlayer) continue;
                    var b = plr.GetComponent<Collider>().bounds;
                    b.Expand(new Vector3(0.5f, 1f, 0.5f));
                    b.center+=new Vector3(0, 0.5f, 0);
                    Draw3DBox(b, new Color(1f, 0.8f,0.8f), 1.5f);
                }
            }
        }
        internal static Rect GetRect(Vector3 pos)
        {
            return new Rect(pos.x * Screen.width - 200, Screen.height - pos.y * Screen.height - 25, 400, 50);
        }
        internal static Vector2 GetPosition(Vector3 pos)
        {
            return new Vector2(pos.x * Screen.width, Screen.height - pos.y * Screen.height);
        }
    }
}
