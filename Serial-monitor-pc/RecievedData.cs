using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serial_monitor_pc
{
    class RecievedData
    {
        public int potValue {  get; set; }
        public bool eindeloop { get; set; }
        public bool knop1 { get; set; }
        public bool knop2 { get; set; }
    }
}
