using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Destructy.Gui
{
    internal class WeaponGetMenu : MonoBehaviour
    {
        float width = 150;
        float height = 0.5f;
        Vector2 scrollPos = Vector2.zero;
        internal static WeaponGetMenu instance;
        void Awake()
        {
            instance = this;
            enabled = false;
        }
        void OnGUI()
        {
            if (WeaponController.instance == null || PlayerInventory.instance == null)
                Window(DrawAlt);
            else
                Window(Draw);
        }
        void Window(GUI.WindowFunction func)
        {
            GUI.Window(1, new Rect(Screen.width - width, Screen.height / 2 - Screen.height * height / 2, width, Screen.height * height), func, "Weapon Get Menu");
        }
        void GiveWeapon(WeaponData data)
        {
            PlayerInventory.instance.items.Add(new InventoryItem(data, data.MaxAmmo, data.StartReserveAmmo));
        }
        void Draw(int wID)
        {
            scrollPos = GUI.BeginScrollView(new Rect(0, 15, width, Screen.height * height - 20), scrollPos, new Rect(0, 0, width, 5 + WeaponController.instance.weapons.Count * (30 + 5)));
            if (GUI.Button(new Rect(5, 5, width - 10 - 10, 30), "<color=red>Close</color>"))
            {
                enabled = false;
            }
            int i = 1;
            foreach(WeaponData data in WeaponController.instance.weapons)
            {
                if (GUI.Button(new Rect(5, 5 + i * (30 + 5), width - 10 - 10, 30), data.DisplayName))
                {
                    GiveWeapon(data);
                }
                i++;
            }
            GUI.EndScrollView();
        }
        void DrawAlt(int wID)
        {
            if (GUI.Button(new Rect(5, 20, width - 10 - 10, 30), "<color=red>Close</color>"))
            {
                enabled = false;
            }
            GUI.Label(new Rect(5, 50, width - 10 - 10, 60), "<color=orange>Please\njoin map!</color>");
        }
    }
}
