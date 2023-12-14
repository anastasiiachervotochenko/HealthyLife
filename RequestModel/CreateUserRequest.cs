using System;

namespace HealthyLife.RequestModel
{
    public class CreateUserRequest
    {
        public string Fio { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime? BirthdayDate { get; set; }
        public string Sex { get; set; }
        public int Role { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
    }
}