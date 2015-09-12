using System;

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

        public TellCoreClient() : this(new TelldusCoreProxy()) { }

        public TellCoreClient(ITelldusCoreProxy proxy)
        {
            Proxy = proxy;
            Proxy.Init();
        }

        private ITelldusCoreProxy Proxy { get; }

        /// <summary>
        /// Gets the total number of devices in TelldusCenter.
        /// </summary>
        /// <returns>The number of devices</returns>
        public int GetNumberOfDevices()
        {
            return Proxy.GetNumberOfDevices();
        }

        /// <summary>
        /// Gets the Telldus deviceId of a device at a certain index.
        /// </summary>
        /// <param name="index">The index of the device</param>
        /// <returns>The device id at given index</returns>
        public int GetDeviceId(int index)
        {
            return Proxy.GetDeviceId(index);
        }

        /// <summary>
        /// Gets the name of a device.
        /// </summary>
        /// <param name="deviceId">The deviceId of the device</param>
        /// <returns>The name of the device</returns>
        public string GetName(int deviceId)
        {
            return Proxy.GetName(deviceId);
        }

        /// <summary>
        /// Gets the protocol a certain device uses.
        /// </summary>
        /// <param name="deviceId">The id of the device</param>
        /// <returns>The protocol used by the device</returns>
        public string GetProtocol(int deviceId)
        {
            return Proxy.GetProtocol(deviceId);
        }

        /// <summary>
        /// Gets the model of a certain device.
        /// </summary>
        /// <param name="deviceId">The id of the device</param>
        /// <returns>The model of a device</returns>
        public string GetModel(int deviceId)
        {
            return Proxy.GetModel(deviceId);
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
            return Proxy.GetDeviceParameter(deviceId, parameterName, defaultValue);
        }

        /// <summary>
        /// Sets a device parameter
        /// </summary>
        /// <param name="deviceId">The device id for which to set a parameter</param>
        /// <param name="name">The name of the parameter to set</param>
        /// <param name="value">The value of the parameter to set</param>
        public void SetDeviceParameter(int deviceId, string name, string value)
        {
            Proxy.SetDeviceParameter(deviceId, name, value);
        }

        /// <summary>
        /// Sets the name of a device
        /// </summary>
        /// <param name="deviceId">Id of the device to set name for</param>
        /// <param name="name">The name to set</param>
        public void SetName(int deviceId, string name)
        {
            Proxy.SetName(deviceId, name);
        }

        /// <summary>
        /// Sets a protocol for the device.
        /// </summary>
        /// <param name="deviceId">The device id for which to set the protocol</param>
        /// <param name="protocol">The protocol to set</param>
        public void SetProtocol(int deviceId, string protocol)
        {
            Proxy.SetProtocol(deviceId, protocol);
        }

        /// <summary>
        /// Sets the model of a device
        /// </summary>
        /// <param name="deviceId">The device id for which to set the model</param>
        /// <param name="model">The model name</param>
        public void SetModel(int deviceId, string model)
        {
            Proxy.SetModel(deviceId, model);
        }

        /// <summary>
        /// Adds a new device
        /// </summary>
        /// <returns>The deviceId for the new device</returns>
        public int AddDevice()
        {
            return Proxy.AddDevice();
        }

        /// <summary>
        /// Removes a device
        /// </summary>
        /// <param name="deviceId">The deviceId to remove</param>
        public void RemoveDevice(int deviceId)
        {
            Proxy.RemoveDevice(deviceId);
        }

        /// <summary>
        /// Gets the supported method of a device
        /// </summary>
        /// <param name="deviceId">The deviceId for which to query</param>
        public DeviceMethod GetMethods(int deviceId)
        {
            return Proxy.GetMethods(deviceId);
        }

        /// <summary>
        /// Turns on a device
        /// </summary>
        /// <param name="deviceId">The deviceId to turn on</param>
        public void TurnOn(int deviceId)
        {
            Proxy.TurnOn(deviceId);
        }

        /// <summary>
        /// Turns off a device
        /// </summary>
        /// <param name="deviceId">The deviceId to turn off</param>
        public void TurnOff(int deviceId)
        {
            Proxy.TurnOff(deviceId);
        }

        /// <summary>
        /// Sends a bell command to a device
        /// </summary>
        /// <param name="deviceId">The deviceId to bell</param>
        public void Bell(int deviceId)
        {
            Proxy.Bell(deviceId);
        }

        /// <summary>
        /// Dims a device
        /// </summary>
        /// <param name="deviceId">The deviceId to dim</param>
        /// <param name="level">The level to dim to (0 - 255)</param>
        public void Dim(int deviceId, int level)
        {
            if (level < 0 || level > 255)
                throw new ArgumentOutOfRangeException(nameof(level), "Must be between 0 and 255");

            Proxy.Dim(deviceId, level);
        }

        /// <summary>
        /// Execute a scene action
        /// </summary>
        /// <param name="deviceId">The deviceId for which to execute a scene</param>
        public void Execute(int deviceId)
        {
            Proxy.Execute(deviceId);
        }

        /// <summary>
        /// Sends an "up" command to a device
        /// </summary>
        /// <param name="deviceId">Device to send command to</param>
        public void Up(int deviceId)
        {
            Proxy.Up(deviceId);
        }

        /// <summary>
        /// Sends a "down" command to a device
        /// </summary>
        /// <param name="deviceId">Device to send command to</param>
        public void Down(int deviceId)
        {
            Proxy.Down(deviceId);
        }

        /// <summary>
        /// Sends a "stop" command to a device
        /// </summary>
        /// <param name="deviceId">Device to send command to</param>
        public void Stop(int deviceId)
        {
            Proxy.Stop(deviceId);
        }

        /// <summary>
        /// Gets the last sent command to a device
        /// </summary>
        /// <param name="deviceId">Device to query</param>
        /// <returns></returns>
        public DeviceMethod GetLastSentCommand(int deviceId)
        {
            return Proxy.GetLastSentCommand(deviceId);
        }

        /// <summary>
        /// Gets the device type of a device
        /// </summary>
        /// <param name="deviceId">The device to query</param>
        /// <returns>The type of the device</returns>
        public DeviceType GetDeviceType(int deviceId)
        {
            return Proxy.GetDeviceType(deviceId);
        }

        /// <summary>
        /// Send a raw command to TellStick. 
        /// Please read the TellStick protocol definition on how the command should be constructed.
        /// </summary>
        /// <param name="command">The command for TellStick in its native format</param>
        public void SendRawCommand(string command)
        {
            Proxy.SendRawCommand(command);
        }

        public event DeviceChangedHandler DeviceChanged
        {
            add
            {
                // If this is the first subscriber to the event we'll register with telldus
                if (deviceChanged == null)
                    deviceChangedCallbackId = Proxy.RegisterDeviceChangeEvent(OnDeviceChanged);

                deviceChanged += value;
            }
            remove
            {
                // Need this double check to prevent accidental tdUnregisterCallback
                if (deviceChanged != null)
                {
                    deviceChanged -= value;
                    if (deviceChanged == null && deviceChangedCallbackId.HasValue)
                        Proxy.UnregisterCallback(deviceChangedCallbackId.Value);
                }
            }
        }

        public event DeviceStateChangedHandler DeviceStateChanged
        {
            add
            {
                // If this is the first subscriber to the event we'll register with telldus
                if (deviceStateChanged == null)
                    deviceStateChangedCallbackId = Proxy.RegisterDeviceEvent(OnDeviceStateChanged);

                deviceStateChanged += value;
            }
            remove
            {
                // Need this double check to prevent accidental tdUnregisterCallback
                if (deviceStateChanged != null)
                {
                    deviceStateChanged -= value;

                    if (deviceStateChanged == null && deviceStateChangedCallbackId.HasValue)
                        Proxy.UnregisterCallback(deviceStateChangedCallbackId.Value);
                }
            }
        }

        public event RawDeviceEventHandler RawDeviceEvent
        {
            add
            {
                // If this is the first subscriber to the event we'll register with telldus
                if (rawDeviceEvent == null)
                    rawDeviceEventCallbackId = Proxy.RegisterRawDeviceEvent(OnRawDeviceEvent);

                rawDeviceEvent += value;
            }
            remove
            {
                // Need this double check to prevent accidental tdUnregisterCallback
                if (rawDeviceEvent != null)
                {
                    rawDeviceEvent -= value;

                    if (rawDeviceEvent == null && rawDeviceEventCallbackId.HasValue)
                        Proxy.UnregisterCallback(rawDeviceEventCallbackId.Value);
                }
            }
        }

        void OnDeviceStateChanged(int deviceId, DeviceMethod method, string data, int callbackId, IntPtr context)
        {
            if (deviceStateChanged == null)
                return;

            var args = new DeviceStateChangedEventArgs
            {
                DeviceId = deviceId,
                Method = method,
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
            Proxy.Close();
        }

        public void Dispose()
        {
            UnregisterIfNecessary(ref deviceChangedCallbackId);
            UnregisterIfNecessary(ref deviceStateChangedCallbackId);
            UnregisterIfNecessary(ref rawDeviceEventCallbackId);

            Proxy.Close();
        }

        private void UnregisterIfNecessary(ref int? callbackId)
        {
            if (callbackId.HasValue)
            {
                Proxy.UnregisterCallback(callbackId.Value);
                callbackId = null;
            }
        }
    }
}