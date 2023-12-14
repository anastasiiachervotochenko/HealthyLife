using System;
using HealthyLife.Model.Enum;

namespace HealthyLife.ResponseModel
{
    public class GroupResponse
    {
        public string Id { get; set; }
        public string GroupName { get; set; }
        public Sport Sport { get; set; }
        public GroupType Type { get; set; }
        public string CoachInInstitutionId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        
        public string CoachName { get; set; }
    }
}