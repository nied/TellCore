using System;

namespace TellCore
{
    public partial class TellCoreClient
    {
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
