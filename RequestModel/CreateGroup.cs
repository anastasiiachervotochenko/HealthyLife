using System;

namespace HealthyLife.RequestModel
{
    public class CreateGroupRequest
    {
        public string GroupName { get; set; }
        public string Sport { get; set; }
        public string Type { get; set; }
        public string CoachInstitutionId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}