using System;

#nullable disable

namespace HealthyLife.Model
{
    public class BodyInformation
    {
        public string Id { get; set; }
        public string AthletId { get; set; }
        public double? Height { get; set; }
        public double? Weight { get; set; }
        public double? ChestGirth { get; set; }
        public double? WaistCircumference { get; set; }
        public double? AbdominalGirth { get; set; }
        public double? ButtocksGirth { get; set; }
        public double? ThighGirth { get; set; }
        public DateTime? Date { get; set; }

        public virtual User Athlet { get; set; }
    }
}
