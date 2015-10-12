using System;
using TellCore.Utils;

namespace TellCore
{
    public delegate void DeviceChangedHandler(object sender, DeviceChangedEventArgs e);
    public delegate void DeviceStateChangedHandler(object sender, DeviceStateChangedEventArgs e);
    public delegate void RawDeviceEventHandler(object sender, RawDeviceEventArgs e);

    public class TellCoreClient : IDisposable
    {
        int? deviceChangedCallbackId;
        int? deviceStateChangedCallbackId;
        int? rawDeviceEventCallbackId;

        event DeviceChangedHandler deviceChanged;
        event DeviceStateChangedHandler deviceStateChanged;
        event RawDeviceEventHandler rawDeviceEvent;

        TelldusUtf8Marshaler InMarshaler { get; set; }
        TelldusUtf8Marshaler OutMarshaler { get; set; }

        public TellCoreClient()
        {
            NativeMethods.tdInit();

            InMarshaler = new TelldusUtf8Marshaler(MarshalDirection.In);
            OutMarshaler = new TelldusUtf8Marshaler(MarshalDirection.Out);
        }

        /// <summary>
        /// Gets the total number of devices in TelldusCenter.
        /// </summary>
        /// <returns>The number of devices</returns>
        public int GetNumberOfDevices()
        {
            return NativeMethods.tdGetNumberOfDevices();
        }

        /// <summary>
        /// Gets the Telldus deviceId of a device at a certain index.
        /// </summary>
        /// <param name="index">The index of the device</param>
        /// <returns>The device id at given index</returns>
        public int GetDeviceId(int index)
        {
            return NativeMethods.tdGetDeviceId(index);
        }

        /// <summary>
        /// Gets the name of a device.
        /// </summary>
        /// <param name="deviceId">The deviceId of the device</param>
        /// <returns>The name of the device</returns>
        public string GetName(int deviceId)
        {
            return NativeMethods.tdGetName(deviceId);
        }

        /// <summary>
        /// Gets the protocol a certain device uses.
        /// </summary>
        /// <param name="deviceId">The id of the device</param>
        /// <returns>The protocol used by the device</returns>
        public string GetProtocol(int deviceId)
        {
            return NativeMethods.tdGetProtocol(deviceId);
        }

        /// <summary>
        /// Gets the model of a certain device.
        /// </summary>
        /// <param name="deviceId">The id of the device</param>
        /// <returns>The model of a device</returns>
        public string GetModel(int deviceId)
        {
            return NativeMethods.tdGetModel(deviceId);
        }

        /// <summary>
        /// Gets a protocol specific parameter for the device.
        /// </summary>
        /// <param name="deviceId">The id of the device</param>
        /// <param name="parameterName">The name of the parameter to query</param>
        /// <param name="defaultValue">A default value to return if the current parameter hasn't previously been set</param>
        /// <returns>The parameter value</returns>
        public string GetDeviceParameter(int deviceId, string parameterName, string defaultValue)
        {
            return NativeMethods.tdGetDeviceParameter(deviceId, parameterName, defaultValue);
        }

        /// <summary>
        /// Sets a device parameter
        /// </summary>
        /// <param name="deviceId">The device id for which to set a parameter</param>
        /// <param name="name">The name of the parameter to set</param>
        /// <param name="value">The value of the parameter to set</param>
        /// <returns>True if successful, false if not</returns>
        public bool SetDeviceParameter(int deviceId, string name, string value)
        {
            return NativeMethods.tdSetDeviceParameter(deviceId, name, value);
        }

        /// <summary>
        /// Sets the name of a device
        /// </summary>
        /// <param name="deviceId">Id of the device to set name for</param>
        /// <param name="name">The name to set</param>
        /// <returns>True if successful, false if not</returns>
        public bool SetName(int deviceId, string name)
        {
            return NativeMethods.tdSetName(deviceId, name);
        }

