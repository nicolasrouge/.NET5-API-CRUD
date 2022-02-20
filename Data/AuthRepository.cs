using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using netwebapi.Models;

namespace netwebapi.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;

        public AuthRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<ServiceResponse<string>> Login(string username, string password)
        {
            var response = new ServiceResponse<string>();
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Username.ToLower().Equals(username.ToLower()));
            if(user == null){
                response.Success = false;
                response.Message = "User not found.";
            }else if(!VerifyPassword(password, user.PasswordHash, user.PasswordSalt)){
                response.Success = false;
                response.Message = "Incorrect Password";
            }else{
                response.Success = true;
                response.Message = "Correct!";
                response.Data = user.Id.ToString();
            }
            return response;
        }

        public async Task<ServiceResponse<int>> Register(User user, string password)
        {
            ServiceResponse<int> respnse = new ServiceResponse<int>();

            if(await UserExists(user.Username)){
                respnse.Success = false;
                respnse.Message = "user already exists";
                return respnse;
            }

            CreatePasswordHash(password, out byte[] passwordHash, out byte [] passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            respnse.Data = user.Id;
            respnse.Message = "Success";
            return respnse;
        }

        public async Task<bool> UserExists(string username){
            if( await _context.Users.AnyAsync(x => x.Username.ToLower().Equals(username.ToLower()))){
                return true;
            }return false;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using(var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPassword(string password, byte[] passwordHash, byte[] passwordSalt){
            bool result = true;
            using(var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for(int i = 0 ; i < computedHash.Length; i++)
                {
                    if(computedHash[i] != passwordHash[i])
                    {
                        return false;
                    }
                }
                return result;
            }
        }
    }
}