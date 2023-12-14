using System;
using System.Collections.Generic;

#nullable disable

namespace HealthyLife.Model
{
    public class Receipt
    {
        public string Id { get; set; }
        public string GroupId { get; set; }
        public double? Sum { get; set; }
        public DateTime? Date { get; set; }
    }
}
