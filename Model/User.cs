using System;
using System.Collections.Generic;

#nullable disable

namespace HealthyLife.Model
{
    public class User
    {
        public User()
        {
            AccountLogs = new HashSet<AccountLog>();
            AthletsToRelativeAthletes = new HashSet<AthletsToRelative>();
            AthletsToRelativeRelatives = new HashSet<AthletsToRelative>();
            BodyInformations = new HashSet<BodyInformation>();
            CoachToInstitutions = new HashSet<CoachToInstitution>();
            FoodRateAthlets = new HashSet<FoodRate>();
            FoodRateCoaches = new HashSet<FoodRate>();
            NutrionInformations = new HashSet<NutrionInformation>();
        }
        
        public string Id { get; set; }
        public string Fio { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime? BirthdayDate { get; set; }
        public string Sex { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public bool Active { get; set; }

        public virtual ICollection<AccountLog> AccountLogs { get; set; }
        public virtual ICollection<AthletsToRelative> AthletsToRelativeAthletes { get; set; }
        public virtual ICollection<AthletsToRelative> AthletsToRelativeRelatives { get; set; }
        public virtual ICollection<BodyInformation> BodyInformations { get; set; }
        public virtual ICollection<CoachToInstitution> CoachToInstitutions { get; set; }
        public virtual ICollection<FoodRate> FoodRateAthlets { get; set; }
        public virtual ICollection<FoodRate> FoodRateCoaches { get; set; }
        public virtual ICollection<NutrionInformation> NutrionInformations { get; set; }
    }
}