        /// <summary>
        /// Sets a protocol for the device.
        /// </summary>
        /// <param name="deviceId">The device id for which to set the protocol</param>
        /// <param name="protocol">The protocol to set</param>
        /// <returns>True if successful, false if not</returns>
        public bool SetProtocol(int deviceId, string protocol)
        {
            return NativeMethods.tdSetProtocol(deviceId, protocol);
        }

        /// <summary>
        /// Sets the model of a device
        /// </summary>
        /// <param name="deviceId">The device id for which to set the model</param>
        /// <param name="model">The model name</param>
        /// <returns>True if successful, false if not</returns>
        public bool SetModel(int deviceId, string model)
        {
            return NativeMethods.tdSetModel(deviceId, model);
        }

        /// <summary>
        /// Adds a new device
        /// </summary>
        /// <returns>The deviceId for the new device</returns>
        public int AddDevice()
        {
            return NativeMethods.tdAddDevice();
        }

        /// <summary>
        /// Removes a device
        /// </summary>
        /// <param name="deviceId">The deviceId to remove</param>
        /// <returns>True if successful, false if not</returns>
        public bool RemoveDevice(int deviceId)
        {
            return NativeMethods.tdRemoveDevice(deviceId);
        }

        /// <summary>
        /// Gets the supported method of a device
        /// </summary>
        /// <param name="deviceId">The deviceId for which to query</param>
        /// <param name="methodsSupported">The methods your application supports</param>
        /// <returns>The applicable methods for the device</returns>
        public DeviceMethod GetMethods(int deviceId, DeviceMethod methodsSupported)
        {
            return NativeMethods.tdMethods(deviceId, methodsSupported);
        }

        /// <summary>
        /// Turns on a device
        /// </summary>
        /// <param name="deviceId">The deviceId to turn on</param>
        /// <returns>The result of the operation</returns>
        public TellstickResult TurnOn(int deviceId)
        {
            return NativeMethods.tdTurnOn(deviceId);
        }

        /// <summary>
        /// Turns off a device
        /// </summary>
        /// <param name="deviceId">The deviceId to turn off</param>
        /// <returns>The result of the operation</returns>
        public TellstickResult TurnOff(int deviceId)
        {
            return NativeMethods.tdTurnOff(deviceId);
        }

        /// <summary>
        /// Sends a bell command to a device
        /// </summary>
        /// <param name="deviceId">The deviceId to bell</param>
        /// <returns>The result of the operation</returns>
        public TellstickResult Bell(int deviceId)
        {
            return NativeMethods.tdBell(deviceId);
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

            return NativeMethods.tdDim(deviceId, (char)level);
        }

        /// <summary>
        /// Execute a scene action
        /// </summary>
        /// <param name="deviceId">The deviceId for which to execute a scene</param>
        /// <returns>The result of the operation</returns>
        public TellstickResult Execute(int deviceId)
        {
            return NativeMethods.tdExecute(deviceId);
        }

        /// <summary>
        /// Sends an "up" command to a device
        /// </summary>
        /// <param name="deviceId">Device to send command to</param>
        /// <returns></returns>
        public TellstickResult Up(int deviceId)
        {
            return NativeMethods.tdUp(deviceId);
        }

        /// <summary>
        /// Sends a "down" command to a device
        /// </summary>
        /// <param name="deviceId">Device to send command to</param>
        /// <returns></returns>
        public TellstickResult Down(int deviceId)
        {
            return NativeMethods.tdDown(deviceId);
        }

