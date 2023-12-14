namespace HealthyLife.Model
{
    public class Admin
    {
        public string Id { get; set; }
        public string Fio { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public bool Active { get; set; }
    }
}