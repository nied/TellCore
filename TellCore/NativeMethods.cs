using System;
using System.Runtime.InteropServices;
using TellCore.Enumerations;
using TellCore.Utils;
using static TellCore.NativeMethods;

namespace TellCore
{
    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void DeviceStateChangeEventDelegate(
        int deviceId,
        DeviceMethod method,
        [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = "out", MarshalTypeRef = typeof(TelldusUtf8Marshaler))] string data,
        int callbackId,
        IntPtr context);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void DeviceChangeEventDelegate(int deviceId, int changeEvent, int changeType, int callbackId, IntPtr context);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void RawDeviceEventDelegate(
        [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = "out", MarshalTypeRef = typeof(TelldusUtf8Marshaler))] string data,
        int controllerId,
        int callbackId,
        IntPtr context);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void SensorEventDelegate(
        [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = "out", MarshalTypeRef = typeof(TelldusUtf8Marshaler))] string protocol,
        [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = "out", MarshalTypeRef = typeof(TelldusUtf8Marshaler))] string model,
        int deviceId,
        SensorDataType dataType,
        [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = "out", MarshalTypeRef = typeof(TelldusUtf8Marshaler))] string value,
        int timeStamp,
        int callbackId,
        IntPtr context);

    public interface ITelldusCoreProxy
    {
        int AddDevice();
        void Bell(int deviceId);
        void Close();
        void Dim(int deviceId, int level);
        void Down(int deviceId);
        void Execute(int deviceId);
        int GetDeviceId(int value);
        string GetDeviceParameter(int deviceId, string name, string defaultValue);
        DeviceType GetDeviceType(int deviceId);
        string GetModel(int deviceId);
        string GetName(int deviceId);
        int GetNumberOfDevices();
        string GetProtocol(int deviceId);
        void Init();
        DeviceMethod GetLastSentCommand(int deviceId);
        string GetLastSentValue(int deviceId);
        void Learn(int deviceId);
        DeviceMethod GetMethods(int deviceId);
        int RegisterDeviceChangeEvent(DeviceChangeEventDelegate deviceChangeEventFunction);
        int RegisterDeviceEvent(DeviceStateChangeEventDelegate deviceEventFunction);
        int RegisterRawDeviceEvent(RawDeviceEventDelegate rawListeningFunction);
        int RegisterSensorEvent(SensorEventDelegate sensorEventFunction);
        void RemoveDevice(int deviceId);
        void SendRawCommand(string command);
        void SetDeviceParameter(int deviceId, string name, string value);
        void SetModel(int deviceId, string model);
        void SetName(int deviceId, string name);
        void SetProtocol(int deviceId, string protocol);
        void Stop(int deviceId);
        void TurnOff(int deviceId);
        void TurnOn(int deviceId);
        void UnregisterCallback(int eventId);
        void Up(int deviceId);
    }

    internal class TelldusCoreProxy : ITelldusCoreProxy
    {
        private static readonly object LockKey = new object();

        private static int CheckReturnCode(Func<int> func, string methodName, params object[] args)
        {
            int result = Lock(func);
            if (result >= 0)
                return result;
            throw new TellCoreException($"{methodName}({FormatArgs(args)}): {tdGetErrorString(result)}");
        }

        private static string FormatArgs(object[] args)
        {
            return string.Join(", ", args);
        }

        private static void CheckReturnCode(Func<bool> func, string methodName, params object[] args)
        {
            bool result = Lock(func);
            if (result)
                return;
            throw new TellCoreException($"{methodName}({FormatArgs(args)}): failed");
        }

        private static T Lock<T>(Func<T> func)
        {
            T result;
            lock (LockKey)
            {
                result = func();
            }
            return result;
        }

        public int GetNumberOfDevices() { return CheckReturnCode(() => tdGetNumberOfDevices(), nameof(GetNumberOfDevices)); }

        public int GetDeviceId(int value) { return CheckReturnCode(() => tdGetDeviceId(value), nameof(GetDeviceId), value); }

        public string GetName(int deviceId) { return Lock(() => tdGetName(deviceId)); }

        public string GetProtocol(int deviceId) { return Lock(() => tdGetProtocol(deviceId)); }

        public string GetModel(int deviceId) { return Lock(() => tdGetModel(deviceId)); }

        public string GetDeviceParameter(int deviceId, string name, string defaultValue) { return Lock(() => tdGetDeviceParameter(deviceId, name, defaultValue)); }

        public void SetName(int deviceId, string name) { CheckReturnCode(() => tdSetName(deviceId, name), nameof(SetName), deviceId, name); }

        public void SetProtocol(int deviceId, string protocol) { CheckReturnCode(() => tdSetProtocol(deviceId, protocol), nameof(SetProtocol), deviceId, protocol); }

        public void SetModel(int deviceId, string model) { CheckReturnCode(() => tdSetModel(deviceId, model), nameof(SetModel), deviceId, model); }

        public void SetDeviceParameter(int deviceId, string name, string value) { CheckReturnCode(() => tdSetDeviceParameter(deviceId, name, value), nameof(SetDeviceParameter), deviceId, name, value); }

        public int AddDevice() { return CheckReturnCode(() => tdAddDevice(), nameof(AddDevice)); }

        public void RemoveDevice(int deviceId) { CheckReturnCode(() => tdRemoveDevice(deviceId), nameof(RemoveDevice), deviceId); }

        public DeviceMethod GetMethods(int deviceId) { return (DeviceMethod)CheckReturnCode(() => tdMethods(deviceId, (DeviceMethod)1023), nameof(GetMethods), deviceId); }

        public void TurnOn(int deviceId) { CheckReturnCode(() => tdTurnOn(deviceId), nameof(TurnOn), deviceId); }

        public void TurnOff(int deviceId) { CheckReturnCode(() => tdTurnOff(deviceId), nameof(TurnOff), deviceId); }

        public void Bell(int deviceId) { CheckReturnCode(() => tdBell(deviceId), nameof(Bell), deviceId); }

        public void Dim(int deviceId, int level) { CheckReturnCode(() => tdDim(deviceId, (byte)level), nameof(Dim), deviceId, level); }

        public void Execute(int deviceId) { CheckReturnCode(() => tdExecute(deviceId), nameof(Execute), deviceId); }

        public void Up(int deviceId) { CheckReturnCode(() => tdUp(deviceId), nameof(Up), deviceId); }

        public void Down(int deviceId) { CheckReturnCode(() => tdDown(deviceId), nameof(Down), deviceId); }

        public void Stop(int deviceId) { CheckReturnCode(() => tdStop(deviceId), nameof(Stop), deviceId); }

        public void Close() { lock (LockKey) { tdClose(); } }

        public void Init() { lock (LockKey) { tdInit(); } }

        public DeviceMethod GetLastSentCommand(int deviceId) { return (DeviceMethod)CheckReturnCode(() => tdLastSentCommand(deviceId, (DeviceMethod)1023), nameof(GetLastSentCommand), deviceId); }

        public DeviceType GetDeviceType(int deviceId) { return (DeviceType)CheckReturnCode(() => tdGetDeviceType(deviceId), nameof(GetDeviceType), deviceId); }

        public void SendRawCommand(string command) { CheckReturnCode(() => tdSendRawCommand(command, 0), nameof(SendRawCommand), command); }

        public void Learn(int deviceId) { CheckReturnCode(() => tdLearn(deviceId), nameof(Learn), deviceId); }

        public string GetLastSentValue(int deviceId) { return Lock(() => tdLastSentValue(deviceId)); }

        public void UnregisterCallback(int eventId) { CheckReturnCode(() => tdUnregisterCallback(eventId), nameof(UnregisterCallback), eventId); }

        public int RegisterDeviceEvent(DeviceStateChangeEventDelegate deviceEventFunction) { return CheckReturnCode(() => tdRegisterDeviceEvent(deviceEventFunction, IntPtr.Zero), nameof(RegisterDeviceEvent), "delegate"); }

        public int RegisterRawDeviceEvent(RawDeviceEventDelegate rawListeningFunction) { return CheckReturnCode(() => tdRegisterRawDeviceEvent(rawListeningFunction, IntPtr.Zero), nameof(RegisterRawDeviceEvent), "delegate"); }

        public int RegisterDeviceChangeEvent(DeviceChangeEventDelegate deviceChangeEventFunction) { return CheckReturnCode(() => tdRegisterDeviceChangeEvent(deviceChangeEventFunction, IntPtr.Zero), nameof(RegisterDeviceChangeEvent), "delegate"); }

        public int RegisterSensorEvent(SensorEventDelegate sensorEventFunction) { return CheckReturnCode(() => tdRegisterSensorEvent(sensorEventFunction, IntPtr.Zero), nameof(RegisterSensorEvent), "delegate"); }
    }

    internal class NativeMethods
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

        [DllImport("TelldusCore.dll", BestFitMapping = false, ThrowOnUnmappableChar = true)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = "out", MarshalTypeRef = typeof(TelldusUtf8Marshaler))]
        internal static extern string tdGetDeviceParameter(
            int deviceId,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = "in", MarshalTypeRef = typeof(TelldusUtf8Marshaler))] string name,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = "in", MarshalTypeRef = typeof(TelldusUtf8Marshaler))] string defaultValue);

        [DllImport("TelldusCore.dll", BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern bool tdSetName(
            int deviceId,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = "in", MarshalTypeRef = typeof(TelldusUtf8Marshaler))] string name);

        [DllImport("TelldusCore.dll", BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern bool tdSetProtocol(
            int deviceId,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = "in", MarshalTypeRef = typeof(TelldusUtf8Marshaler))] string protocol);

        [DllImport("TelldusCore.dll", BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern bool tdSetModel(
            int deviceId,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = "in", MarshalTypeRef = typeof(TelldusUtf8Marshaler))] string model);

        [DllImport("TelldusCore.dll", BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern bool tdSetDeviceParameter(
            int deviceId,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = "in", MarshalTypeRef = typeof(TelldusUtf8Marshaler))] string name,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = "in", MarshalTypeRef = typeof(TelldusUtf8Marshaler))] string value);

        [DllImport("TelldusCore.dll")]
        internal static extern int tdAddDevice();

        [DllImport("TelldusCore.dll")]
        internal static extern bool tdRemoveDevice(int deviceId);

        [DllImport("TelldusCore.dll")]
        internal static extern int tdMethods(int deviceId, DeviceMethod methodsSupported);

        [DllImport("TelldusCore.dll")]
        internal static extern int tdTurnOn(int deviceId);

        [DllImport("TelldusCore.dll")]
        internal static extern int tdTurnOff(int deviceId);

        [DllImport("TelldusCore.dll")]
        internal static extern int tdBell(int deviceId);

        [DllImport("TelldusCore.dll")]
        internal static extern int tdDim(int deviceId, byte level);

        [DllImport("TelldusCore.dll")]
        internal static extern int tdExecute(int deviceId);

        [DllImport("TelldusCore.dll")]
        internal static extern int tdUp(int deviceId);

        [DllImport("TelldusCore.dll")]
        internal static extern int tdDown(int deviceId);

        [DllImport("TelldusCore.dll")]
        internal static extern int tdStop(int deviceId);

        [DllImport("TelldusCore.dll")]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = "out", MarshalTypeRef = typeof(TelldusUtf8Marshaler))]
        internal static extern string tdGetErrorString(int errorNo);

        [DllImport("TelldusCore.dll")]
        internal static extern void tdClose();

        [DllImport("TelldusCore.dll")]
        internal static extern void tdInit();

        [DllImport("TelldusCore.dll")]
        internal static extern int tdLastSentCommand(int deviceId, DeviceMethod methods);

        [DllImport("TelldusCore.dll")]
        internal static extern int tdGetDeviceType(int deviceId);

        [DllImport("TelldusCore.dll", BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern int tdSendRawCommand(
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
        internal static extern int tdRegisterDeviceEvent(DeviceStateChangeEventDelegate deviceEventFunction, IntPtr context);

        [DllImport("TelldusCore.dll")]
        internal static extern int tdRegisterRawDeviceEvent(RawDeviceEventDelegate rawListeningFunction, IntPtr context);

        [DllImport("TelldusCore.dll")]
        internal static extern int tdRegisterDeviceChangeEvent(DeviceChangeEventDelegate deviceChangeEventFunction, IntPtr context);

        [DllImport("TelldusCore.dll")]
        internal static extern int tdRegisterSensorEvent(SensorEventDelegate sensorEventFunction, IntPtr context);
    }
}