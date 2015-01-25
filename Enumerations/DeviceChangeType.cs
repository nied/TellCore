using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TellCore
{
    public enum DeviceChangeType
    {
        /// <summary>
        /// The name of an existing device has changed
        /// </summary>
        Name = 1,
        /// <summary>
        /// The protocol of an existing device has changed
        /// </summary>
        Protocol = 2,
        /// <summary>
        /// The model of an existing device has changed
        /// </summary>
        Model = 3,
    }
}
