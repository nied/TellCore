using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TellCore
{
    [Flags]
    public enum DeviceMethod
    {
        TELLSTICK_TURNON = 1,
        TELLSTICK_TURNOFF = 2,
        TELLSTICK_BELL = 4,
        TELLSTICK_TOGGLE = 8,
        TELLSTICK_DIM = 16,
        TELLSTICK_LEARN = 32,
        TELLSTICK_EXECUTE = 64,
        TELLSTICK_UP = 128,
        TELLSTICK_DOWN = 256,
        TELLSTICK_STOP = 512,
    }
}
