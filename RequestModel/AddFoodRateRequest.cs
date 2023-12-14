namespace HealthyLife.RequestModel
{
    public class AddFoodRateRequest
    {
        public string CoachId { get; set; }
        public string AthletId { get; set; }
        public int? Kcal { get; set; }
        public int? Proteins { get; set; }
        public int? Fats { get; set; }
        public int? Carbohydrates { get; set; }
        public double? AmountOfWater { get; set; }
    }
}