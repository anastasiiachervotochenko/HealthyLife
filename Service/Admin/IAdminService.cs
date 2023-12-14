using System.Collections.Generic;
using System.Threading.Tasks;
using HealthyLife.Model;
using HealthyLife.ResponseModel;
using HealthyLife.Utility;

namespace HealthyLife.Service.Admin
{
    public interface IAdminService
    {
        Task<ServiseResponse<AuthorizationIdentifier>> CreateAdmin(Model.Admin admin);
        Task<ServiseResponse<AuthorizationIdentifier>> Login(string login, string password);
        Task<ServiseResponse<string>> RemoveUser(string id);
        Task<ServiseResponse<string>> RemoveInstitution(string id);
        Task<ServiseResponse<List<User>>> GetUsers();
        Task<ServiseResponse<List<Institution>>> GetInstitutions();
    }
}