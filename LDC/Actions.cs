using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LDC
{
    class Actions
    {
        public static Action<List<ListViewItem>> InitDevices;
        public static Action<int, string> UpdatePN;
        public static Action<int, string> UpdateState;
        public static Action<int, string> UpdateFileState;
    }
}
