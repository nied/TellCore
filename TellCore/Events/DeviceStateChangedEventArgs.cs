using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TellCore
{
    public class DeviceStateChangedEventArgs : EventArgs
    {
        public int DeviceId { get; set; }
        public DeviceMethod Method { get; set; }
        public string Data { get; set; }
    }
}
