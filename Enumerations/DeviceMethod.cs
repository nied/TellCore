using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TellCore
{
    /// <summary>
    /// Available Tellstick commands. Can be used with 
    /// the <see cref="TellCoreClient.GetMethods"/> to retrieve 
    /// a bitmask of all commands supported by a particular device
    /// </summary>
    [Flags]
    public enum DeviceMethod
    {
        TurnOn = 1,
        TurnOff = 2,
        Bell = 4,
        Toggle = 8,
        Dim = 16,
        Learn = 32,
        Execute = 64,
        Up = 128,
        Down = 256,
        Stop = 512,
    }
}
