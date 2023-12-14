using System;
using System.Collections.Generic;

#nullable disable

namespace HealthyLife.Model
{
    public class InstitutionLog
    {
        public string Id { get; set; }
        public string InstitutionId { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }

        public virtual Institution Institution { get; set; }
    }
}
