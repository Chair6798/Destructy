using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Destructy.Attaches
{
    internal class MainBehaviour : MonoBehaviour
    {
        internal static bool godMode = false;
        internal static bool noRecoil = false;
        void Awake()
        {
            Logging.Log("Destructy MainBehaviour Awake called.");
            gameObject.AddComponent<EspBehaviour>();
        }
        void OnGUI()
        {

        }
        
    }
}
