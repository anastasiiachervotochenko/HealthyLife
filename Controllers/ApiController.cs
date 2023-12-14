using System;
using System.Linq;
using System.Threading.Tasks;
using HealthyLife.Manager.App;
using HealthyLife.Model;
using HealthyLife.RequestModel;
using HealthyLife.ResponseModel;
using HealthyLife.Utility;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace HealthyLife.Controllers
{
    [EnableCors()]
    [Route("[controller]")]
    public class ApiController : ControllerBase
    {
        private readonly IAppManager _appManager;
        private readonly IJsonEncryptionService _encryptionService;
        private const string _userIsNotAdmin = "User is not institution admin";
        private const string _userIsNotAuthorized = "User is not authorized";

        public ApiController(IAppManager appManager, IJsonEncryptionService encryptionService)
        {
            _appManager = appManager;
            _encryptionService = encryptionService;
        }

        #region Create

        [HttpPost]
        [Route("Create/User")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest user)
        {
            var result = await _appManager.CreateUser(user);
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

            return new JsonResult(result);
        }

        [HttpPost]
        [Route("Create/Institution")]
        public async Task<IActionResult> CreateInstitution([FromBody] CreateInstitutionRequest institution)
        {
            var result = await _appManager.CreateInstitution(institution);

            return new JsonResult(result);
        }

        [HttpPost]
        [Route("Create/InstitutionAdmin")]
        public async Task<IActionResult> CreateInstitutionAdmin([FromBody] CreateInstitutionAdminRequest user)
        {
            var result = await _appManager.CreateInstitutionAdmin(user);

            return new JsonResult(result);
        }

        [HttpPost]
        [Route("Create/Group")]
        public async Task<IActionResult> CreateGroup([FromBody] CreateGroupRequest user)
        {
            var accountInfo = GetTokenInformation();

            if (accountInfo != null && accountInfo.IsAdmin)
            {
                var result = await _appManager.CreateGroup(user);

                return new JsonResult(result);
            }

            return new JsonResult(_userIsNotAdmin);
        }

        #endregion

        #region Add

        [HttpGet]
        [Route("Add/UserRole")]
        public async Task<IActionResult> AddUserRole([FromQuery] string role)
        {
            var accountInfo = GetTokenInformation();
            var result = await _appManager.AddUserRole(role, accountInfo);

            return new JsonResult(result);
        }

        [HttpPost]
        [Route("Add/BodyInformation")]
        public async Task<IActionResult> AddBodyInformation([FromBody] AddBodyInformationRequest request)
        {
            var accountInfo = GetTokenInformation();
            var result = await _appManager.AddBodyInformation(request, accountInfo);

            return new JsonResult(result);
        }

        [HttpPost]
        [Route("Add/NutrionInformation")]
        public async Task<IActionResult> AddNutrionInformation([FromBody] AddNutrionInformationRequest request)
        {
            var accountInfo = GetTokenInformation();
            var result = await _appManager.AddNutrionInformation(request, accountInfo);

            return new JsonResult(result);
        }

        [HttpPost]
        [Route("Add/FoodRate")]
        public async Task<IActionResult> AddFoodRate([FromBody] AddFoodRateRequest request)
        {
            var accountInfo = GetTokenInformation();
            var result = await _appManager.AddFoodRate(request, accountInfo);

            return new JsonResult(result);
        }

        [HttpGet]
        [Route("Add/AthletsToRelatives")]
        public async Task<IActionResult> AddAthletsToRelatives([FromQuery] string relativeId,
            [FromQuery] string athletId, [FromQuery] string role)
        {
            var accountInfo = GetTokenInformation();
            var result = await _appManager.AddAthletsToRelatives(relativeId, athletId, role, accountInfo);

            return new JsonResult(result);
        }

        [HttpGet]
        [Route("Add/AthletsToGroup")]
        public async Task<IActionResult> AddAthletsToGroup([FromQuery] string groupId, [FromQuery] string athletId)
        {
            var accountInfo = GetTokenInformation();
            var result = await _appManager.AddAthletsToGroup(groupId, athletId, accountInfo);

            return new JsonResult(result);
        }

        [HttpGet]
        [Route("Add/CoachToInstitution")]
        public async Task<IActionResult> AddCoachToInstitution([FromQuery] string coachId, [FromQuery] string position)
        {
            var accountInfo = GetTokenInformation();

            if (accountInfo != null && accountInfo.IsAdmin)
            {
                var result = await _appManager.AddCoachToInstitution(coachId, position, accountInfo);

                return new JsonResult(result);
            }

            return new JsonResult(_userIsNotAdmin);
        }

        [HttpGet]
        [Route("AthletByRelative")]
        public async Task<IActionResult> GetAthletByRelative([FromQuery] string relativeId)
        {
            var accountInfo = GetTokenInformation();

            if (accountInfo != null)
            {
                var result = await _appManager.GetAthletByRelative(relativeId);

                return new JsonResult(result);
            }

            return new JsonResult(_userIsNotAdmin); 
        }
      
        #endregion

        #region Get

        [HttpGet]
        [Route("UserByCreds")]
        public async Task<IActionResult> GetUserByCreds([FromQuery] string login, [FromQuery] string password)
        {
            var result = await _appManager.GetUserByCreds(login, password);

            if (result.Completed)
            {
                var response = new ServiseResponse<GetUserByCredsResponse>
                {
                    Data = new()
                    {
                        Token = _encryptionService.Encrypt(result.Data), IsAdmin = result.Data.IsAdmin,
                        Id = result.Data.Id
                    },
                    Completed = result.Completed,
                    Message = result.Message
                };

                return new JsonResult(response);
            }

            return new JsonResult(result);
        }

        [HttpGet]
        [Route("ProfileInfo")]
        public async Task<IActionResult> GetProfileInfo()
        {
            var accountInfo = GetTokenInformation();

            if (accountInfo == null)
            {
                return new JsonResult(_userIsNotAuthorized);
            }

            if (accountInfo.IsAdmin)
            {
                var result = await _appManager.GetInstitutionProfileInfo(accountInfo);

                return new JsonResult(result);
            }
            else
            {
                var result = await _appManager.GetUserProfileInfo(accountInfo);

                return new JsonResult(result);
            }
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetProfileInfoById([FromRoute] string id, [FromQuery] bool isAdminUser)
        {
            var accountInfo = GetTokenInformation();

            if (isAdminUser)
            {
                var result = await _appManager.GetInstitutionById(accountInfo, id);

                return new JsonResult(result);
            }
            else
            {
                var result = await _appManager.GetUserById(accountInfo, id);

                return new JsonResult(result);
            }
        }


        [HttpGet]
        [Route("Athlets/{groupId}")]
        public async Task<IActionResult> GetAthletsByGroup([FromRoute] string groupId)
        {
            var accountInfo = GetTokenInformation();
            var result = await _appManager.GetAthletsByGroup(groupId, accountInfo);

            return new JsonResult(result);
        }

        [HttpGet]
        [Route("Groups/{athletId}")]
        public async Task<IActionResult> GetGroupsByAthlet([FromRoute] string athletId)
        {
            var accountInfo = GetTokenInformation();
            var result = await _appManager.GetGroupsByAthlet(athletId, accountInfo);

            return new JsonResult(result);
        }

        [HttpGet]
        [Route("Groups")]
        public async Task<IActionResult> GetGroupsByCoach([FromQuery] string couchId)
        {
            var accountInfo = GetTokenInformation();
            var result = await _appManager.GetGroupsByCoach(couchId, accountInfo);

            return new JsonResult(result);
        }

        [HttpGet]
        [Route("Coaches")]
        public async Task<IActionResult> GetCoachesByInstitution([FromQuery] string institutionId)
        {
            var accountInfo = GetTokenInformation();
            var result = await _appManager.GetCoachesByInstitution(institutionId, accountInfo);

            return new JsonResult(result);
        }

        [HttpGet]
        [Route("Athlet/BodyInformation")]
        public async Task<IActionResult> GetAthletBodyInformation([FromQuery] string athletId)
        {
            var accountInfo = GetTokenInformation();
            var result = await _appManager.GetAthletBodyInformation(athletId, accountInfo);

            return new JsonResult(result);
        }

        [HttpGet]
        [Route("Athlet/NutrionInformation")]
        public async Task<IActionResult> GetAthletNutrionInformation([FromQuery] string athletId)
        {
            var accountInfo = GetTokenInformation();
            var result = await _appManager.GetAthletNutrionInformation(athletId, accountInfo);

            return new JsonResult(result);
        }


        [HttpGet]
        [Route("Group/BodyInformation")]
        public async Task<IActionResult> GetGroupBodyInformation([FromQuery] string groupId)
        {
            var accountInfo = GetTokenInformation();
            var result = await _appManager.GetGroupBodyInformation(groupId, accountInfo);

            return new JsonResult(result);
        }

        [HttpGet]
        [Route("Group/NutrionInformation")]
        public async Task<IActionResult> GetGroupNutrionInformation([FromQuery] string groupId)
        {
            var accountInfo = GetTokenInformation();
            var result = await _appManager.GetGroupNutrionInformation(groupId, accountInfo);

            return new JsonResult(result);
        }

        [HttpGet]
        [Route("User/Roles")]
        public async Task<IActionResult> GetUserRoles(string userId)
        {
            var accountInfo = GetTokenInformation();
            var result = await _appManager.GetUserRoles(userId, accountInfo);

            return new JsonResult(result);
        }

        [HttpGet]
        [Route("Group/{groupId}")]
        public async Task<IActionResult> GetGroup([FromRoute] string groupId)
        {
            var accountInfo = GetTokenInformation();
            var result = await _appManager.GetGroup(groupId, accountInfo);

            return new JsonResult(result);
        }

        #endregion

        #region Update

        [HttpPost]
        [Route("Update/NutrionInfo")]
        public async Task<IActionResult> UpdateNutrionInformation([FromBody] NutrionInformation request)
        {
            var accountInfo = GetTokenInformation();
            var result = await _appManager.UpdateNutrionInformation(request, accountInfo);

            return new JsonResult(result);
        }

        [HttpPost]
        [Route("Update/User")]
        public async Task<IActionResult> UpdateUser([FromBody] User request)
        {
            var accountInfo = GetTokenInformation();
            var result = await _appManager.UpdateUser(request, accountInfo);

            return new JsonResult(result);
        }

        [HttpPost]
        [Route("Update/Institution")]
        public async Task<IActionResult> UpdateInstitution([FromBody] Institution request)
        {
            var accountInfo = GetTokenInformation();
            var result = await _appManager.UpdateInstitution(request, accountInfo);

            return new JsonResult(result);
        }

        [HttpGet]
        [Route("Update/UserCreds")]
        public async Task<IActionResult> UpdateUserCreds([FromQuery] string login, [FromQuery] string password)
        { 
            var result = await _appManager.UpdateUserCreds(login, password);

            return new JsonResult(result);
        }

        #endregion

        #region Delete

        [HttpDelete]
        [Route("Group/Athlet")]
        public async Task<IActionResult> RemoveAthletFromGroup([FromQuery] string athletId, [FromQuery] string groupId)
        {
            var accountInfo = GetTokenInformation();
            var result = _appManager.RemoveAthletFromGroup(athletId, groupId, accountInfo);

            return new JsonResult(result);
        }

        [HttpDelete]
        [Route("Relatives/Athlet")]
        public async Task<IActionResult> RemoveAthletFromRelative([FromQuery] string athletId,
            [FromQuery] string relativeId)
        {
            var accountInfo = GetTokenInformation();
            var result = _appManager.RemoveAthletFromRelative(athletId, relativeId, accountInfo);

            return new JsonResult(result);
        }

        [HttpDelete]
        [Route("NutrionInformation")]
        public async Task<IActionResult> RemoveNutrionInformation([FromQuery] string nutrionId)
        {
            var accountInfo = GetTokenInformation();
            var result = _appManager.RemoveNutrionInformation(nutrionId, accountInfo);

            return new JsonResult(result);
        }

        #endregion


        #region Analyzing

        [HttpGet]
        [Route("Analyze/Nutrion/{athletId}")]
        public async Task<IActionResult> AnalyzeNutrion([FromRoute] string athletId)
        {
            var accountInfo = GetTokenInformation();
            var result = await _appManager.AnalyzeNutrion(athletId, accountInfo);

            return new JsonResult(result);
        }

        [HttpGet]
        [Route("Analyze/BodyParams/{athletId}")]
        public async Task<IActionResult> AnalyzeBodyParams([FromRoute] string athletId)
        {
            var accountInfo = GetTokenInformation();
            var result = await _appManager.AnalyzeBodyParams(athletId, accountInfo);

            return new JsonResult(result);
        }

        #endregion

        private AuthorizationIdentifier GetTokenInformation()
        {
            try
            {
                var request = this.HttpContext.Request.Headers;
                var token = request["Authorization"].First();
                var account = this._encryptionService.Decrypt<AuthorizationIdentifier>(token);

                return account;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}