        /// <summary>
        /// Use this function to iterate over all sensors.
        /// </summary>
        /// <returns>Returns a sensor, if present. Call until Result != Success to enumerate sensors.</returns>
        public SensorResult GetSensor()
        {
            IntPtr protocol = InMarshaler.MarshalManagedToNative("");
            IntPtr model = InMarshaler.MarshalManagedToNative("");
            
            int sensorId = 0;
            int type = 0;

            // Assume no protocol name is longer than 20 characters, and no models are longer than 30 characters
            var response = NativeMethods.tdSensor(protocol, 20, model, 30, ref sensorId, ref type);

            var result = new SensorResult
            {
                Model = (string)OutMarshaler.MarshalNativeToManaged(model),
                Protocol = (string)OutMarshaler.MarshalNativeToManaged(protocol),
                Result = response,
                SensorId = sensorId,
                Type = (SensorValueType)type
            };

            InMarshaler.CleanUpNativeData(protocol);
            InMarshaler.CleanUpNativeData(model);

            return result;
        }

        /// <summary>
        /// Get one of the supported sensor values from a sensor. The triplet protocol, model and id together identifies a sensor.
        /// </summary>
        /// <param name="protocol">The protocol returned by GetSensor</param>
        /// <param name="model">The model returned by GetSensor</param>
        /// <param name="id">The id returned by GetSensor</param>
        /// <param name="type">One of the SensorValueTypes returned by GetSensor</param>
        /// <returns>A reading with a timestamp for when the value was read.</returns>
        public SensorReadingResult GetSensorValue(string protocol, string model, int id, SensorValueType type) 
        {
            IntPtr protocolPointer = InMarshaler.MarshalManagedToNative(protocol);
            IntPtr modelPointer = InMarshaler.MarshalManagedToNative(model);
            IntPtr valuePointer = InMarshaler.MarshalManagedToNative("");

            int timestamp = 0;

            var response = NativeMethods.tdSensorValue(protocolPointer, modelPointer, id, (int)type, valuePointer, 20, ref timestamp);

            var result = new SensorReadingResult
            {
                Value = (string)OutMarshaler.MarshalNativeToManaged(valuePointer),
                TimeStamp = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(timestamp)
            };

            InMarshaler.CleanUpNativeData(protocolPointer);
            InMarshaler.CleanUpNativeData(modelPointer);
            InMarshaler.CleanUpNativeData(valuePointer);

            return result;
        }

        /// <summary>
        /// Sends a "stop" command to a device
        /// </summary>
        /// <param name="deviceId">Device to send command to</param>
        /// <returns></returns>
        public TellstickResult Stop(int deviceId)
        {
            return NativeMethods.tdStop(deviceId);
        }

        /// <summary>
        /// Gets the last sent command to a device
        /// </summary>
        /// <param name="deviceId">Device to query</param>
        /// <param name="methodsSupported">The methods supported by the client</param>
        /// <returns></returns>
        public DeviceMethod GetLastSentCommand(int deviceId, DeviceMethod methodsSupported)
        {
            return NativeMethods.tdLastSentCommand(deviceId, methodsSupported);
        }

        /// <summary>
        /// Gets the device type of a device
        /// </summary>
        /// <param name="deviceId">The device to query</param>
        /// <returns>The type of the device</returns>
        public DeviceType GetDeviceType(int deviceId)
        {
            return NativeMethods.tdGetDeviceType(deviceId);
        }

        /// <summary>
        /// Send a raw command to TellStick. 
        /// Please read the TellStick protocol definition on how the command should be constructed.
        /// </summary>
        /// <param name="command">The command for TellStick in its native format</param>
        /// <param name="reserved">Reserved for future use</param>
        /// <returns></returns>
        public TellstickResult SendRawCommand(string command, int reserved)
        {
            return NativeMethods.tdSendRawCommand(command, reserved);
        }

        /// <summary>
        /// Gets a human readable string from an error code returned from a function in telldus-core.
        /// </summary>
        /// <param name="errorCode">The error code to translate</param>
        /// <returns>A human-readable error string</returns>
        public string GetErrorString(TellstickResult errorCode)
        {
            return NativeMethods.tdGetErrorString(errorCode);
        }

