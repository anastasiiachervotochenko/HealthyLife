using System;
using System.Collections.Generic;

#nullable disable

namespace HealthyLife.Model
{
    public class AccountLog
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string Role { get; set; }

        public virtual User User { get; set; }
    }
}
