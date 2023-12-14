using System;
using System.Collections.Generic;

#nullable disable

namespace HealthyLife.Model
{
    public class Group
    {
        public string Id { get; set; }
        public string GroupName { get; set; }
        public string Sport { get; set; }
        public string Type { get; set; }
        public string CoachInInstitutionId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
