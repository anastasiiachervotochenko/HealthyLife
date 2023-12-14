using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HealthyLife.Context;
using HealthyLife.Model;
using HealthyLife.Model.Enum;
using HealthyLife.RequestModel;
using HealthyLife.ResponseModel;
using HealthyLife.Utility;
using Microsoft.EntityFrameworkCore;
using Group = HealthyLife.Model.Group;

namespace HealthyLife.Service.App
{
    public class AppService : IAppService
    {
        private readonly DevContext _context;
        private const string _errorValidationFail = "Validation fail";
        private const string _errorLoginIsNotUnique = "Login is not unique";
        private const string _exeedParams = "{0} exceed normal rate";
        private const string _lackParams = "Lack of {0}";

        public AppService(DevContext context)
        {
            _context = context;
        }

        public async Task<ServiseResponse<AuthorizationIdentifier>> CreateUser(CreateUserRequest user)
        {
            if (!Helper.Validator(user))
            {
                return Helper.ConvertToServiceResponse<AuthorizationIdentifier>(null, _errorValidationFail, false);
            }

            if (_context.Users.Any(u => u.Login == user.Login))
            {
                return Helper.ConvertToServiceResponse<AuthorizationIdentifier>(null, _errorLoginIsNotUnique, false);
            }

            var salt = Helper.CreateSalt(20);
            var hashPassword = Helper.GenerateHash(user.Password, salt);
            var userId = Guid.NewGuid().ToString();
            var accountLogId = Guid.NewGuid().ToString();

            _context.Users.Add(new()
            {
                Id = userId,
                Fio = user.Fio,
                Phone = user.Phone,
                Email = user.Email,
                BirthdayDate = user.BirthdayDate,
                Sex = user.Sex,
                Login = user.Login,
                Password = hashPassword,
                Salt = salt,
                Active = true
            });

            _context.AccountLogs.Add(new()
            {
                Id = accountLogId,
                UserId = userId,
                Role = ((UserRole) user.Role).ToString()
            });
            await _context.SaveChangesAsync();

            return Helper.ConvertToServiceResponse(new AuthorizationIdentifier
            {
                Id = Guid.Parse(userId),
                Login = user.Login,
                IsAdmin = false
            });
        }

        public async Task<ServiseResponse<AuthorizationIdentifier>> GetUserByCreds(string login, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Login == login && u.Active == true);
            var instAdmin = await _context.InstitutionLogs.FirstOrDefaultAsync(u => u.Login == login);

            if (user != null && Helper.AreEqual(password, user.Password, user.Salt))
            {
                var authInfo = new AuthorizationIdentifier
                {
                    Id = Guid.Parse(user.Id),
                    Login = user.Login,
                    IsAdmin = false
                };
                return Helper.ConvertToServiceResponse(authInfo);
            }
            else if (instAdmin != null && Helper.AreEqual(password, instAdmin.Password, instAdmin.Salt))
            {
                var authInfo = new AuthorizationIdentifier
                {
                    Id = Guid.Parse(instAdmin.InstitutionId),
                    Login = instAdmin.Login,
                    IsAdmin = true
                };
                return Helper.ConvertToServiceResponse(authInfo);
            }

            return Helper.ConvertToServiceResponse<AuthorizationIdentifier>(null, "Uncorrect creds", false);
        }

        public async Task<ServiseResponse<string>> CreateInstitution(CreateInstitutionRequest institution)
        {
            _context.Institutions.Add(new()
            {
                Id = Guid.NewGuid().ToString(),
                Name = institution.Name,
                Phone = institution.Phone,
                Email = institution.Email,
                Address = institution.Address,
                SiteLink = institution.SiteLink
            });
            await _context.SaveChangesAsync();
            return Helper.ConvertToServiceResponse("");
        }

        public async Task<ServiseResponse<string>> AddUserRole(string role, AuthorizationIdentifier identifier)
        {
            if (!IsAccountExist(identifier).Data)
            {
                return Helper.ConvertToServiceResponse("", "User was not found", false);
            }

            if (_context.AccountLogs.Any(a => a.UserId == identifier.Id.ToString() && a.Role == role))
            {
                return Helper.ConvertToServiceResponse("", "User has this role", false);
            }

            _context.AccountLogs.Add(new()
            {
                Id = Guid.NewGuid().ToString(),
                UserId = identifier.Id.ToString(),
                Role = role
            });
            await _context.SaveChangesAsync();
            return Helper.ConvertToServiceResponse("");
        }

