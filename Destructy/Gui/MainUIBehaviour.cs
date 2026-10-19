using UnityEngine;
using System.Collections.Generic;
using Destructy.Gui.FuncLists;
namespace Destructy.Gui
{
    internal class MainUIBehaviour : MonoBehaviour
    {
        List<BaseList> lists = new List<BaseList>();
        void Awake()
        {

        }
        void InitDefaultLists()
        {
            lists.Add(new SelfList());
        }
        void OnGUI()
        {

        }
    }
}
