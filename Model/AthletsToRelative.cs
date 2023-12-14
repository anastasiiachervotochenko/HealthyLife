using System;
using System.Collections.Generic;

#nullable disable

namespace HealthyLife.Model
{
    public class AthletsToRelative
    {
        public string Id { get; set; }
        public string RelativeId { get; set; }
        public string AthleteId { get; set; }
        public string Role { get; set; }

        public virtual User Athlete { get; set; }
        public virtual User Relative { get; set; }
    }
}
