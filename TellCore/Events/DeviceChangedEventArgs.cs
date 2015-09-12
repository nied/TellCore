using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TellCore
{
    public class DeviceChangedEventArgs
    {
        public int DeviceId { get; set; }
        public DeviceChange DeviceChange { get; set; }
        public DeviceChangeType DeviceChangeType { get; set; }
    }
}
