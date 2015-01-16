using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TellCore
{
    public enum TellstickResult
    {
        SUCCESS = 0,
        ERROR_NOT_FOUND = -1,
        ERROR_PERMISSION_DENIED = -2,
        ERROR_DEVICE_NOT_FOUND = -3,
        ERROR_METHOD_NOT_SUPPORTED = -4,
        ERROR_COMMUNICATION = -5,
        ERROR_CONNECTING_SERVICE = -6,
        ERROR_UNKNOWN_RESPONSE = -7,
        ERROR_UNKNOWN = -99,
    }
}
