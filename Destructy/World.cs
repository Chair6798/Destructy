using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Destructy
{
    internal static class World
    {
        internal static void CleanMap()
        {
            if(DestructionManager.instance!=null)
                DestructionManager.instance.DoDamage(new RaycastHit(), 2f, 999f);
        }
        internal static void RemoveFire()
        {
            foreach (var fire in UnityEngine.Object.FindObjectsOfType<Fire>())
            {
                fire.DestroyFire_ServerRpc();
            }
        }
    }
}
