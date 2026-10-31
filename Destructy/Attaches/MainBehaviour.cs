using Destructy.Gui;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

namespace Destructy.Attaches
{
    internal class MainBehaviour : MonoBehaviour
    {
        float rocket_timeout = 0f;
        void Awake()
        {
            Logging.Log("Destructy MainBehaviour Awake called.");
            gameObject.AddComponent<EspBehaviour>();
            gameObject.AddComponent<MainUIBehaviour>();
            gameObject.AddComponent<NoclipBehaviour>();
            gameObject.AddComponent<WeaponGetMenu>();
            ((FuncElement)ElementCollector.GetElement("Weapon", "get_menu")).Event.AddListener(() =>
            {
                WeaponGetMenu.instance.enabled = true;
            });
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
            //
            
        }
    }
}
