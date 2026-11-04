using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Destructy
{
    internal class MinigunBuilder
    {
        internal static void Build()
        {
            var results = Resources.FindObjectsOfTypeAll(typeof(GameObject))
                .OfType<GameObject>()
                .Where(go => go.name.IndexOf("Minigun", System.StringComparison.OrdinalIgnoreCase) >= 0)
                .ToList();
            GameObject model = null;
            foreach (var go in results)
            {
                model = UnityEngine.Object.Instantiate(go);
            }
            if (model == null) { Debug.Log("nope"); return; }
            var muzzle0 = WeaponController.instance.weapons[21].Muzzle;
            var data = new WeaponData();
            data.ViewModel = model;
            var hands = GameObject.Find("Bone.008");
            var mno = model.GetComponent<Unity.Netcode.NetworkObject>();
            UnityEngine.Object.DestroyImmediate(mno);
            model.transform.parent = hands.transform;
            data.Weaponid = 25;
            data.HitSound = WeaponController.instance.weapons[21].HitSound;
            data.PlayerDamage = 5;
            data.name = "minigun";
            data.DisplayName = "Minigun";
            data.MaxAmmo = 300;
            data.StartReserveAmmo = 100;
            data.AdditionalObjects = new List<GameObject>();
            data.DestructionRadius = 0.1f;
            data.DestructionVoxelSize = 0.1f;
            data.CustomShootInterval = 0.05f;
            data.Muzzle = muzzle0;
            data.Recoil = new Vector2(3, 3);
            model.transform.localPosition = new Vector3(1.2909f, 0.0418f, 1.2036f);
            model.transform.localRotation = Quaternion.Euler(0f, 90f, 270f);
            WeaponController.instance.weapons.Add(data);
            PlayerInventory.instance.items.Add(new InventoryItem(data, 999, 999));
        }
        internal static bool IsMinigunAvailable()
        {
            return WeaponController.instance != null&&PlayerInventory.instance != null;
        }
    }
}
