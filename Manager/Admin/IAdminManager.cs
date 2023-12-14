using System.Collections.Generic;
using System.Threading.Tasks;
using HealthyLife.Model;
using HealthyLife.ResponseModel;
using HealthyLife.Utility;

namespace HealthyLife.Manager.Admin
{
    public interface IAdminManager
    {
        Task CreateBackupAsync(string path);      
        Task InsertBackupToDbAsync(string path);
        Task RestoreDatabaseBySomeBackupAsync(Backup backup);
        Task<Backup> GetLastBackupAsync();
        Task<Backup> GetBackupByIdAsync(int id);
        Task<ServiseResponse<AuthorizationIdentifier>> CreateAdmin(Model.Admin admin);
        Task<ServiseResponse<AuthorizationIdentifier>> Login(string login, string password);
        Task<ServiseResponse<string>> RemoveUser(string id);
        Task<ServiseResponse<string>> RemoveInstitution(string id);
        Task<ServiseResponse<List<User>>> GetUsers();
        Task<ServiseResponse<List<Institution>>> GetInstitutions();
    }
}