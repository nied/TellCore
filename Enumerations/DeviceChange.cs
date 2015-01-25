using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TellCore
{
    public enum DeviceChange
    {
        /// <summary>
        /// A new device has been added 
        /// </summary>
        Added = 1,
        /// <summary>
        /// An existing device has been changed
        /// </summary>
        Changed = 2,
        /// <summary>
        /// An existing device has been removed
        /// </summary>
        Removed = 3,
        /// <summary>
        /// The state of an existing device has changed
        /// </summary>
        StateChanged = 4,
    }
}
