using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HealthyLife.Model;
using HealthyLife.Model.Enum;
using HealthyLife.RequestModel;
using HealthyLife.ResponseModel;
using HealthyLife.Service.App;
using HealthyLife.Utility;
using Group = HealthyLife.Model.Group;

namespace HealthyLife.Manager.App
{
    public class AppManager : IAppManager
    {
        private IAppService _appService { get; set; }
        private List<string> UserRoles = new() {"Athlet", "Coach", "Relative"};

        public AppManager(IAppService appService)
        {
            _appService = appService;
        }

        public Task<ServiseResponse<AuthorizationIdentifier>> CreateUser(CreateUserRequest user)
        {
            return _appService.CreateUser(user);
        }

        public Task<ServiseResponse<AuthorizationIdentifier>> GetUserByCreds(string login, string password)
        {
            return _appService.GetUserByCreds(login, password);
        }

        public Task<ServiseResponse<string>> CreateInstitution(CreateInstitutionRequest institution)
        {
            return _appService.CreateInstitution(institution);
        }

        public async Task<ServiseResponse<string>> AddUserRole(string role, AuthorizationIdentifier identifier)
        {
            if (!UserRoles.Contains(role))
            {
                return ConvertToServiceResponse("", "This role was not found", false);
            }

            if (identifier == null)
            {
                return ConvertToServiceResponse("", "User is not authorized", false);
            }

            return await _appService.AddUserRole(role, identifier);
        }

        public async Task<ServiseResponse<string>> AddBodyInformation(AddBodyInformationRequest request,
            AuthorizationIdentifier identifier)
        {
            if (CheckId(request.AthletId))
            {
                return ConvertToServiceResponse("", "Id is not correct", false);
            }

            if (identifier == null)
            {
                return ConvertToServiceResponse("", "User is not authorized", false);
            }

            return await _appService.AddBodyInformation(request, identifier);
        }

        public Task<ServiseResponse<string>> CreateInstitutionAdmin(CreateInstitutionAdminRequest user)
        {
            return _appService.CreateInstitutionAdmin(user);
        }

        public Task<ServiseResponse<string>> CreateGroup(CreateGroupRequest group)
        {
            return _appService.CreateGroup(group);
        }
        public async Task<ServiseResponse<string>> AddNutrionInformation(AddNutrionInformationRequest request,
            AuthorizationIdentifier identifier)
        {
            if (CheckId(request.AthletId))
            {
                return ConvertToServiceResponse("", "Id is not correct", false);
            }

            if (identifier == null)
            {
                return ConvertToServiceResponse("", "User is not authorized", false);
            }

            return await _appService.AddNutrionInformation(request, identifier);
        }

        public async Task<ServiseResponse<string>> AddFoodRate(AddFoodRateRequest request,
            AuthorizationIdentifier identifier)
        {
            if (CheckId(request.AthletId) || CheckId(request.CoachId))
            {
                return ConvertToServiceResponse("", "Id is not correct", false);
            }

            if (identifier == null)
            {
                return ConvertToServiceResponse("", "User is not authorized", false);
            }

            return await _appService.AddFoodRate(request, identifier);
        }

        public async Task<ServiseResponse<string>> AddAthletsToRelatives(string relativeId, string athletId,
            string role, AuthorizationIdentifier identifier)
        {
            if (athletId == relativeId)
            {
                return ConvertToServiceResponse("", "Id shouldn't be equal", false);
            }

            if (CheckId(athletId) || CheckId(relativeId))
            {
                return ConvertToServiceResponse("", "Id is not correct", false);
            }

            if (identifier == null)
            {
                return ConvertToServiceResponse("", "User is not authorized", false);
            }

            return await _appService.AddAthletsToRelatives(relativeId, athletId, role, identifier);
        }

        public async Task<ServiseResponse<string>> AddAthletsToGroup(string groupId, string athletId,
            AuthorizationIdentifier identifier)
        {
            if (CheckId(athletId) || CheckId(groupId))
            {
                return ConvertToServiceResponse("", "Id is not correct", false);
            }

            if (identifier == null)
            {
                return ConvertToServiceResponse("", "User is not authorized", false);
            }

            return await _appService.AddAthletsToGroup(groupId, athletId, identifier);
        }

        public async Task<ServiseResponse<string>> AddCoachToInstitution(string coachId,
            string position, AuthorizationIdentifier identifier)
        {
            if ( CheckId(coachId))
            {
                return ConvertToServiceResponse("", "Id is not correct", false);
            }
            
            return await _appService.AddCoachToInstitution(coachId, position, identifier);
        }

        public async Task<ServiseResponse<List<CustomItem<Group>>>> GetGroupsByCoach(string couchId,
            AuthorizationIdentifier identifier)
        {
            if (CheckId(couchId))
            {
                return ConvertToServiceResponse<List<CustomItem<Group>>>(null, "Id is not correct", false);
            }

            if (identifier == null)
            {
                return ConvertToServiceResponse<List<CustomItem<Group>>>(null, "User is not authorized", false);
            }

            return await _appService.GetGroupsByCoach(couchId, identifier);
        }