        public async Task<ServiseResponse<string>> AddBodyInformation(AddBodyInformationRequest request,
            AuthorizationIdentifier identifier)
        {
            if (!IsAccountExist(identifier).Data)
            {
                return Helper.ConvertToServiceResponse("", "User was not found", false);
            }

            _context.BodyInformations.Add(new()
            {
                Id = Guid.NewGuid().ToString(),
                AthletId = request.AthletId,
                Height = request.Height,
                Weight = request.Weight,
                ChestGirth = request.ChestGirth,
                WaistCircumference = request.WaistCircumference,
                AbdominalGirth = request.AbdominalGirth,
                ButtocksGirth = request.ButtocksGirth,
                ThighGirth = request.ThighGirth,
                Date = DateTime.Now
            });
            await _context.SaveChangesAsync();
            return Helper.ConvertToServiceResponse("");
        }

        public async Task<ServiseResponse<string>> CreateInstitutionAdmin(CreateInstitutionAdminRequest user)
        {
            if (_context.InstitutionLogs.Any(u => u.Login == user.Login))
            {
                return Helper.ConvertToServiceResponse("", "Login is not unique", false);
            }

            var salt = Helper.CreateSalt(20);
            var hashPassword = Helper.GenerateHash(user.Password, salt);
            _context.InstitutionLogs.Add(new()
            {
                Id = Guid.NewGuid().ToString(),
                InstitutionId = user.InstitutionId,
                Login = user.Login,
                Password = hashPassword,
                Salt = salt
            });
            await _context.SaveChangesAsync();

            return Helper.ConvertToServiceResponse("");
        }

        public async Task<ServiseResponse<string>> CreateGroup(CreateGroupRequest group)
        {
            _context.Groups.Add(new()
            {
                Id = Guid.NewGuid().ToString(),
                GroupName = group.GroupName,
                Sport = group.Sport,
                Type = group.Type,
                CoachInInstitutionId = group.CoachInstitutionId,
                StartDate = group.StartDate,
                EndDate = group.EndDate,
            });
            await _context.SaveChangesAsync();

            return Helper.ConvertToServiceResponse("");
        }

        public async Task<ServiseResponse<string>> AddNutrionInformation(AddNutrionInformationRequest request,
            AuthorizationIdentifier identifier)
        {
            _context.NutrionInformations.Add(new NutrionInformation
            {
                Id = Guid.NewGuid().ToString(),
                AthletId = request.AthletId,
                Kcal = request.Kcal,
                Proteins = request.Proteins,
                Fats = request.Fats,
                Carbohydrates = request.Carbohydrates,
                AmountOfWater = request.AmountOfWater,
                Date = DateTime.Now
            });
            await _context.SaveChangesAsync();
            return Helper.ConvertToServiceResponse("");
        }

        public async Task<ServiseResponse<string>> AddFoodRate(AddFoodRateRequest request,
            AuthorizationIdentifier identifier)
        {
            if (!IsAccountExist(identifier).Data)
            {
                return Helper.ConvertToServiceResponse("", "User was not found", false);
            }

            _context.FoodRates.Add(new FoodRate
            {
                Id = Guid.NewGuid().ToString(),
                CoachId = request.CoachId,
                AthletId = request.AthletId,
                Kcal = request.Kcal,
                Proteins = request.Proteins,
                Fats = request.Fats,
                Carbohydrates = request.Carbohydrates,
                AmountOfWater = request.AmountOfWater,
                Date = DateTime.Now
            });
            await _context.SaveChangesAsync();
            return Helper.ConvertToServiceResponse("");
        }

        public async Task<ServiseResponse<string>> AddAthletsToRelatives(string relativeId, string athletId,
            string role, AuthorizationIdentifier identifier)
        {
            if (!IsAccountExist(identifier).Data || !IsUserExist(relativeId).Data || !IsUserExist(athletId).Data)
            {
                return Helper.ConvertToServiceResponse("", "User was not found", false);
            }

            if (_context.AthletsToRelatives.Any(x => x.AthleteId == athletId && x.RelativeId == relativeId))
            {
                return Helper.ConvertToServiceResponse("", "Users have relate now", false);
            }

            _context.AthletsToRelatives.Add(new AthletsToRelative()
            {
                Id = Guid.NewGuid().ToString(),
                RelativeId = relativeId,
                AthleteId = athletId,
                Role = role
            });
            await _context.SaveChangesAsync();
            return Helper.ConvertToServiceResponse("");
        }

