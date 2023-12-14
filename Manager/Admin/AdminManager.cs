using System.Collections.Generic;
using System.Threading.Tasks;
using HealthyLife.Model;
using HealthyLife.ResponseModel;
using HealthyLife.Service;
using HealthyLife.Service.Admin;
using HealthyLife.Service.Backup;
using HealthyLife.Utility;

namespace HealthyLife.Manager.Admin
{
    public class AdminManager : IAdminManager
    {
        private IBackupService _backupService { get; set; }
        private IAdminService _adminService { get; set; }

        public AdminManager(IBackupService backupService, IAdminService adminService)
        {
            _backupService = backupService;
            _adminService = adminService;
        }

        public async Task CreateBackupAsync(string path)
        {
            await _backupService.CreateBackupAsync(path);
        }

        public async Task InsertBackupToDbAsync(string path)
        {
            await _backupService.InsertBackupToDbAsync(path);
        }

        public async Task RestoreDatabaseBySomeBackupAsync(Backup backup)
        {
            await _backupService.RestoreDatabaseBySomeBackupAsync(backup);
        }

        public async Task<Backup> GetLastBackupAsync()
        {
            return await _backupService.GetLastBackupAsync();
        }

        public async Task<Backup> GetBackupByIdAsync(int id)
        {
            return await _backupService.GetBackupByIdAsync(id);
        }

        public async Task<ServiseResponse<AuthorizationIdentifier>> CreateAdmin(Model.Admin admin)
        {
            return await _adminService.CreateAdmin(admin);
        }

        public async Task<ServiseResponse<AuthorizationIdentifier>> Login(string login, string password)
        {
            return await _adminService.Login(login, password);
        }

        public async Task<ServiseResponse<string>> RemoveUser(string id)
        {
            return await _adminService.RemoveUser(id);
        }

        public async Task<ServiseResponse<string>> RemoveInstitution(string id)
        {
            return await _adminService.RemoveInstitution(id);
        }


        public async Task<ServiseResponse<List<User>>> GetUsers()
        {
            return await _adminService.GetUsers();

        }

        public async Task<ServiseResponse<List<Institution>>> GetInstitutions()
        {
            return await _adminService.GetInstitutions();
        }
    }
}