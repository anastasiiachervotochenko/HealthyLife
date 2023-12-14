using System;
using System.Collections.Generic;

#nullable disable

namespace HealthyLife.Model
{
    public class InformationFromDevice
    {
        public string Id { get; set; }
        public string AthletId { get; set; }
        public int? Pulse { get; set; }
        public int? OxygenLevel { get; set; }
        public double? Temperature { get; set; }
        public double? PositionX { get; set; }
        public double? PositionY { get; set; }
        public double? PositionZ { get; set; }
    }
}