        public async Task<ServiseResponse<string>> AddAthletsToGroup(string groupId, string athletId,
            AuthorizationIdentifier identifier)
        {
            if (!IsAccountExist(identifier).Data || !IsUserExist(athletId).Data)
            {
                return Helper.ConvertToServiceResponse("", "User was not found", false);
            }

            if (_context.AthletToGroups.Any(x => x.AthletId == athletId && x.GroupId == groupId && x.EndDate == null))
            {
                return Helper.ConvertToServiceResponse("", "User now in this group", false);
            }

            _context.AthletToGroups.Add(new AthletToGroup()
            {
                Id = Guid.NewGuid().ToString(),
                GroupId = groupId,
                AthletId = athletId,
                StartDate = DateTime.Today
            });
            await _context.SaveChangesAsync();
            return Helper.ConvertToServiceResponse("");
        }

        public async Task<ServiseResponse<string>> AddCoachToInstitution(string coachId,
            string position, AuthorizationIdentifier identifier)
        {
            if (!IsAccountExist(identifier).Data)
            {
                return Helper.ConvertToServiceResponse("", "User was not found", false);
            }

            if (_context.CoachToInstitutions.Any(x =>
                    x.CoachId == coachId && x.InstitutionId == identifier.Id.ToString() && x.EndDate == null))
            {
                return Helper.ConvertToServiceResponse("", "User now in this institution", false);
            }

            _context.CoachToInstitutions.Add(new CoachToInstitution()
            {
                Id = Guid.NewGuid().ToString(),
                CoachId = coachId,
                InstitutionId = identifier.Id.ToString(),
                Position = position,
                StartDate = DateTime.Today
            });
            await _context.SaveChangesAsync();
            return Helper.ConvertToServiceResponse("");
        }

        public async Task<ServiseResponse<List<CustomItem<Group>>>> GetGroupsByCoach(string coachId,
            AuthorizationIdentifier identifier)
        {
            if (!IsAccountExist(identifier).Data)
            {
                return Helper.ConvertToServiceResponse<List<CustomItem<Group>>>(null, "User was not found", false);
            }

            var institutionIds = _context.CoachToInstitutions.Where(x => x.CoachId.Equals(coachId)).Select(x => x.Id)
                .ToList();
            var dict = _context.Groups.Where(x => institutionIds.Contains(x.CoachInInstitutionId)).ToList()
                .GroupBy(x => x.CoachInInstitutionId).ToDictionary(x => x.Key, x => x.Select(x => x).ToList());
            var result = ConvertDictionaryToList(dict);

            for (var i = 0; i < result.Count(); i++)
            {
                var institution = await (from instCoach in _context.CoachToInstitutions
                    join inst in _context.Institutions on instCoach.InstitutionId equals inst.Id
                    where instCoach.Id == result[i].Id
                    select inst).FirstOrDefaultAsync();
                result[i].Id = institution.Id;
                result[i].Name = institution.Name;
            }

            return Helper.ConvertToServiceResponse(result);
        }

        public async Task<ServiseResponse<List<User>>> GetCoachesByInstitution(string institutionId,
            AuthorizationIdentifier identifier)
        {
            if (!IsAccountExist(identifier).Data)
            {
                return Helper.ConvertToServiceResponse<List<User>>(null, "User was not found", false);
            }

            var result = from user in _context.Users
                join cti in _context.CoachToInstitutions on user.Id equals cti.CoachId
                where cti.InstitutionId == institutionId
                select user;

            return Helper.ConvertToServiceResponse(result.ToList());
        }

        public async Task<ServiseResponse<List<User>>> GetAthletsByGroup(string groupId,
            AuthorizationIdentifier identifier)
        {
            if (!IsAccountExist(identifier).Data)
            {
                return Helper.ConvertToServiceResponse<List<User>>(null, "User was not found", false);
            }

            var result = from user in _context.Users
                join atg in _context.AthletToGroups on user.Id equals atg.AthletId
                where atg.GroupId == groupId && user.Active == true
                select user;

            return Helper.ConvertToServiceResponse(result.ToList());
        }

