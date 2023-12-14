using System;
using System.Collections.Generic;

#nullable disable

namespace HealthyLife.Model
{
    public class Payment
    {
        public string Id { get; set; }
        public string ReceiptId { get; set; }
        public string PayerId { get; set; }
        public string AthletId { get; set; }
        public double? Sum { get; set; }
        public DateTime? Date { get; set; }
    }
}
