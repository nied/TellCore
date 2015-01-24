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

        NativeMethods.EventFunctionDelegate deviceStateChangedDelegate;
        NativeMethods.DeviceChangeEventFunctionDelegate deviceChangedDelegate;
        NativeMethods.RawListeningDelegate rawDeviceEventDelegate;

        int eventDeviceChangedCallbackId = -1;
        int eventFunctionCallbackId = -1;
        int rawDeviceEventDelegateId = -1;

        public EventProxy()
        {
            deviceStateChangedDelegate = new NativeMethods.EventFunctionDelegate(OnDeviceStateChanged);
            deviceChangedDelegate = new NativeMethods.DeviceChangeEventFunctionDelegate(OnDeviceChanged);
            rawDeviceEventDelegate = new NativeMethods.RawListeningDelegate(OnRawDeviceEvent);

            NativeMethods.tdRegisterDeviceEvent(deviceStateChangedDelegate, IntPtr.Zero);
            NativeMethods.tdRegisterDeviceChangeEvent(deviceChangedDelegate, IntPtr.Zero);
            NativeMethods.tdRegisterRawDeviceEvent(rawDeviceEventDelegate, IntPtr.Zero);
        }

        private void OnDeviceStateChanged(int deviceId, int method, string data, int callbackId, IntPtr context)
        {
            eventFunctionCallbackId = callbackId;

            if (DeviceStateChanged == null)
                return;

            var args = new DeviceStateChangedEventArgs
            {
                DeviceId = deviceId,
                Method = (DeviceMethod)Enum.Parse(typeof(DeviceMethod), method.ToString()),
                Data = data
            };

            DeviceStateChanged(this, args);
        }

        private void OnDeviceChanged(int deviceId, int changeEvent, int changeType, int callbackId, IntPtr context)
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

        private void OnRawDeviceEvent(string data, int controllerId, int callbackId, IntPtr context)
        {
            eventDeviceChangedCallbackId = callbackId;

            if (DeviceChanged == null)
                return;

            var args = new RawDeviceEventArgs()
            {
                ControllerId = controllerId,
                Data = data
            };

            RawDeviceEvent(this, args);
        }

        public void Dispose()
        {
            if (eventDeviceChangedCallbackId != -1)
                NativeMethods.tdUnregisterCallback(eventDeviceChangedCallbackId);

            if (eventFunctionCallbackId != -1)
                NativeMethods.tdUnregisterCallback(eventFunctionCallbackId);

            if (rawDeviceEventDelegateId != -1)
                NativeMethods.tdUnregisterCallback(rawDeviceEventDelegateId);
        }
    }
}