        public event DeviceChangedHandler DeviceChanged
        {
            add
            {
                // If this is the first subscriber to the event we'll register with telldus
                if (deviceChanged == null)
                    deviceChangedCallbackId = NativeMethods.tdRegisterDeviceChangeEvent(OnDeviceChanged, IntPtr.Zero);

                deviceChanged += value;
            }
            remove
            {
                // Need this double check to prevent accidental tdUnregisterCallback
                if (deviceChanged != null)
                {
                    deviceChanged -= value;
                    if (deviceChanged == null && deviceChangedCallbackId.HasValue)
                        NativeMethods.tdUnregisterCallback(deviceChangedCallbackId.Value);
                }
            }
        }

        public event DeviceStateChangedHandler DeviceStateChanged
        {
            add
            {
                // If this is the first subscriber to the event we'll register with telldus
                if (deviceStateChanged == null)
                    deviceStateChangedCallbackId = NativeMethods.tdRegisterDeviceEvent(OnDeviceStateChanged, IntPtr.Zero);

                deviceStateChanged += value;
            }
            remove
            {
                // Need this double check to prevent accidental tdUnregisterCallback
                if (deviceStateChanged != null)
                {
                    deviceStateChanged -= value;

                    if (deviceStateChanged == null && deviceStateChangedCallbackId.HasValue)
                        NativeMethods.tdUnregisterCallback(deviceStateChangedCallbackId.Value);
                }
            }
        }

        public event RawDeviceEventHandler RawDeviceEvent
        {
            add
            {
                // If this is the first subscriber to the event we'll register with telldus
                if (rawDeviceEvent == null)
                    rawDeviceEventCallbackId = NativeMethods.tdRegisterRawDeviceEvent(OnRawDeviceEvent, IntPtr.Zero);

                rawDeviceEvent += value;
            }
            remove
            {
                // Need this double check to prevent accidental tdUnregisterCallback
                if (rawDeviceEvent != null)
                {
                    rawDeviceEvent -= value;

                    if (rawDeviceEvent == null && rawDeviceEventCallbackId.HasValue)
                        NativeMethods.tdUnregisterCallback(rawDeviceEventCallbackId.Value);
                }
            }
        }

        void OnDeviceStateChanged(int deviceId, int method, string data, int callbackId, IntPtr context)
        {
            if (deviceStateChanged == null)
                return;

            var args = new DeviceStateChangedEventArgs
            {
                DeviceId = deviceId,
                Method = (DeviceMethod)method,
                Data = data
            };

            deviceStateChanged(this, args);
        }

        void OnDeviceChanged(int deviceId, int changeEvent, int changeType, int callbackId, IntPtr context)
        {
            if (deviceChanged == null)
                return;

            var args = new DeviceChangedEventArgs
            {
                DeviceId = deviceId,
                DeviceChange = (DeviceChange)changeEvent,
                DeviceChangeType = (DeviceChangeType)changeType
            };

            deviceChanged(this, args);
        }

        void OnRawDeviceEvent(string data, int controllerId, int callbackId, IntPtr context)
        {
            if (rawDeviceEvent == null)
                return;

            var args = new RawDeviceEventArgs()
            {
                ControllerId = controllerId,
                Data = data
            };

            rawDeviceEvent(this, args);
        }

        /// <summary>
        /// Closes the session with TellCore.dll. 
        /// In general you do not need to call this method, as we do it internally when disposing.
        /// </summary>
        public void Close()
        {
            NativeMethods.tdClose();
        }

        public void Dispose()
        {
            UnregisterIfNecessary(ref deviceChangedCallbackId);
            UnregisterIfNecessary(ref deviceStateChangedCallbackId);
            UnregisterIfNecessary(ref rawDeviceEventCallbackId);

            NativeMethods.tdClose();
        }

        static void UnregisterIfNecessary(ref int? callbackId)
        {
            if (callbackId.HasValue)
            {
                NativeMethods.tdUnregisterCallback(callbackId.Value);
                callbackId = null;
            }
        }
    }
}