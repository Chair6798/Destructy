using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Destructy.Gui.FuncLists
{
    internal class BaseList
    {
        internal string header;
        void DrawHeader()
        {
            
        }
        internal virtual void Draw()
        {
            DrawHeader();
        }
    }
}
