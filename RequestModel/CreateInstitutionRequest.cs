namespace HealthyLife.RequestModel
{
    public class CreateInstitutionRequest
    {
        public string Name { get; set; } 
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string SiteLink { get; set; }
    }
}