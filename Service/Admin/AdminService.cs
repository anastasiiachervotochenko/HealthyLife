using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HealthyLife.Context;
using HealthyLife.Model;
using HealthyLife.ResponseModel;
using HealthyLife.Utility;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace HealthyLife.Service.Admin
{
    public class AdminService : IAdminService
    {
        private readonly DevContext _context;
        private readonly IConfiguration _config;
        private string _errorLoginIsNotUnique = "Login is not unique";
        private string _errorAuthorization = "Login or password are not valid";


        public AdminService(DevContext context, IConfiguration config)
        {
            _context = context;
            _config = config; 
        }

        public async  Task<ServiseResponse<AuthorizationIdentifier>> CreateAdmin(Model.Admin admin)
        {
            if (_context.Admins.Any(u => u.Login == admin.Login))
            {
                return Helper.ConvertToServiceResponse<AuthorizationIdentifier>(null, _errorLoginIsNotUnique, false);
            }

            var salt = Helper.CreateSalt(20);
            var hashPassword = Helper.GenerateHash(admin.Password, salt);
            var userId = Guid.NewGuid().ToString();
            
            _context.Admins.Add(new Model.Admin()
            {
                Id = userId,
                Fio = admin.Fio,
                Password = hashPassword,
                Salt = salt,
                Login = admin.Login,
                Active = true
            });
            await _context.SaveChangesAsync();

            return new ServiseResponse<AuthorizationIdentifier>();
        }


        public async Task<ServiseResponse<AuthorizationIdentifier>> Login(string login, string password)
        {
            var admin = await _context.Admins.FirstOrDefaultAsync(a => a.Login == login);
            
            if (admin != null && Helper.AreEqual(password, admin.Password, admin.Salt))
            {
                var authInfo = new AuthorizationIdentifier
                {
                    Id = Guid.Parse(admin.Id),
                    Login = admin.Login
                };
                return Helper.ConvertToServiceResponse(authInfo);
            }

            return Helper.ConvertToServiceResponse<AuthorizationIdentifier>(null, _errorAuthorization);
        }
        
        public async Task<ServiseResponse<string>> RemoveUser(string id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);

            if (user != null)
            {
                user.Active = false;
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
            }

            return Helper.ConvertToServiceResponse("");
        }

        public async Task<ServiseResponse<string>> RemoveInstitution(string id)
        {
            var inst = await _context.Institutions.FirstOrDefaultAsync(x => x.Id == id);

            if (inst != null)
            {
                inst.Active = false;
                _context.Institutions.Update(inst);
                await _context.SaveChangesAsync();
            }

            return Helper.ConvertToServiceResponse("");
        }

        public async Task<ServiseResponse<List<User>>> GetUsers()
        {
            var result = await _context.Users.ToListAsync();

            return Helper.ConvertToServiceResponse(result);
        }

        public async Task<ServiseResponse<List<Institution>>> GetInstitutions()
        {
            var result = await _context.Institutions.ToListAsync();

            return Helper.ConvertToServiceResponse(result);
        }
    }
}