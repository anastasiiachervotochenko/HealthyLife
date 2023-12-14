using System;
using System.Collections.Generic;

#nullable disable

namespace HealthyLife.Model
{
    public class Institution
    {
        public Institution()
        {
            CoachToInstitutions = new HashSet<CoachToInstitution>();
            InstitutionLogs = new HashSet<InstitutionLog>();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string SiteLink { get; set; }
        public Boolean Active { get; set; }

        public virtual ICollection<CoachToInstitution> CoachToInstitutions { get; set; }
        public virtual ICollection<InstitutionLog> InstitutionLogs { get; set; }
    }
}
