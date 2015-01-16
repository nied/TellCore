using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TellCore
{
    public enum DeviceChange
    {
        TELLSTICK_DEVICE_ADDED = 1,
        TELLSTICK_DEVICE_CHANGED = 2,
        TELLSTICK_DEVICE_REMOVED = 3,
        TELLSTICK_DEVICE_STATE_CHANGED = 4,
    }
}
