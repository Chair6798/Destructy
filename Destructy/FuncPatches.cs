using Epic.OnlineServices.Presence;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
namespace Destructy
{
    [HarmonyPatch(typeof(NetworkPlayer))]
    internal static class NetworkPlayerPatches
    {
        [HarmonyPatch("Update")]
        [HarmonyPrefix]
        private static bool Update_Prefix(NetworkPlayer __instance, float ___VisibleForAiTimer, NetworkObject ___netObj, NetworkPlayer ___ClientPlayer, float ___accel, float ___accelLerp, NetworkVariable<Vector3> ___Velocity, Animator ___Anim, float ___speedModifier, Text ___nickText, Transform ___NicknamePlate)
        {
            if(ElementCollector.GetBool("Status", "invisibility"))
            {
                if (___VisibleForAiTimer >= 0f)
                {
                    ___VisibleForAiTimer -= Time.deltaTime;
                }

                if (___netObj.IsOwner)
                {
                    ___ClientPlayer = __instance;
                    if (PlayerController.instance != null)
                    {
                        __instance.transform.position = new Vector3(0,-100,0);
                        __instance.transform.rotation = Quaternion.identity;
                    }
                }

                ___accel = Mathf.Lerp(___accel, ___Velocity.Value.magnitude, ___accelLerp);
                ___Anim.SetFloat("Forward", ___accel * ___speedModifier);
                if (Camera.main != null)
                {
                    ___NicknamePlate.transform.LookAt(Camera.main.transform);
                    ___nickText.color = new Color(1f, 1f, 1f, 10f - Vector3.Distance(Camera.main.transform.position, ___NicknamePlate.position));
                }
                return false;
            }
            return true;
        }
    }
    [HarmonyPatch(typeof(PlayerController))]
    internal static class PlayerControllerPatch
    {
        [HarmonyPatch("Damage")]
        [HarmonyPrefix]
        private static bool Damage_Prefix()
        {
            return !ElementCollector.GetBool("Status", "godmode");
        }
    }
    [HarmonyPatch(typeof(PlayerCameraControl))]
    internal static class PlayerCameraControlPatch
    {
        [HarmonyPatch("ApplyRecoil")]
        [HarmonyPrefix]
        private static bool ApplyRecoil_Prefix()
        {
            return !ElementCollector.GetBool("Weapon", "no_recoil");
        }

    }
    [HarmonyPatch(typeof(Tank))]
    internal static class TankPatch
    {
        //[HarmonyPatch("Shoot")]
        //[HarmonyPrefix]
        //private static bool Shoot_Prefix(Tank __instance, Transform ___TankMuzzleEnd, float ___ShootForce)
        //{
        //    __instance.Shoot_ServerRpc(___TankMuzzleEnd.transform.position + ___TankMuzzleEnd.transform.forward, ___TankMuzzleEnd.transform.forward);
        //    __instance.GetComponent<Rigidbody>().AddForceAtPosition(-___TankMuzzleEnd.transform.forward * ___ShootForce, ___TankMuzzleEnd.transform.position, ForceMode.Impulse);
        //    return false;
        //}
        [HarmonyPatch("ClampAngle")]
        [HarmonyPrefix]
        private static bool ClampAngle_Prefix(float current, ref float __result)
        {
            if(ElementCollector.GetBool("Vehicle", "free_aim"))
            {
                __result = current;
                return false;
            }
            return true;
        }
    }
    [HarmonyPatch(typeof(Damagable))]
    internal static class DamagablePatcher
    {
        [HarmonyPatch("NetDamage")]
        [HarmonyPrefix]
        private static bool NetDamagePatch(Damagable __instance, ref float force, ref int dType)
        {
            if(ElementCollector.GetBool("Weapon", "one_hit"))
            {
                force = 9999f;
                if (__instance.GetComponent<Vehicle>() != null)
                {
                    dType = (int)DamageData.DamageType.Explosion;
                }

            }
            return true;
        }
        //[HarmonyPatch("Damage")]
        //[HarmonyPrefix]
        //private static bool DamagePatch(Damagable __instance)
        //{
        //    if(__instance.GetComponent<Vehicle>()!=null)
        //    {
        //        return !ElementCollector.GetBool("Vehicle", "unbreak");
        //    }
        //    return true;
        //}
    }
    [HarmonyPatch(typeof(MainMenu))]
    internal static class MainMenuPatch
    {
        [HarmonyPatch("GetPlayerName")]
        [HarmonyPrefix]
        private static bool GetPlayerName_Prefix(ref string __result)
        {
            if (ElementCollector.GetBool("Status", "change_nickname"))
            {
                __result = ElementCollector.GetString("Status", "custom_nickname");
                return false;
            }
            return true;
        }
    }
    [HarmonyPatch(typeof(Chat))]
    internal static class ChatPatch
    {
        [HarmonyPatch("SendChatMessage")]
        [HarmonyPrefix]
        private static bool SendChatMessage_Prefix(InputField ___text)
        {
            if (ElementCollector.GetEnum("Status", "custom_message_mode") > 0)
            {
                int enumVal = ElementCollector.GetEnum("Status", "custom_message_mode");
                switch (enumVal)
                {
                    case 1:
                        ___text.text = ElementCollector.GetString("Status", "custom_message_style").Replace("msg", ___text.text);
                        break;
                    case 2:
                        ___text.text = ChatMessagePresetFormat.Format(ElementCollector.GetString("Status", "custom_message_style"), ___text.text);
                        break;
                }
                    
                
            }
            return true;
        }
    }
    [HarmonyPatch(typeof(DestructionManager))]
    internal static class DestructionManagerPatch
    {
        [HarmonyPatch("DoDamage")]
        [HarmonyPrefix]
        private static bool DoDamage_Prefix(DestructionManager __instance, ref float voxelSize, ref float damageRadius, bool NetHit)
        {
            if (NetHit) { return true; }
            float f = ElementCollector.GetFloat("World", "destruction_multiplier");
            voxelSize = voxelSize * f;
            damageRadius = damageRadius * f;
            return true;
        }
    }
    [HarmonyPatch(typeof(Networking))]
    internal static class NetworkingPatch
    {
        [HarmonyPatch("PlayerData_ServerRpc")]
        [HarmonyPrefix]
        private static bool PlayerData_ServerRpc_Prefix(ref int platform)
        {
            if (ElementCollector.GetEnum("Status", "fake_device") > 0)
            {
                platform = ElementCollector.GetEnum("Status", "fake_device") - 1;
            }

            return true;
        }
    }
}