        public async Task<ServiseResponse<List<Group>>> GetGroupsByAthlet(string athletId,
            AuthorizationIdentifier identifier)
        {
            if (!IsAccountExist(identifier).Data)
            {
                return Helper.ConvertToServiceResponse<List<Group>>(null, "User was not found", false);
            }

            var result = from g in _context.Groups
                join atg in _context.AthletToGroups on g.Id equals atg.GroupId
                where atg.AthletId == athletId
                select g;

            return Helper.ConvertToServiceResponse(result.ToList());
        }

        public async Task<ServiseResponse<Institution>> GetInstitutionProfileInfo(AuthorizationIdentifier identifier)
        {
            if (!identifier.IsAdmin)
            {
                return Helper.ConvertToServiceResponse<Institution>(null, "You are not institution admin", false);
            }

            var result = (from inst in _context.Institutions
                join instLog in _context.InstitutionLogs on inst.Id equals instLog.InstitutionId
                where instLog.InstitutionId == identifier.Id.ToString() && instLog.Login == identifier.Login
                select inst).First();
            return Helper.ConvertToServiceResponse(result);
        }

        public async Task<ServiseResponse<User>> GetUserProfileInfo(AuthorizationIdentifier identifier)
        {
            if (identifier.IsAdmin)
            {
                return Helper.ConvertToServiceResponse<User>(null, "You are not simple user", false);
            }

            var result = (from user in _context.Users
                join accountLog in _context.AccountLogs on user.Id equals accountLog.UserId
                where user.Id == identifier.Id.ToString() && user.Login == identifier.Login
                select user).First();
            return Helper.ConvertToServiceResponse(result);
        }

        public async Task<ServiseResponse<Institution>> GetInstitutionById(string id)
        {
            var result = (from inst in _context.Institutions
                join instLog in _context.InstitutionLogs on inst.Id equals instLog.InstitutionId
                where instLog.InstitutionId == id
                select inst).First();
            return Helper.ConvertToServiceResponse(result);
        }

        public async Task<ServiseResponse<User>> GetUserById(string id)
        {
            var result = (from user in _context.Users
                join accountLog in _context.AccountLogs on user.Id equals accountLog.UserId
                where user.Id == id
                select user).First();
            return Helper.ConvertToServiceResponse(result);
        }


        public async Task<ServiseResponse<string>> AnalyzeNutrion(string athletId, AuthorizationIdentifier identifier)
        {
            if (!IsAccountExist(identifier).Data)
            {
                return Helper.ConvertToServiceResponse("", "User was not found", false);
            }

            var exeed = "";
            var lack = "";
            var foodRate = await _context.FoodRates.Where(x => x.AthletId == athletId).OrderBy(x=>x.Date).FirstOrDefaultAsync();

            var nutrionInfo = _context.NutrionInformations.Where(x => x.AthletId == athletId).GroupBy(x => x.AthletId)
                .Select(x => new NutrionInformation
                {
                    Kcal = (int) x.Select(data => data.Kcal).Average(),
                    Proteins = (int) x.Select(data => data.Proteins).Average(),
                    Fats = (int) x.Select(data => data.Fats).Average(),
                    Carbohydrates = (int) x.Select(data => data.Carbohydrates).Average(),
                    AmountOfWater = (int) x.Select(data => data.AmountOfWater).Average()
                }).First();
            AnalyzingNutrion(foodRate, nutrionInfo, "Kcal", ref exeed, ref lack);
            AnalyzingNutrion(foodRate, nutrionInfo, "Proteins", ref exeed, ref lack);
            AnalyzingNutrion(foodRate, nutrionInfo, "Fats", ref exeed, ref lack);
            AnalyzingNutrion(foodRate, nutrionInfo, "Carbohydrates", ref exeed, ref lack);
            AnalyzingNutrion(foodRate, nutrionInfo, "AmountOfWater", ref exeed, ref lack);
            return Helper.ConvertToServiceResponse(string.Format(_exeedParams, exeed) + "/n" +
                                                   string.Format(_lackParams, lack));
        }

        public async Task<ServiseResponse<string>> AnalyzeBodyParams(string athletId,
            AuthorizationIdentifier identifier)
        {
            if (!IsAccountExist(identifier).Data)
            {
                return Helper.ConvertToServiceResponse("", "User was not found", false);
            }

            var result = "";
            var maxDate = _context.BodyInformations.Where(x => x.AthletId == athletId).Max(x => x.Date);
            var bodyInfo = _context.BodyInformations.FirstOrDefault(x => x.AthletId == athletId && x.Date == maxDate);
            result = "Weight statistics: " + AnalyzingWeight(bodyInfo);
            result += "Waist to hip ratio: " + AnalyzingWaistButtocks(bodyInfo);
            
            return Helper.ConvertToServiceResponse(result);
        }

