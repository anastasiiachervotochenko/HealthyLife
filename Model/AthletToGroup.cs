using System;
using System.Collections.Generic;

#nullable disable

namespace HealthyLife.Model
{
    public class AthletToGroup
    {
        public string Id { get; set; }
        public string GroupId { get; set; }
        public string AthletId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
