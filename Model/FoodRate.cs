using System;
using System.Collections.Generic;

#nullable disable

namespace HealthyLife.Model
{
    public class FoodRate
    {
        public string Id { get; set; }
        public string CoachId { get; set; }
        public string AthletId { get; set; }
        public int? Kcal { get; set; }
        public int? Proteins { get; set; }
        public int? Fats { get; set; }
        public int? Carbohydrates { get; set; }
        public double? AmountOfWater { get; set; }
        public DateTime? Date { get; set; }

        public virtual User Athlet { get; set; }
        public virtual User Coach { get; set; }
    }
}