        public async Task<ServiseResponse<List<BodyInformation>>> GetAthletBodyInformation(string athletId,
            AuthorizationIdentifier identifier)
        {
            if (!IsAccountExist(identifier).Data)
            {
                return Helper.ConvertToServiceResponse<List<BodyInformation>>(null, "User was not found", false);
            }

            var information = _context.BodyInformations.Where(x => x.AthletId == athletId).ToList();

            return Helper.ConvertToServiceResponse(information);
        }

        public async Task<ServiseResponse<List<NutrionInformation>>> GetAthletNutrionInformation(string athletId,
            AuthorizationIdentifier identifier)
        {
            if (!IsAccountExist(identifier).Data)
            {
                return Helper.ConvertToServiceResponse<List<NutrionInformation>>(null, "User was not found", false);
            }

            var information = _context.NutrionInformations.Where(x => x.AthletId == athletId).ToList();

            return Helper.ConvertToServiceResponse(information);
        }

        public async Task<ServiseResponse<List<BodyInformation>>> GetGroupBodyInformation(string groupId,
            AuthorizationIdentifier identifier)
        {
            if (!IsAccountExist(identifier).Data)
            {
                return Helper.ConvertToServiceResponse<List<BodyInformation>>(null, "User was not found", false);
            }

            var athlets = from atg in _context.AthletToGroups
                where atg.GroupId == groupId && atg.EndDate == null
                select atg.AthletId;

            var information = _context.BodyInformations.Where(x => athlets.Contains(x.AthletId))
                .GroupBy(x => x.Date)
                .Select(x => new BodyInformation()
                {
                    Date = x.First().Date,
                    AbdominalGirth = x.Select(athlet => athlet.AbdominalGirth).Average(),
                    ButtocksGirth = x.Select(athlet => athlet.ButtocksGirth).Average(),
                    ChestGirth = x.Select(athlet => athlet.ChestGirth).Average(),
                    Height = x.Select(athlet => athlet.Height).Average(),
                    Weight = x.Select(athlet => athlet.Weight).Average(),
                    ThighGirth = x.Select(athlet => athlet.ThighGirth).Average(),
                    WaistCircumference = x.Select(athlet => athlet.WaistCircumference).Average(),
                });

            return Helper.ConvertToServiceResponse(information.ToList());
        }

        public async Task<ServiseResponse<List<NutrionInformation>>> GetGroupNutrionInformation(string groupId,
            AuthorizationIdentifier identifier)
        {
            if (!IsAccountExist(identifier).Data)
            {
                return Helper.ConvertToServiceResponse<List<NutrionInformation>>(null, "User was not found", false);
            }

            var athlets = await (from atg in _context.AthletToGroups
                where atg.GroupId == groupId && atg.EndDate == null
                select atg.AthletId).ToListAsync();

            var information = await _context.NutrionInformations.Where(x => athlets.Contains(x.AthletId))
                .GroupBy(x => x.Date)
                .Select(x => new NutrionInformation()
                {
                    Date = x.Key,
                    Kcal = (int) x.Select(athlet => athlet.Kcal).Average(),
                    AmountOfWater = (int) x.Select(athlet => athlet.AmountOfWater).Average(),
                    Carbohydrates = (int) x.Select(athlet => athlet.Carbohydrates).Average(),
                    Fats = (int) x.Select(athlet => athlet.Fats).Average(),
                    Proteins = (int) x.Select(athlet => athlet.Proteins).Average()
                }).ToListAsync();

            return Helper.ConvertToServiceResponse(information);
        }

        public async Task<ServiseResponse<string>> RemoveNutrionInformation(string nutrionId,
            AuthorizationIdentifier identifier)
        {
            if (!IsAccountExist(identifier).Data)
            {
                return Helper.ConvertToServiceResponse("", "User was not found", false);
            }

            var entity = _context.NutrionInformations.Where(x => x.Id == nutrionId).First();
            _context.NutrionInformations.Remove(entity);
            await _context.SaveChangesAsync();

            return Helper.ConvertToServiceResponse("");
        }


