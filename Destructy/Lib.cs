using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SubsystemsImplementation;

namespace Destructy
{
    internal static class Lib
    {
        internal static Collider GetLookingAt()
        {
            Collider LookingAt=null;
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out var hit, 3f, ~LayerMask.GetMask("LocalPlayer"), QueryTriggerInteraction.Ignore))
            {
                LookingAt = hit.collider;
            }
            return LookingAt;
        }
        internal static float GetDefaultTorque(string name)
        {
            name = name.Replace("(Clone)", string.Empty);

            switch(name)
            {
                case "SportsCar":
                    return 4000;
                case "Hatchback 2":
                    return 1500;
                case "Hatchback":
                    return 2000;
                case "Tank":
                    return 300;
                case "Heli":
                    return 1200;
                case "M270_MLRS":
                    return 900;
            }


            return 2000;
        }
    }
}
