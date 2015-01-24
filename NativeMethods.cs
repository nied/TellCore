using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using TellCore.Utils;

namespace TellCore
{
    internal static class NativeMethods
    {
        [DllImport("TelldusCore.dll")]
        public static extern int tdGetNumberOfDevices();

        [DllImport("TelldusCore.dll")]
        public static extern int tdGetDeviceId(int value);

        [DllImport("TelldusCore.dll")]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = "out", MarshalTypeRef = typeof(TelldusUtf8Marshaler))]
        public static extern string tdGetName(int deviceId);

        [DllImport("TelldusCore.dll")]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = "out", MarshalTypeRef = typeof(TelldusUtf8Marshaler))]
        public static extern string tdGetProtocol(int deviceId);

        [DllImport("TelldusCore.dll")]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = "out", MarshalTypeRef = typeof(TelldusUtf8Marshaler))]
        public static extern string tdGetModel(int deviceId);

        [DllImport("TelldusCore.dll")]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = "out", MarshalTypeRef = typeof(TelldusUtf8Marshaler))]
        public static extern string tdGetDeviceParameter(
            int deviceId,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = "in", MarshalTypeRef = typeof(TelldusUtf8Marshaler))] string name,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = "in", MarshalTypeRef = typeof(TelldusUtf8Marshaler))] string defaultValue);

        [DllImport("TelldusCore.dll")]
        public static extern bool tdSetName(
            int deviceId,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = "in", MarshalTypeRef = typeof(TelldusUtf8Marshaler))] string name);

        [DllImport("TelldusCore.dll")]
        public static extern bool tdSetProtocol(
            int deviceId,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = "in", MarshalTypeRef = typeof(TelldusUtf8Marshaler))] string protocol);

        [DllImport("TelldusCore.dll")]
        public static extern bool tdSetModel(
            int deviceId,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = "in", MarshalTypeRef = typeof(TelldusUtf8Marshaler))] string model);

        [DllImport("TelldusCore.dll")]
        public static extern bool tdSetDeviceParameter(
            int deviceId,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = "in", MarshalTypeRef = typeof(TelldusUtf8Marshaler))] string name,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = "in", MarshalTypeRef = typeof(TelldusUtf8Marshaler))] string value);

        [DllImport("TelldusCore.dll")]
        public static extern int tdAddDevice();

        [DllImport("TelldusCore.dll")]
        public static extern bool tdRemoveDevice(int deviceId);

        [DllImport("TelldusCore.dll")]
        public static extern int tdMethods(int deviceId, int methodsSupported);

        [DllImport("TelldusCore.dll")]
        public static extern int tdTurnOn(int deviceId);

        [DllImport("TelldusCore.dll")]
        public static extern int tdTurnOff(int deviceId);

        [DllImport("TelldusCore.dll")]
        public static extern int tdBell(int deviceId);

        [DllImport("TelldusCore.dll")]
        public static extern int tdDim(int deviceId, char level);

        [DllImport("TelldusCore.dll")]
        public static extern int tdExecute(int deviceId);

        [DllImport("TelldusCore.dll")]
        public static extern int tdUp(int deviceId);

        [DllImport("TelldusCore.dll")]
        public static extern int tdDown(int deviceId);

        [DllImport("TelldusCore.dll")]
        public static extern int tdStop(int deviceId);

        [DllImport("TelldusCore.dll")]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = "out", MarshalTypeRef = typeof(TelldusUtf8Marshaler))]
        public static extern string tdGetErrorString(int errorNo);

        [DllImport("TelldusCore.dll")]
        public static extern void tdClose();

        [DllImport("TelldusCore.dll")]
        public static extern void tdInit();

        [DllImport("TelldusCore.dll")]
        public static extern int tdLastSentCommand(int deviceId, int methods);

        [DllImport("TelldusCore.dll")]
        public static extern int tdGetDeviceType(int deviceId);

        [DllImport("TelldusCore.dll")]
        public static extern int tdSendRawCommand(
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = "in", MarshalTypeRef = typeof(TelldusUtf8Marshaler))]string command, 
            int reserved);

        [DllImport("TelldusCore.dll")]
        public static extern int tdLearn(int deviceId);

        [DllImport("TelldusCore.dll")]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = "out", MarshalTypeRef = typeof(TelldusUtf8Marshaler))]
        public static extern string tdLastSentValue(int deviceId);
        
        [DllImport("TelldusCore.dll")]
        public static extern void tdReleaseString(IntPtr value);

        [DllImport("TelldusCore.dll")]
        public static extern int tdUnregisterCallback(int eventId);

        [DllImport("TelldusCore.dll")]
        public static extern int tdRegisterDeviceEvent(Delegate deviceEventFunction, IntPtr context);

        [DllImport("TelldusCore.dll")]
        public static extern int tdRegisterRawDeviceEvent(Delegate rawListeningFunction, IntPtr context);

        [DllImport("TelldusCore.dll")]
        public static extern int tdRegisterDeviceChangeEvent(Delegate deviceChangeEventFunction, IntPtr context);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void EventFunctionDelegate(
            int deviceId, 
            int method, 
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = "in", MarshalTypeRef = typeof(TelldusUtf8Marshaler))] string data, 
            int callbackId, 
            IntPtr context);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void DeviceChangeEventFunctionDelegate(int deviceId, int changeEvent, int changeType, int callbackId, IntPtr context);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void RawListeningDelegate(
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = "in", MarshalTypeRef = typeof(TelldusUtf8Marshaler))] string data, 
            int controllerId, 
            int callbackId, 
            IntPtr context);
    }
}