        public async Task<ServiseResponse<string>> RemoveAthletFromGroup(string athletId, string groupId,
            AuthorizationIdentifier identifier)
        {
            if (!IsAccountExist(identifier).Data)
            {
                return Helper.ConvertToServiceResponse("", "User was not found", false);
            }

            var entity = _context.AthletToGroups.Where(x => x.AthletId == athletId && x.EndDate == null).First();
            entity.EndDate = DateTime.Now;
            _context.AthletToGroups.Update(entity);
            await _context.SaveChangesAsync();

            return Helper.ConvertToServiceResponse("");
        }

        public async Task<ServiseResponse<string>> RemoveAthletFromRelative(string athletId, string relativeId,
            AuthorizationIdentifier identifier)
        {
            if (!IsAccountExist(identifier).Data)
            {
                return Helper.ConvertToServiceResponse("", "User was not found", false);
            }

            var entity = _context.AthletsToRelatives.Where(x => x.AthleteId == athletId && x.RelativeId == relativeId);
            _context.AthletsToRelatives.RemoveRange(entity);
            await _context.SaveChangesAsync();

            return Helper.ConvertToServiceResponse("");
        }

        public async Task<ServiseResponse<string>> UpdateNutrionInformation(NutrionInformation request,
            AuthorizationIdentifier identifier)
        {
            if (!IsAccountExist(identifier).Data)
            {
                return Helper.ConvertToServiceResponse("", "User was not found", false);
            }

            if (!_context.NutrionInformations.Any(x => x == request))
            {
                return Helper.ConvertToServiceResponse("", "Nutrion information is not correct", false);
            }

            _context.NutrionInformations.Update(request);
            await _context.SaveChangesAsync();

            return Helper.ConvertToServiceResponse("");
        }

        public async Task<ServiseResponse<string>> UpdateUser(User request, AuthorizationIdentifier identifier)
        {
            if (!IsAccountExist(identifier).Data)
            {
                return Helper.ConvertToServiceResponse("", "User was not found", false);
            }

            if (!_context.Users.Any(x => x == request))
            {
                return Helper.ConvertToServiceResponse("", "User is not correct", false);
            }

            _context.Users.Update(request);
            await _context.SaveChangesAsync();

            return Helper.ConvertToServiceResponse("");
        }


        public async Task<ServiseResponse<string>> UpdateInstitution(Institution request,
            AuthorizationIdentifier identifier)
        {
            if (!IsAccountExist(identifier).Data)
            {
                return Helper.ConvertToServiceResponse("", "User was not found", false);
            }

            if (!_context.Institutions.Any(x => x == request))
            {
                return Helper.ConvertToServiceResponse("", "Institution is not correct", false);
            }

            _context.Institutions.Update(request);
            await _context.SaveChangesAsync();

            return Helper.ConvertToServiceResponse("");
        }

        public async Task<ServiseResponse<string>> UpdateUserCreds(string login, string password)
        {
            if (!_context.Users.Any(x => x.Login == login && x.Active == true))
            {
                return Helper.ConvertToServiceResponse("", "Login was not found", false);
            }

            var entity = await _context.Users.FirstOrDefaultAsync(x => x.Login == login && x.Active == true);

            var salt = Helper.CreateSalt(20);
            var hashPassword = Helper.GenerateHash(password, salt);
            entity.Password = hashPassword;
            entity.Salt = salt;
            _context.Users.Update(entity);
            await _context.SaveChangesAsync();

            return Helper.ConvertToServiceResponse("");
        }

        public async Task<ServiseResponse<List<UserRole>>> GetUserRoles(string id, AuthorizationIdentifier? identifier)
        {
            var roles = _context.AccountLogs.Where(user => user.UserId == id).Select(user => user.Role).ToList();
            var result = new List<UserRole>();
            foreach (var role in roles)
            {
                var roleToAdd = role == UserRole.Athlet.ToString()
                    ? UserRole.Athlet
                    : role == UserRole.Coach.ToString()
                        ? UserRole.Coach
                        : UserRole.Relative;
                result.Add(roleToAdd);
            }

            return Helper.ConvertToServiceResponse(result);
        }

