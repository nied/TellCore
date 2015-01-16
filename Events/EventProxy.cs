using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TellCore
{
    /// <summary>
    /// This class is meant to act as a proxy for the events provided by TellCore.dll.
    /// We could just expose the events but that would mean calling code would have to deal with char-pointers
    /// and the like. 
    /// </summary>
    public class EventProxy : IDisposable
    {
        public event DeviceChangedHandler DeviceChanged;
        public event DeviceStateChangedHandler DeviceStateChanged;
        public event RawDeviceEventHandler RawDeviceEvent;

        public delegate void DeviceChangedHandler(object sender, DeviceChangedEventArgs e);
        public delegate void DeviceStateChangedHandler(object sender, DeviceStateChangedEventArgs e);
        public delegate void RawDeviceEventHandler(object sender, RawDeviceEventArgs e);

        UnmanagedWrapper.EventFunctionDelegate deviceStateChangedDelegate;
        UnmanagedWrapper.DeviceChangeEventFunctionDelegate deviceChangedDelegate;
        UnmanagedWrapper.RawListeningDelegate rawDeviceEventDelegate;

        int eventDeviceChangedCallbackId = -1;
        int eventFunctionCallbackId = -1;
        int rawDeviceEventDelegateId = -1;

        public EventProxy()
        {
            unsafe
            {
                deviceStateChangedDelegate = new UnmanagedWrapper.EventFunctionDelegate(OnDeviceStateChanged);
                deviceChangedDelegate = new UnmanagedWrapper.DeviceChangeEventFunctionDelegate(OnDeviceChanged);
                rawDeviceEventDelegate = new UnmanagedWrapper.RawListeningDelegate(OnRawDeviceEvent);

                UnmanagedWrapper.tdRegisterDeviceEvent(deviceStateChangedDelegate, null);
                UnmanagedWrapper.tdRegisterDeviceChangeEvent(deviceChangedDelegate, null);
                UnmanagedWrapper.tdRegisterRawDeviceEvent(rawDeviceEventDelegate, null);
            }
        }

        private unsafe void OnDeviceStateChanged(int deviceId, int method, char* data, int callbackId, void* context)
        {
            eventFunctionCallbackId = callbackId;

            if (DeviceStateChanged == null)
                return;

            var args = new DeviceStateChangedEventArgs
            {
                DeviceId = deviceId,
                Method = (DeviceMethod)Enum.Parse(typeof(DeviceMethod), method.ToString()),
                Data = StringUtils.PointerToUtf8String((IntPtr)data)
            };

            DeviceStateChanged(this, args);
        }

        private unsafe void OnDeviceChanged(int deviceId, int changeEvent, int changeType, int callbackId, void* context)
        {
            eventDeviceChangedCallbackId = callbackId;

            if (DeviceChanged == null)
                return;

            var args = new DeviceChangedEventArgs
            {
                DeviceId = deviceId,
                DeviceChange = (DeviceChange)Enum.Parse(typeof(DeviceChange), changeEvent.ToString()),
                DeviceChangeType = (DeviceChangeType)Enum.Parse(typeof(DeviceChangeType), changeType.ToString())
            };

            DeviceChanged(this, args);
        }

        private unsafe void OnRawDeviceEvent(char* data, int controllerId, int callbackId, void* context)
        {
            eventDeviceChangedCallbackId = callbackId;

            if (DeviceChanged == null)
                return;

            var args = new RawDeviceEventArgs()
            {
                ControllerId = controllerId,
                Data = StringUtils.PointerToUtf8String((IntPtr)data),
            };
            
            RawDeviceEvent(this, args);
        }

        public void Dispose()
        {
            if (eventDeviceChangedCallbackId != -1)
                UnmanagedWrapper.tdUnregisterCallback(eventDeviceChangedCallbackId);

            if (eventFunctionCallbackId != -1)
                UnmanagedWrapper.tdUnregisterCallback(eventFunctionCallbackId);

            if (rawDeviceEventDelegateId != -1)
                UnmanagedWrapper.tdUnregisterCallback(rawDeviceEventDelegateId);
        }
    }
}