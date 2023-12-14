using System;

namespace HealthyLife.ResponseModel
{
    public class GetUserByCredsResponse
    {
        public string Token { get; set; }
        public bool IsAdmin { get; set; }
        public Guid Id { get; set; }
    }
}