        public async Task<ServiseResponse<GroupResponse>> GetGroup(string groupId, AuthorizationIdentifier identifier)
        {
            var result = await (from g in _context.Groups
                    join instCoach in _context.CoachToInstitutions on g.CoachInInstitutionId equals instCoach.Id
                    join user in _context.Users on instCoach.CoachId equals user.Id
                    where g.Id == groupId
                    select new GroupResponse()
                    {
                        Id = g.Id,
                        GroupName = g.GroupName,
                        Sport = (Sport) Convert.ToInt32(g.Sport),
                        Type = (GroupType) Convert.ToInt32(g.Type),
                        StartDate = g.StartDate,
                        EndDate = g.EndDate,
                        CoachName = user.Fio,
                        CoachInInstitutionId = g.CoachInInstitutionId
                    }
                ).FirstOrDefaultAsync();

            return Helper.ConvertToServiceResponse(result);
        }

        public async Task<ServiseResponse<List<AccountData>>> GetAthletByRelative(string relativeId)
        {
            var result = await (from atr in _context.AthletsToRelatives
                join u in _context.Users on atr.AthleteId equals u.Id
                where atr.RelativeId == relativeId
                select new AccountData
                {
                    Id = u.Id,
                    BirthdayDate = u.BirthdayDate,
                    Email = u.Email,
                    Fio = u.Fio,
                    Login = u.Login,
                    Password = u.Password,
                    Phone = u.Phone,
                    Sex = u.Sex
                }).ToListAsync();
            
            return Helper.ConvertToServiceResponse(result);
        }

        public ServiseResponse<bool> IsAccountExist(AuthorizationIdentifier identifier)
        {
            if (identifier.IsAdmin)
            {
                var result =
                    _context.InstitutionLogs.Any(x =>
                        x.InstitutionId == identifier.Id.ToString() && x.Login == identifier.Login);
                return Helper.ConvertToServiceResponse(result);
            }
            else
            {
                var result = _context.Users.Any(x => x.Id == identifier.Id.ToString() && x.Login == identifier.Login  && x.Active == true);
                return Helper.ConvertToServiceResponse(result);
            }
        }

        public ServiseResponse<bool> IsUserExist(string id)
        {
            return Helper.ConvertToServiceResponse(_context.Users.Any(x => x.Id == id));
        }

        #region Private methods

        private Difference CalculateDifference(double firstValue, double secondValue)
        {
            var percentOfNorm = 10;
            if (firstValue == secondValue)
            {
                return Difference.Norm;
            }

            var difference = (secondValue - firstValue) * 100 / percentOfNorm;
            if (difference >= (-1) * percentOfNorm && difference <= percentOfNorm)
            {
                return Difference.Norm;
            }

            if (difference > 0)
            {
                return Difference.High;
            }

            return Difference.Low;
        }

        private void AnalyzingNutrion<T, K>(T idealData, K realData, string property, ref string exeedParams,
            ref string lackParams)
        {
            var idealPropertyValue = idealData.GetType().GetProperties().Where(x => x.Name == property).First()
                .GetValue(idealData);
            var realPropertyValue = realData.GetType().GetProperties().Where(x => x.Name == property).First()
                .GetValue(realData);

            if (CalculateDifference((double) idealPropertyValue, (double) realPropertyValue) == Difference.High)
            {
                exeedParams += property + " ;";
            }

            if (CalculateDifference((double) idealPropertyValue, (double) realPropertyValue) == Difference.Low)
            {
                lackParams += property + " ;";
            }
        }

        private string AnalyzingWeight(BodyInformation body)
        {
            var imt = body.Weight / Math.Pow(Convert.ToDouble(body.Height) / 100, 2);
            if (imt < 18.5)
            {
                return "Underweight";
            }
            else if (imt < 24.99)
            {
                return "Normal weight";
            }
            else if (imt < 30)
            {
                return "Overweight ";
            }
            else
            {
                return "Obesity";
            }
        }

        private string AnalyzingWaistButtocks(BodyInformation body)
        {
            var ratio = body.WaistCircumference / body.ButtocksGirth;
            if (ratio < 0.75)
            {
                return "Perfect";
            }
            else if (ratio < 0.8)
            {
                return "Good";
            }
            else if (ratio < 0.85)
            {
                return "Not bad";
            }
            else if (ratio < 0.9)
            {
                return "Bad";
            }
            else
            {
                return "Very bad";
            }
        }

        private enum Difference
        {
            Norm,
            High,
            Low
        }

        private List<CustomItem<T>> ConvertDictionaryToList<T>(Dictionary<string, List<T>> dict)
        {
            List<CustomItem<T>> list = new();
            foreach (var d in dict)
            {
                list.Add(new() {Id = d.Key, Items = d.Value});
            }

            return list;
        }

        #endregion
    }
}