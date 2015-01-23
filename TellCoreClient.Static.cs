using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TellCore
{
    public partial class TellCoreClient
    {
        /// <summary>
        /// We must manually inform TelldusCore.dll that we're done with the string so it releases the pointer - or else we leak memory.
        /// </summary>
        /// <param name="pointer"></param>
        /// <returns></returns>
        private static unsafe string WithUnmanagedString(char* pointer)
        {
            string result = StringUtils.PointerToUtf8String((IntPtr)pointer);

            NativeMethods.tdReleaseString(pointer);

            return result;
        }

        private static DeviceType ToDeviceType(int result)
        {
            return (DeviceType)Enum.Parse(typeof(DeviceType), result.ToString());
        }

        private static DeviceMethod ToDeviceMethod(int result)
        {
            return (DeviceMethod)Enum.Parse(typeof(DeviceMethod), result.ToString());
        }

        private static TellstickResult ToTellstickResult(int result)
        {
            return (TellstickResult)Enum.Parse(typeof(TellstickResult), result.ToString());
        }
    }
}
