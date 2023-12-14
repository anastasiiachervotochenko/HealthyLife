using System.Threading.Tasks;
using HealthyLife.Manager.Admin;
using HealthyLife.Model;
using HealthyLife.ResponseModel;
using HealthyLife.Utility;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace HealthyLife.Controllers
{
    [EnableCors()]
    [Route("[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminManager _adminManager;
        private readonly IJsonEncryptionService _encryptionService;

        public AdminController(IAdminManager adminManager, IJsonEncryptionService encryptionService)
        {
            _adminManager = adminManager;
            _encryptionService = encryptionService;
        }

        [HttpGet]
        [Route("BackUp/{path}")]
        public async Task<IActionResult> CreateDatabaseBackUp([FromRoute] string path)
        {
            await _adminManager.CreateBackupAsync(path);
            await _adminManager.InsertBackupToDbAsync(path);

            return new JsonResult("Done");
        }

        [HttpGet]
        [Route("BackUp/last")]
        public async Task<IActionResult> GetLastBackupAsync()
        {
            var result = await _adminManager.GetLastBackupAsync();

            return new JsonResult(result);
        }

        [HttpGet]
        [Route("BackUp/Id/{id}")]
        public async Task<IActionResult> GetBackupByIdAsync([FromRoute] int id)
        {
            var result = await _adminManager.GetBackupByIdAsync(id);

            return new JsonResult(result);
        }

        [HttpPost]
        [Route("Create/Admin")]
        public async Task<IActionResult> CreateAdmin([FromBody] Admin admin)
        {
            var result = await _adminManager.CreateAdmin(admin);

            if (result.Completed)
            {
                var response = new ServiseResponse<string>
                {
                    Data = _encryptionService.Encrypt(result.Data),
                    Completed = result.Completed,
                    Message = result.Message
                };

                return new JsonResult(response);
            }

            return new JsonResult(result.Message);
        }

        [HttpGet]
        [Route("Login")]
        public async Task<IActionResult> Login([FromQuery] string login, [FromQuery] string password)
        {
            var result = await _adminManager.Login(login, password);

            if (result.Completed)
            {
                var response = new ServiseResponse<string>
                {
                    Data = _encryptionService.Encrypt(result.Data),
                    Completed = result.Completed,
                    Message = result.Message
                };

                return new JsonResult(response);
            }

            return new JsonResult(result.Message);
        }

        [HttpGet]
        [Route("Users")]
        public async Task<IActionResult> GetUsers()
        {
            var result = await _adminManager.GetUsers();

            return new JsonResult(result);
        }

        [HttpGet]
        [Route("Institutions")]
        public async Task<IActionResult> GetInstitutions()
        {
            var result = await _adminManager.GetInstitutions();

            return new JsonResult(result);
        }
        
        [HttpDelete]
        [Route("User/{id}")]
        public async Task<IActionResult> RemoveUser([FromRoute] string id)
        {
            var result = await _adminManager.RemoveUser(id);
            
            return new JsonResult(result);
        }

        [HttpDelete]
        [Route("Institution/{id}")]
        public async Task<IActionResult> RemoveInstitution([FromRoute] string id)
        {
            var result = await _adminManager.RemoveInstitution(id);

            return new JsonResult(result);
        }
    }
}