using System;
using System.Collections.Generic;

#nullable disable

namespace HealthyLife.Model
{
    public class CoachToInstitution
    {
        public string Id { get; set; }
        public string InstitutionId { get; set; }
        public string CoachId { get; set; }
        public string Position { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public virtual User Coach { get; set; }
        public virtual Institution Institution { get; set; }
    }
}
