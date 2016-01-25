using System;
using System.Runtime.InteropServices;
using TellCore.Utils;

namespace TellCore
{
    internal static class NativeMethods
    {
        [DllImport("TelldusCore.dll")]
        internal static extern int tdGetNumberOfDevices();

        [DllImport("TelldusCore.dll")]
        internal static extern int tdGetDeviceId(int value);

        [DllImport("TelldusCore.dll")]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = "out", MarshalTypeRef = typeof(TelldusUtf8Marshaler))]
        internal static extern string tdGetName(int deviceId);

        [DllImport("TelldusCore.dll")]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = "out", MarshalTypeRef = typeof(TelldusUtf8Marshaler))]
        internal static extern string tdGetProtocol(int deviceId);

        [DllImport("TelldusCore.dll")]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = "out", MarshalTypeRef = typeof(TelldusUtf8Marshaler))]
        internal static extern string tdGetModel(int deviceId);

        [DllImport("TelldusCore.dll")]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = "out", MarshalTypeRef = typeof(TelldusUtf8Marshaler))]
        internal static extern string tdGetDeviceParameter(
            int deviceId,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = "in", MarshalTypeRef = typeof(TelldusUtf8Marshaler))] string name,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = "in", MarshalTypeRef = typeof(TelldusUtf8Marshaler))] string defaultValue);

        [DllImport("TelldusCore.dll")]
        internal static extern bool tdSetName(
            int deviceId,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = "in", MarshalTypeRef = typeof(TelldusUtf8Marshaler))] string name);

        [DllImport("TelldusCore.dll")]
        internal static extern bool tdSetProtocol(
            int deviceId,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = "in", MarshalTypeRef = typeof(TelldusUtf8Marshaler))] string protocol);

        [DllImport("TelldusCore.dll")]
        internal static extern bool tdSetModel(
            int deviceId,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = "in", MarshalTypeRef = typeof(TelldusUtf8Marshaler))] string model);

        [DllImport("TelldusCore.dll")]
        internal static extern bool tdSetDeviceParameter(
            int deviceId,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = "in", MarshalTypeRef = typeof(TelldusUtf8Marshaler))] string name,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = "in", MarshalTypeRef = typeof(TelldusUtf8Marshaler))] string value);

        [DllImport("TelldusCore.dll")]
        internal static extern int tdAddDevice();

        [DllImport("TelldusCore.dll")]
        internal static extern bool tdRemoveDevice(int deviceId);

        [DllImport("TelldusCore.dll")]
        internal static extern DeviceMethod tdMethods(int deviceId, DeviceMethod methodsSupported);

        [DllImport("TelldusCore.dll")]
        internal static extern TellstickResult tdTurnOn(int deviceId);

        [DllImport("TelldusCore.dll")]
        internal static extern TellstickResult tdTurnOff(int deviceId);

        [DllImport("TelldusCore.dll")]
        internal static extern TellstickResult tdBell(int deviceId);

        [DllImport("TelldusCore.dll")]
        internal static extern TellstickResult tdDim(int deviceId, byte level);

        [DllImport("TelldusCore.dll")]
        internal static extern TellstickResult tdExecute(int deviceId);

        [DllImport("TelldusCore.dll")]
        internal static extern TellstickResult tdUp(int deviceId);

        [DllImport("TelldusCore.dll")]
        internal static extern TellstickResult tdDown(int deviceId);

        [DllImport("TelldusCore.dll")]
        internal static extern TellstickResult tdStop(int deviceId);

        [DllImport("TelldusCore.dll")]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = "out", MarshalTypeRef = typeof(TelldusUtf8Marshaler))]
        internal static extern string tdGetErrorString(TellstickResult errorNo);

        [DllImport("TelldusCore.dll")]
        internal static extern void tdClose();

        [DllImport("TelldusCore.dll")]
        internal static extern void tdInit();

        [DllImport("TelldusCore.dll")]
        internal static extern DeviceMethod tdLastSentCommand(int deviceId, DeviceMethod methods);

        [DllImport("TelldusCore.dll")]
        internal static extern DeviceType tdGetDeviceType(int deviceId);

        [DllImport("TelldusCore.dll")]
        internal static extern TellstickResult tdSendRawCommand(
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = "in", MarshalTypeRef = typeof(TelldusUtf8Marshaler))]string command, 
            int reserved);

        [DllImport("TelldusCore.dll")]
        internal static extern int tdLearn(int deviceId);

        [DllImport("TelldusCore.dll")]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = "out", MarshalTypeRef = typeof(TelldusUtf8Marshaler))]
        internal static extern string tdLastSentValue(int deviceId);
        
        [DllImport("TelldusCore.dll")]
        internal static extern void tdReleaseString(IntPtr value);

        [DllImport("TelldusCore.dll")]
        internal static extern int tdUnregisterCallback(int eventId);

        [DllImport("TelldusCore.dll")]
        internal static extern int tdRegisterDeviceEvent(EventFunctionDelegate deviceEventFunction, IntPtr context);

        [DllImport("TelldusCore.dll")]
        internal static extern int tdRegisterRawDeviceEvent(RawListeningDelegate rawListeningFunction, IntPtr context);

        [DllImport("TelldusCore.dll")]
        internal static extern int tdRegisterDeviceChangeEvent(DeviceChangeEventFunctionDelegate deviceChangeEventFunction, IntPtr context);

        [DllImport("TelldusCore.dll")]
        public static extern TellstickResult tdSensor(
            IntPtr protocol,
            int protocolLength, 
            IntPtr model, 
            int modelLength,
            ref int id,
            ref int dataTypes);

        [DllImport("TelldusCore.dll")]
        public static extern int tdSensorValue(
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = "in", MarshalTypeRef = typeof(TelldusUtf8Marshaler))]string protocol,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = "in", MarshalTypeRef = typeof(TelldusUtf8Marshaler))]string model, 
            int id, 
            int dataType,
            IntPtr value, 
            int valueLength, 
            ref int timestamp);
        
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate void EventFunctionDelegate(
            int deviceId, 
            int method, 
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = "out", MarshalTypeRef = typeof(TelldusUtf8Marshaler))] string data, 
            int callbackId, 
            IntPtr context);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate void DeviceChangeEventFunctionDelegate(int deviceId, int changeEvent, int changeType, int callbackId, IntPtr context);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate void RawListeningDelegate(
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = "out", MarshalTypeRef = typeof(TelldusUtf8Marshaler))] string data, 
            int controllerId, 
            int callbackId, 
            IntPtr context);
    }
}