        public async Task<ServiseResponse<List<User>>> GetCoachesByInstitution(string institutionId,
            AuthorizationIdentifier identifier)
        {
            if (CheckId(institutionId))
            {
                return ConvertToServiceResponse<List<User>>(null, "Id is not correct", false);
            }

            if (identifier == null)
            {
                return ConvertToServiceResponse<List<User>>(null, "User is not authorized", false);
            }

            return await _appService.GetCoachesByInstitution(institutionId, identifier);
        }

        public async Task<ServiseResponse<List<User>>> GetAthletsByGroup(string groupId,
            AuthorizationIdentifier identifier)
        {
            if (CheckId(groupId))
            {
                return ConvertToServiceResponse<List<User>>(null, "Id is not correct", false);
            }

            if (identifier == null)
            {
                return ConvertToServiceResponse<List<User>>(null, "User is not authorized", false);
            }

            return await _appService.GetAthletsByGroup(groupId, identifier);
        }

        public async Task<ServiseResponse<List<Group>>> GetGroupsByAthlet(string athletId,
            AuthorizationIdentifier identifier)
        {
            if (CheckId(athletId))
            {
                return ConvertToServiceResponse<List<Group>>(null, "Id is not correct", false);
            }

            if (identifier == null)
            {
                return ConvertToServiceResponse<List<Group>>(null, "User is not authorized", false);
            }

            return await _appService.GetGroupsByAthlet(athletId, identifier);
        }

        public async Task<ServiseResponse<string>> AnalyzeNutrion(string athletId, AuthorizationIdentifier identifier)
        {
            if (CheckId(athletId))
            {
                return ConvertToServiceResponse("", "Id is not correct", false);
            }

            if (identifier == null)
            {
                return ConvertToServiceResponse("", "User is not authorized", false);
            }

            return await _appService.AnalyzeNutrion(athletId, identifier);
        }

        public async Task<ServiseResponse<string>> AnalyzeBodyParams(string athletId,
            AuthorizationIdentifier identifier)
        {
            if (CheckId(athletId))
            {
                return ConvertToServiceResponse("", "Id is not correct", false);
            }

            if (identifier == null)
            {
                return ConvertToServiceResponse("", "User is not authorized", false);
            }

            return await _appService.AnalyzeBodyParams(athletId, identifier);
        }


        public async Task<ServiseResponse<Institution>> GetInstitutionProfileInfo(AuthorizationIdentifier identifier)
        {
            if (identifier == null)
            {
                return ConvertToServiceResponse<Institution>(null, "User is not authorized", false);
            }

            return await _appService.GetInstitutionProfileInfo(identifier);
        }
        
        public async Task<ServiseResponse<User>> GetUserProfileInfo(AuthorizationIdentifier identifier)
        {
            if (identifier == null)
            {
                return ConvertToServiceResponse<User>(null, "User is not authorized", false);
            }

            return await _appService.GetUserProfileInfo(identifier);
        }
        
        public async Task<ServiseResponse<Institution>> GetInstitutionById(AuthorizationIdentifier identifier,
            string id)
        {
            if (identifier == null)
            {
                return ConvertToServiceResponse<Institution>(null, "User is not authorized", false);
            }

            return await _appService.GetInstitutionById(id);
        }
        public async Task<ServiseResponse<User>> GetUserById(AuthorizationIdentifier identifier, string id)
        { if (identifier == null)
            {
                return ConvertToServiceResponse<User>(null, "User is not authorized", false);
            }

            return await _appService.GetUserById(id);}

        public async Task<ServiseResponse<List<BodyInformation>>> GetAthletBodyInformation(string athletId,
            AuthorizationIdentifier identifier)
        {
            if (CheckId(athletId))
            {
                return ConvertToServiceResponse<List<BodyInformation>>(null, "Id is not correct", false);
            }

            if (identifier == null)
            {
                return ConvertToServiceResponse<List<BodyInformation>>(null, "User is not authorized", false);
            }

            return await _appService.GetAthletBodyInformation(athletId, identifier);
        }

        public async Task<ServiseResponse<List<NutrionInformation>>> GetAthletNutrionInformation(string athletId,
            AuthorizationIdentifier identifier)
        {
            if (CheckId(athletId))
            {
                return ConvertToServiceResponse<List<NutrionInformation>>(null, "Id is not correct", false);
            }

            if (identifier == null)
            {
                return ConvertToServiceResponse<List<NutrionInformation>>(null, "User is not authorized", false);
            }

            return await _appService.GetAthletNutrionInformation(athletId, identifier);
        }

        public async Task<ServiseResponse<List<BodyInformation>>> GetGroupBodyInformation(string groupId,
            AuthorizationIdentifier identifier)
        {
            if (CheckId(groupId))
            {
                return ConvertToServiceResponse<List<BodyInformation>>(null, "Id is not correct", false);
            }

            if (identifier == null)
            {
                return ConvertToServiceResponse<List<BodyInformation>>(null, "User is not authorized", false);
            }

            return await _appService.GetGroupBodyInformation(groupId, identifier);
        }

