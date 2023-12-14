namespace HealthyLife.RequestModel
{
    public class CreateInstitutionAdminRequest
    {
        public string InstitutionId { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
    }
}