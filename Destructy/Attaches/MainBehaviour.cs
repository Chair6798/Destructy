using Destructy.Gui;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Burst.Intrinsics;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Assertions;
using static UnityEngine.Rendering.DebugUI.Table;

namespace Destructy.Attaches
{
    internal class MainBehaviour : MonoBehaviour
    {
        float rocket_timeout = 0f;

        float toyotaYarisMod = 1f;
        bool toyotaYarisAnimateDirection = false;

        void Awake()
        {
            Logging.Log("Destructy MainBehaviour Awake called.");
            gameObject.AddComponent<EspBehaviour>();
            gameObject.AddComponent<MainUIBehaviour>();
            gameObject.AddComponent<NoclipBehaviour>();
            gameObject.AddComponent<WeaponGetMenu>();
            (ElementCollector.GetEvent("Weapon", "get_menu")).AddListener(() =>
            {
                WeaponGetMenu.instance.enabled = true;
            });
            (ElementCollector.GetEvent("Weapon", "get_minigun")).AddListener(() =>
            {
                MinigunBuilder.Build();
            });
            (ElementCollector.GetEvent("World", "clean_map")).AddListener(() =>
            {
                World.CleanMap();
            });
            (ElementCollector.GetEvent("World", "remove_fire")).AddListener(() =>
            {
                World.RemoveFire();
            });
            //(ElementCollector.GetEvent("Vehicle", "eject")).AddListener(() =>
            //{
            //    Vehicle veh = Lib.GetLookingAt().GetComponent<Vehicle>();
            //    if(veh==null)
            //    {
            //        return;
            //    }
            //    var driver = veh.OwnerClientId;
            //    veh.VehicleLeft_ClientRpc();
            //    GetComponent<NetworkObject>().ChangeOwnership(veh.NetworkManager.LocalClientId);
            //    veh.DriverSeatOccupied.Value = false;
            //    Networking.instance.GetNetworkPlayer(driver).PlayerLeftVehicle_ClientRpc();
            //    veh.GetType().GetField("driver", System.Reflection.BindingFlags.NonPublic| System.Reflection.BindingFlags.Instance).SetValue(veh, null);
            //});
        }
        void Update()
        {
            if (PlayerInventory.instance != null && PlayerInventory.instance.WeaponInHands != null && ElementCollector.GetBool("Weapon", "infinite_ammo"))
                PlayerInventory.instance.WeaponInHands.Ammo = 9999;
            //
            if (ElementCollector.GetBool("Weapon", "rapid_fire") && WeaponController.instance != null)
                Fields.weapon_controller_elapsed.SetValue(WeaponController.instance, -1f);
            if (ElementCollector.GetBool("Vehicle", "rapid_fire") && PlayerController.instance!=null && PlayerController.instance.vehicleIn!=null)
            {
                Tank tank = PlayerController.instance.vehicleIn.GetComponent<Tank>();
                if (tank != null)
                {
                    tank.ReloadDurationElapsed = -1f;
                }
                Helicopter heli = PlayerController.instance.vehicleIn.GetComponent<Helicopter>();
                if (heli != null)
                {
                    heli.ReloadDurationElapsed = -1f;
                }
            }
            // ReloadDurationElapsed
            if (ElementCollector.GetBool("World", "rocket_launcher") && WeaponController.instance != null && rocket_timeout > 0.05)
            {
                Fields.weapon_controller_projectile_serverRPC.Invoke(WeaponController.instance, new object[] { 2, PlayerController.instance.transform.position + new Vector3(0, 1, 0), -Camera.main.transform.forward, new ServerRpcParams() });
                rocket_timeout = 0f;
            }
            rocket_timeout += Time.deltaTime;
            // Minigun Element Update
            ((FuncElement)ElementCollector.GetElement("Weapon", "get_minigun")).Avaiable = MinigunBuilder.IsMinigunAvailable();
            // Car update
            if (PlayerController.instance != null)
            {
                if (PlayerController.instance.InVehicle)
                {
                    Vehicle veh = PlayerController.instance.vehicleIn;
                    veh.Torque = Lib.GetDefaultTorque(veh.gameObject.name) * ElementCollector.GetFloat("Vehicle", "torque_modifier");
                    if(ElementCollector.GetBool("Vehicle", "no_speed_limit"))
                    {
                        veh.MaxSpeed = float.MaxValue;
                    }
                    float scaleMod = ElementCollector.GetFloat("Vehicle", "scale");
                    if (ElementCollector.GetBool("Vehicle", "toyotayaris"))
                    {
                        toyotaYarisMod += (toyotaYarisAnimateDirection ? 1 : -1) * 2 * Time.deltaTime;
                    }
                    else
                    {
                        toyotaYarisMod = 1f;
                    }

                    if (toyotaYarisMod > 1.8f)
                    {
                        toyotaYarisMod = 1.8f;
                        toyotaYarisAnimateDirection = false;
                    }
                    if (toyotaYarisMod < 0.2f)
                    {
                        toyotaYarisMod = 0.2f;
                        toyotaYarisAnimateDirection = true;
                    }
                    veh.transform.localScale = new Vector3(scaleMod, scaleMod * toyotaYarisMod, scaleMod);
                }
            }
        }
    }
}
