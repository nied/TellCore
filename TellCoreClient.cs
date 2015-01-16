using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TellCore
{
    public partial class TellCoreClient : IDisposable
    {
        public EventProxy Events { get; private set; }

        public TellCoreClient()
        {
            UnmanagedWrapper.tdInit();
            Events = new EventProxy();
        }

        /// <summary>
        /// Gets the total number of devices in TelldusCenter.
        /// </summary>
        /// <returns>The number of devices</returns>
        public int GetNumberOfDevices()
        {
            return UnmanagedWrapper.tdGetNumberOfDevices();
        }

        /// <summary>
        /// Gets the Telldus deviceId of a device at a certain index.
        /// </summary>
        /// <param name="index">The index of the device</param>
        /// <returns>The device id at given index</returns>
        public int GetDeviceId(int index)
        {
            return UnmanagedWrapper.tdGetDeviceId(index);
        }

        /// <summary>
        /// Gets the name of a device.
        /// </summary>
        /// <param name="deviceId">The deviceId of the device</param>
        /// <returns>The name of the device</returns>
        public unsafe string GetName(int deviceId)
        {
            return WithUnmanagedString(UnmanagedWrapper.tdGetName(deviceId));
        }

        /// <summary>
        /// Gets the protocol a certain device uses.
        /// </summary>
        /// <param name="deviceId">The id of the device</param>
        /// <returns>The protocol used by the device</returns>
        public unsafe string GetProtocol(int deviceId)
        {
            return WithUnmanagedString(UnmanagedWrapper.tdGetProtocol(deviceId));
        }

        /// <summary>
        /// Gets the model of a certain device.
        /// </summary>
        /// <param name="deviceId">The id of the device</param>
        /// <returns>The model of a device</returns>
        public unsafe string GetModel(int deviceId)
        {
            return WithUnmanagedString(UnmanagedWrapper.tdGetModel(deviceId));
        }

        /// <summary>
        /// Gets a protocol specific parameter for the device.
        /// </summary>
        /// <param name="deviceId">The id of the device</param>
        /// <param name="parameterName">The name of the parameter to query</param>
        /// <param name="defaultValue">A default value to return if the current parameter hasn't previously been set</param>
        /// <returns>The parameter value</returns>
        public unsafe string GetDeviceParameter(int deviceId, string parameterName, string defaultValue)
        {
            char* namePointer = StringUtils.StringToUtf8Pointer(parameterName);
            char* defaultValuePointer = StringUtils.StringToUtf8Pointer(defaultValue);
            
            var result = WithUnmanagedString(UnmanagedWrapper.tdGetDeviceParameter(deviceId, namePointer, defaultValuePointer));

            Marshal.FreeHGlobal((IntPtr)namePointer);
            Marshal.FreeHGlobal((IntPtr)defaultValuePointer);

            return result;
        }

        /// <summary>
        /// Sets a device parameter
        /// </summary>
        /// <param name="deviceId">The device id for which to set a parameter</param>
        /// <param name="name">The name of the parameter to set</param>
        /// <param name="value">The value of the parameter to set</param>
        /// <returns>True if successful, false if not</returns>
        public unsafe bool SetDeviceParameter(int deviceId, string name, string value)
        {
            char* namePointer = StringUtils.StringToUtf8Pointer(name);
            char* valuePointer = StringUtils.StringToUtf8Pointer(value);

            var result = UnmanagedWrapper.tdSetDeviceParameter(deviceId, namePointer, valuePointer);

            Marshal.FreeHGlobal((IntPtr)namePointer);
            Marshal.FreeHGlobal((IntPtr)valuePointer);

            return result;
        }

        /// <summary>
        /// Sets the name of a device
        /// </summary>
        /// <param name="deviceId">Id of the device to set name for</param>
        /// <param name="name">The name to set</param>
        /// <returns>True if successful, false if not</returns>
        public unsafe bool SetName(int deviceId, string name)
        {
            char* namePointer = StringUtils.StringToUtf8Pointer(name);
            var result = UnmanagedWrapper.tdSetName(deviceId, namePointer);

            Marshal.FreeHGlobal((IntPtr)namePointer);

            return result;
        }

        /// <summary>
        /// Sets a protocol for the device.
        /// </summary>
        /// <param name="deviceId">The device id for which to set the protocol</param>
        /// <param name="protocol">The protocol to set</param>
        /// <returns>True if successful, false if not</returns>
        public unsafe bool SetProtocol(int deviceId, string protocol)
        {
            char* protocolPointer = StringUtils.StringToUtf8Pointer(protocol);
            var result = UnmanagedWrapper.tdSetProtocol(deviceId, protocolPointer);

            Marshal.FreeHGlobal((IntPtr)protocolPointer);

            return result;
        }

        /// <summary>
        /// Sets the model of a device
        /// </summary>
        /// <param name="deviceId">The device id for which to set the model</param>
        /// <param name="model">The model name</param>
        /// <returns>True if successful, false if not</returns>
        public unsafe bool SetModel(int deviceId, string model)
        {
            char* modelPointer = StringUtils.StringToUtf8Pointer(model);
            var result = UnmanagedWrapper.tdSetModel(deviceId, modelPointer);

            Marshal.FreeHGlobal((IntPtr)modelPointer);

            return result;
        }

        /// <summary>
        /// Adds a new device
        /// </summary>
        /// <returns>The deviceId for the new device</returns>
        public int AddDevice()
        {
            return UnmanagedWrapper.tdAddDevice();
        }

        /// <summary>
        /// Removes a device
        /// </summary>
        /// <param name="deviceId">The deviceId to remove</param>
        /// <returns>True if successful, false if not</returns>
        public bool RemoveDevice(int deviceId)
        {
            return UnmanagedWrapper.tdRemoveDevice(deviceId);
        }

        /// <summary>
        /// Gets the supported method of a device
        /// </summary>
        /// <param name="deviceId">The deviceId for which to query</param>
        /// <param name="methodsSupported">The methods your application supports</param>
        /// <returns>The applicable methods for the device</returns>
        public DeviceMethod GetMethods(int deviceId, DeviceMethod methodsSupported)
        {
            var result = UnmanagedWrapper.tdMethods(deviceId, (int)methodsSupported);

            return (DeviceMethod)Enum.Parse(typeof(DeviceMethod), result.ToString());
        }

        /// <summary>
        /// Turns on a device
        /// </summary>
        /// <param name="deviceId">The deviceId to turn on</param>
        /// <returns>The result of the operation</returns>
        public TellstickResult TurnOn(int deviceId)
        {
            return ToTellstickResult(UnmanagedWrapper.tdTurnOn(deviceId));
        }

        /// <summary>
        /// Turns off a device
        /// </summary>
        /// <param name="deviceId">The deviceId to turn off</param>
        /// <returns>The result of the operation</returns>
        public TellstickResult TurnOff(int deviceId)
        {
            return ToTellstickResult(UnmanagedWrapper.tdTurnOff(deviceId));
        }

        /// <summary>
        /// Sends a bell command to a device
        /// </summary>
        /// <param name="deviceId">The deviceId to bell</param>
        /// <returns>The result of the operation</returns>
        public TellstickResult Bell(int deviceId)
        {
            return ToTellstickResult(UnmanagedWrapper.tdBell(deviceId));
        }

        /// <summary>
        /// Dims a device
        /// </summary>
        /// <param name="deviceId">The deviceId to dim</param>
        /// <param name="level">The level to dim to (0 - 255)</param>
        /// <returns>The result of the operation</returns>
        public TellstickResult Dim(int deviceId, int level)
        {
            if (level < 0 || level > 255)
                throw new ArgumentOutOfRangeException("level", "Must be between 0 and 255");

            return ToTellstickResult(UnmanagedWrapper.tdDim(deviceId, (char)level));
        }

        /// <summary>
        /// Execute a scene action
        /// </summary>
        /// <param name="deviceId">The deviceId for which to execute a scene</param>
        /// <returns>The result of the operation</returns>
        public TellstickResult Execute(int deviceId)
        {
            return ToTellstickResult(UnmanagedWrapper.tdExecute(deviceId));
        }
        
        /// <summary>
        /// Sends an "up" command to a device
        /// </summary>
        /// <param name="deviceId">Device to send command to</param>
        /// <returns></returns>
        public TellstickResult Up(int deviceId)
        {
            return ToTellstickResult(UnmanagedWrapper.tdUp(deviceId));
        }

        /// <summary>
        /// Sends a "down" command to a device
        /// </summary>
        /// <param name="deviceId">Device to send command to</param>
        /// <returns></returns>
        public TellstickResult Down(int deviceId)
        {
            return ToTellstickResult(UnmanagedWrapper.tdDown(deviceId));
        }

        /// <summary>
        /// Sends a "stop" command to a device
        /// </summary>
        /// <param name="deviceId">Device to send command to</param>
        /// <returns></returns>
        public TellstickResult Stop(int deviceId)
        {
            return ToTellstickResult(UnmanagedWrapper.tdStop(deviceId));
        }

        /// <summary>
        /// Gets the last sent command to a device
        /// </summary>
        /// <param name="deviceId">Device to query</param>
        /// <param name="methodsSupported">The methods supported by the client</param>
        /// <returns></returns>
        public DeviceMethod GetLastSentCommand(int deviceId, DeviceMethod methodsSupported)
        {
            return ToDeviceMethod(UnmanagedWrapper.tdLastSentCommand(deviceId, (int)methodsSupported));
        }

        /// <summary>
        /// Gets the device type of a device
        /// </summary>
        /// <param name="deviceId">The device to query</param>
        /// <returns>The type of the device</returns>
        public DeviceType GetDeviceType(int deviceId)
        {
            return ToDeviceType(UnmanagedWrapper.tdGetDeviceType(deviceId));
        }

        /// <summary>
        /// Send a raw command to TellStick. 
        /// Please read the TellStick protocol definition on how the command should be constructed.
        /// </summary>
        /// <param name="command">The command for TellStick in its native format</param>
        /// <param name="reserved">Reserved for future use</param>
        /// <returns></returns>
        public unsafe TellstickResult SendRawCommand(string command, int reserved)
        {
            char* pointer = StringUtils.StringToUtf8Pointer(command);
            var result = ToTellstickResult(UnmanagedWrapper.tdSendRawCommand(pointer, reserved));

            Marshal.FreeHGlobal((IntPtr)pointer);

            return result;
        }

        /// <summary>
        /// Gets a human readable string from an error code returned from a function in telldus-core.
        /// </summary>
        /// <param name="errorCode">The error code to translate</param>
        /// <returns>A human-readable error string</returns>
        public unsafe string GetErrorString(TellstickResult errorCode)
        {
            return WithUnmanagedString(UnmanagedWrapper.tdGetErrorString((int)errorCode));
        }

        /// <summary>
        /// Closes the session with TellCore.dll. 
        /// In general you do not need to call this method, as we do it internally when disposing.
        /// </summary>
        public void Close()
        {
            UnmanagedWrapper.tdClose();
        }

        private void Init()
        {
            UnmanagedWrapper.tdInit();
        }

        public void Dispose()
        {
            if (Events != null)
                Events.Dispose();

            UnmanagedWrapper.tdClose();
        }
    }
}