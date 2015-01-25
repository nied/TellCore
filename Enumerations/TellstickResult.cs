using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TellCore
{
    public enum TellstickResult
    {
        Success = 0,
        ErrorNotFound = -1,
        ErrorPermissionDenied = -2,
        ErrorDeviceNotFound = -3,
        ErrorMethodNotSupported = -4,
        ErrorCommunication = -5,
        ErrorConnectingService = -6,
        ErrorUnknownResponse = -7,
        ErrorUnknown = -99,
    }
}
