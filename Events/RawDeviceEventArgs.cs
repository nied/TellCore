using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TellCore
{
    public class RawDeviceEventArgs
    {
        public int ControllerId { get; set; }
        public string Data { get; set; }
    }
}