        public async Task<ServiseResponse<List<NutrionInformation>>> GetGroupNutrionInformation(string groupId,
            AuthorizationIdentifier identifier)
        {
            if (CheckId(groupId))
            {
                return ConvertToServiceResponse<List<NutrionInformation>>(null, "Id is not correct", false);
            }

            if (identifier == null)
            {
                return ConvertToServiceResponse<List<NutrionInformation>>(null, "User is not authorized", false);
            }

            return await _appService.GetGroupNutrionInformation(groupId, identifier);
        }

        public async Task<ServiseResponse<string>> RemoveNutrionInformation(string nutrionId,
            AuthorizationIdentifier identifier)
        {
            if (CheckId(nutrionId))
            {
                return ConvertToServiceResponse("", "Id is not correct", false);
            }

            if (identifier == null)
            {
                return ConvertToServiceResponse<string>(null, "User is not authorized", false);
            }

            return await _appService.RemoveNutrionInformation(nutrionId, identifier);
        }

        public async Task<ServiseResponse<string>> RemoveAthletFromGroup(string athletId, string groupId,
            AuthorizationIdentifier identifier)
        {
            if (CheckId(athletId) || CheckId(groupId))
            {
                return ConvertToServiceResponse("", "Id is not correct", false);
            }

            if (identifier == null)
            {
                return ConvertToServiceResponse("", "User is not authorized", false);
            }

            return await _appService.RemoveAthletFromGroup(athletId, groupId, identifier);
        }

        public async Task<ServiseResponse<string>> RemoveAthletFromRelative(string athletId, string relativeId,
            AuthorizationIdentifier identifier)
        {
            if (CheckId(athletId) || CheckId(relativeId))
            {
                return ConvertToServiceResponse("", "Id is not correct", false);
            }

            if (identifier == null)
            {
                return ConvertToServiceResponse("", "User is not authorized", false);
            }

            return await _appService.RemoveAthletFromRelative(athletId, relativeId, identifier);
        }

        public async Task<ServiseResponse<string>> UpdateNutrionInformation(NutrionInformation request,
            AuthorizationIdentifier identifier)
        {
            if (CheckId(request.Id))
            {
                return ConvertToServiceResponse("", "Id is not correct", false);
            }

            if (identifier == null)
            {
                return ConvertToServiceResponse("", "User is not authorized", false);
            }

            return await _appService.UpdateNutrionInformation(request, identifier);
        }

        public async Task<ServiseResponse<string>> UpdateUser(User request, AuthorizationIdentifier identifier)
        {
            if (CheckId(request.Id))
            {
                return ConvertToServiceResponse("", "Id is not correct", false);
            }

            if (identifier == null)
            {
                return ConvertToServiceResponse("", "User is not authorized", false);
            }

            return await _appService.UpdateUser(request, identifier);
        }

        public async Task<ServiseResponse<string>> UpdateInstitution(Institution request,
            AuthorizationIdentifier identifier)
        {
            if (CheckId(request.Id))
            {
                return ConvertToServiceResponse("", "Id is not correct", false);
            }

            if (identifier == null)
            {
                return ConvertToServiceResponse("", "User is not authorized", false);
            }

            return await _appService.UpdateInstitution(request, identifier);
        }

        public async Task<ServiseResponse<List<UserRole>>> GetUserRoles(string id, AuthorizationIdentifier identifier)
        {
            if (CheckId(id))
            {
                return ConvertToServiceResponse<List<UserRole>>(null, "Id is not correct", false);
            }

            if (identifier == null)
            {
                return ConvertToServiceResponse<List<UserRole>>(null, "User is not authorized", false);
            }

            return await _appService.GetUserRoles(id, identifier);
        }

        public async Task<ServiseResponse<GroupResponse>> GetGroup(string groupId, AuthorizationIdentifier identifier)
        {
            if (CheckId(groupId))
            {
                return ConvertToServiceResponse<GroupResponse>(null, "Id is not correct", false);
            }

            if (identifier == null)
            {
                return ConvertToServiceResponse<GroupResponse>(null, "User is not authorized", false);
            }

            return await _appService.GetGroup(groupId, identifier);
        }
        
        public async Task<ServiseResponse<string>> UpdateUserCreds(string login, string password)
        {
            return await _appService.UpdateUserCreds(login, password);
        }

        public async Task<ServiseResponse<List<AccountData>>> GetAthletByRelative(string relativeId)
        {
            return await _appService.GetAthletByRelative(relativeId);
        }

        #region Private methods

        private bool CheckId(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return false;
            }

            Regex guidRegEx =
                new Regex(
                    @"^(\{{0,1}([0-9a-fA-F]){8}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){12}\}{0,1})$");

            return !guidRegEx.IsMatch(id);
        }

        private ServiseResponse<T> ConvertToServiceResponse<T>(T data, string message = "", bool completed = true)
        {
            return new ServiseResponse<T>
            {
                Message = message,
                Completed = completed,
                Data = data
            };
        }

        #endregion
    }
}