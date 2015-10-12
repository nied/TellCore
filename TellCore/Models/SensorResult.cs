using System;

namespace TellCore
{
    public class SensorResult
    {
        public TellstickResult Result { get; set; }
        public SensorValueType Type { get; set; }
        public string Protocol { get; set; }
        public string Model { get; set; }
        public int SensorId { get; set; }
    }
}
