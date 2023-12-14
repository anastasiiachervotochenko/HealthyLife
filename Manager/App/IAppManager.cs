using System.Collections.Generic;
using System.Threading.Tasks;
using HealthyLife.Model;
using HealthyLife.Model.Enum;
using HealthyLife.RequestModel;
using HealthyLife.ResponseModel;
using HealthyLife.Utility;

namespace HealthyLife.Manager.App
{
    public interface IAppManager
    {
        Task<ServiseResponse<AuthorizationIdentifier>> CreateUser(CreateUserRequest user);
        Task<ServiseResponse<AuthorizationIdentifier>> GetUserByCreds(string login, string password);
        Task<ServiseResponse<string>> CreateInstitution(CreateInstitutionRequest institution);
        Task<ServiseResponse<string>> AddUserRole(string role, AuthorizationIdentifier identifier);

        Task<ServiseResponse<string>> AddBodyInformation(AddBodyInformationRequest request,
            AuthorizationIdentifier identifier);

        Task<ServiseResponse<string>> CreateInstitutionAdmin(CreateInstitutionAdminRequest user);
        Task<ServiseResponse<string>> CreateGroup(CreateGroupRequest group);
        Task<ServiseResponse<string>> AddNutrionInformation(AddNutrionInformationRequest request,
            AuthorizationIdentifier identifier);

        Task<ServiseResponse<string>> AddFoodRate(AddFoodRateRequest request, AuthorizationIdentifier identifier);

        Task<ServiseResponse<string>> AddAthletsToRelatives(string relativeId, string athletId, string role,
            AuthorizationIdentifier identifier);

        Task<ServiseResponse<string>> AddAthletsToGroup(string groupId, string athletId,
            AuthorizationIdentifier identifier);

        Task<ServiseResponse<string>> AddCoachToInstitution(string coachId, string position,
            AuthorizationIdentifier identifier);

        Task<ServiseResponse<List<AccountData>>> GetAthletByRelative(string relativeId);
        Task<ServiseResponse<List<CustomItem<Group>>>> GetGroupsByCoach(string couchId,
            AuthorizationIdentifier identifier);

        Task<ServiseResponse<List<User>>> GetCoachesByInstitution(string institutionId,
            AuthorizationIdentifier identifier);

        Task<ServiseResponse<List<User>>> GetAthletsByGroup(string groupId, AuthorizationIdentifier identifier);
        Task<ServiseResponse<List<Group>>> GetGroupsByAthlet(string athletId, AuthorizationIdentifier identifier);
        Task<ServiseResponse<Institution>> GetInstitutionProfileInfo(AuthorizationIdentifier identifier);
        Task<ServiseResponse<User>> GetUserProfileInfo(AuthorizationIdentifier identifier);
        Task<ServiseResponse<Institution>> GetInstitutionById(AuthorizationIdentifier identifier, string id);
        Task<ServiseResponse<User>> GetUserById(AuthorizationIdentifier identifier, string id);
        Task<ServiseResponse<string>> AnalyzeNutrion(string athletId, AuthorizationIdentifier identifier);
        Task<ServiseResponse<string>> AnalyzeBodyParams(string athletId, AuthorizationIdentifier identifier);

        Task<ServiseResponse<List<BodyInformation>>> GetAthletBodyInformation(string athletId,
            AuthorizationIdentifier identifier);

        Task<ServiseResponse<List<NutrionInformation>>> GetAthletNutrionInformation(string athletId,
            AuthorizationIdentifier identifier);

        Task<ServiseResponse<List<BodyInformation>>> GetGroupBodyInformation(string groupId,
            AuthorizationIdentifier identifier);

        Task<ServiseResponse<List<NutrionInformation>>> GetGroupNutrionInformation(string groupId,
            AuthorizationIdentifier identifier);

        Task<ServiseResponse<string>> RemoveNutrionInformation(string nutrionId, AuthorizationIdentifier identifier);

        Task<ServiseResponse<string>> RemoveAthletFromGroup(string athletId, string groupId,
            AuthorizationIdentifier identifier);

        Task<ServiseResponse<string>> RemoveAthletFromRelative(string athletId, string relativeId,
            AuthorizationIdentifier identifier);

        Task<ServiseResponse<string>> UpdateNutrionInformation(NutrionInformation request,
            AuthorizationIdentifier identifier);

        Task<ServiseResponse<string>> UpdateUser(User request, AuthorizationIdentifier identifier);
        
        Task<ServiseResponse<string>> UpdateInstitution(Institution request, AuthorizationIdentifier identifier);
        
        Task<ServiseResponse<List<UserRole>>> GetUserRoles(string id, AuthorizationIdentifier identifier);
        Task<ServiseResponse<GroupResponse>> GetGroup(string groupId, AuthorizationIdentifier identifier);

        Task<ServiseResponse<string>> UpdateUserCreds(string login, string password);
    }
}