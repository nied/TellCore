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

        [DllImport("TelldusCore.dll", CharSet = CharSet.Unicode)]
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
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = "out", MarshalTypeRef = typeof(TelldusUtf8Marshaler))]
        public static unsafe extern string tdGetDeviceParameter(
            int deviceId,
            char* name,
            char* defaultValue);

        [DllImport("TelldusCore.dll")]
        public static unsafe extern bool tdSetName(int deviceId, char* name);

        [DllImport("TelldusCore.dll")]
        public static unsafe extern bool tdSetProtocol(int deviceId, char* protocol);

        [DllImport("TelldusCore.dll")]
        public static unsafe extern bool tdSetModel(int deviceId, char* model);

        [DllImport("TelldusCore.dll")]
        public static unsafe extern bool tdSetDeviceParameter(int deviceId, char* name, char* value);

        [DllImport("TelldusCore.dll")]
        public static unsafe extern int tdAddDevice();

        [DllImport("TelldusCore.dll")]
        public static unsafe extern bool tdRemoveDevice(int deviceId);

        [DllImport("TelldusCore.dll")]
        public static unsafe extern int tdMethods(int deviceId, int methodsSupported);

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
        public static unsafe extern char* tdGetErrorString(int errorNo);

        [DllImport("TelldusCore.dll")]
        public static extern void tdClose();

        [DllImport("TelldusCore.dll")]
        public static extern void tdInit();

        [DllImport("TelldusCore.dll")]
        public static extern int tdLastSentCommand(int deviceId, int methods);

        [DllImport("TelldusCore.dll")]
        public static extern int tdGetDeviceType(int deviceId);

        [DllImport("TelldusCore.dll")]
        public static unsafe extern int tdSendRawCommand(char* command, int reserved);

        [DllImport("TelldusCore.dll")]
        public static extern int tdLearn(int deviceId);

        [DllImport("TelldusCore.dll")]
        public static unsafe extern char* tdLastSentValue(int deviceId);

        [DllImport("TelldusCore.dll")]
        public static unsafe extern void tdReleaseString(char* value);
        
        [DllImport("TelldusCore.dll")]
        public static extern void tdReleaseString(IntPtr value);

        [DllImport("TelldusCore.dll")]
        public static unsafe extern int tdUnregisterCallback(int eventId);

        [DllImport("TelldusCore.dll")]
        public static unsafe extern int tdRegisterDeviceEvent(Delegate deviceEventFunction, void* context);

        [DllImport("TelldusCore.dll")]
        public static unsafe extern int tdRegisterRawDeviceEvent(Delegate rawListeningFunction, void* context);

        [DllImport("TelldusCore.dll")]
        public static unsafe extern int tdRegisterDeviceChangeEvent(Delegate deviceChangeEventFunction, void* context);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public unsafe delegate void EventFunctionDelegate(int deviceId, int method, char* data, int callbackId, void* context);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public unsafe delegate void DeviceChangeEventFunctionDelegate(int deviceId, int changeEvent, int changeType, int callbackId, void* context);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public unsafe delegate void RawListeningDelegate(char* data, int controllerId, int callbackId, void* context);
    }
}