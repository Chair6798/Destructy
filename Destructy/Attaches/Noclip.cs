using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Destructy.Attaches
{
    internal class NoclipBehaviour : MonoBehaviour
    {
        Rigidbody rb;
        void Awake()
        {
            
        }
        void Update()
        {
            if (PlayerController.instance==null){return;}
            if(rb==null)
            {
                rb = PlayerController.instance.GetComponent<Rigidbody>();
            }
            rb.isKinematic = isEnabled();
            if(!isEnabled()) {return;}
            PlayerController.instance.transform.position += (Camera.main.transform.forward*((Input.GetKey(KeyCode.W)?1:0)- (Input.GetKey(KeyCode.S) ? 1 : 0)) + Camera.main.transform.right * ((Input.GetKey(KeyCode.D) ? 1 : 0) - (Input.GetKey(KeyCode.A) ? 1 : 0))) * 20 * (Input.GetKey(KeyCode.LeftShift) ? 2 : 1) * Time.deltaTime;
        }
        bool isEnabled()
        {
            return ElementCollector.GetBool("Movement", "noclip");
        }
    }
}
