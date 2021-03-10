using System.Security.Cryptography;
using System;
using System.Threading.Tasks;
using Hero_Project.Data;
using Hero_Project.Entities;
using Hero_Project.NetCore5.Interfaces;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;

namespace Hero_Project.NetCore5.Services
{
    public class AccountService : IAccountService
    {
        private readonly DatabaseContext databaseContext;
        public AccountService(DatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }

        public  async Task<Account> Login(string username, string password)
        {
            var account = await databaseContext.Accounts.Include(a => a.Role)
                                .SingleOrDefaultAsync(a => a.Username == username);
            if (account != null && VerifyPassword(account.Password, password)) {
                return account;
            }
            return null;
        }

        public async Task Register(Account account)
        {
            //check existingAccount from user
            var existingAccount = await databaseContext.Accounts.SingleOrDefaultAsync(a => a.Username == account.Username);
            if (existingAccount != null) {
                throw new Exception("Existing Account");
            }
            account.Password = CreatePasswordHash(account.Password);
            databaseContext.Accounts.Add(account);
            await databaseContext.SaveChangesAsync();
        }

        private string CreatePasswordHash(string password){
            //salt is ref data from random for hash password
            byte[] salt = new byte[128/8];
            //random ref data of salt
            using (var rng = RandomNumberGenerator.Create()){
                rng.GetBytes(salt);
            }
            //hash base64 password by map
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password : password,
                salt : salt,
                prf : KeyDerivationPrf.HMACSHA512,
                iterationCount: 10000,
                numBytesRequested : 258 / 8
            ));
            //convert salt and hashed to base64
            return $"{Convert.ToBase64String(salt)}.{hashed}";
        }

        //verify password
        private bool VerifyPassword(string hash, string password){
            //split hashed and password
            var parts = hash.Split('.', 2 );
            if (parts.Length != 2) {
                return false;
            }
            //hash password from database
            var salt = Convert.FromBase64String(parts[0]);
            var passwordHash = parts[1];
             string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password : password,
                salt : salt,
                prf : KeyDerivationPrf.HMACSHA512,
                iterationCount: 10000,
                numBytesRequested : 258 / 8
            ));
            //return password from user is equl hashed from database 
            return passwordHash == hashed;
        }
    }
}