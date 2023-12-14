using System;

namespace HealthyLife.ResponseModel
{
    public class AccountData
    {
        public string Id { get; set; }
        public string Fio { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime? BirthdayDate { get; set; }
        public string Sex { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
    }
}