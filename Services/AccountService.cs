using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System;
using System.Threading.Tasks;
using Hero_Project.Data;
using Hero_Project.Entities;
using Hero_Project.NetCore5.Interfaces;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using static Hero_Project.NetCore5.Installers_Libraries.JWTInstaller;
using Microsoft.IdentityModel.Tokens;

namespace Hero_Project.NetCore5.Services
{
    public class AccountService : IAccountService
    {
        private readonly DatabaseContext databaseContext;
        private readonly JWTSettings jwtSettings;

        public AccountService(DatabaseContext databaseContext, JWTSettings jwtSettings)
        {
            this.jwtSettings = jwtSettings;
            this.databaseContext = databaseContext;

        }

        public string GenerateToken(Account account)
        {
            // Key is case sensitive 
            var claims = new[]{
                new Claim(JwtRegisteredClaimNames.Sub, account.Username),
                new Claim("role", account.Role.Name),
                new Claim("additional", "todo"),
                new Claim("todo day", "11/22/33")
            };
            return BuildToken(claims);
        }

        public string BuildToken(Claim[] claims)
        {
            var expires = DateTime.Now.AddDays(Convert.ToDouble(jwtSettings.Expire));
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            //Generate Token
            var token = new JwtSecurityToken(
                issuer : jwtSettings.Issuer,
                audience: jwtSettings.Audience,
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public Account GetInfo(string accessToken)
        {
            var token = new JwtSecurityTokenHandler().ReadToken(accessToken) as JwtSecurityToken;
            var username = token.Claims.First(claim => claim.Type == "sub").Value;
            var role = token.Claims.First(claim => claim.Type == "role").Value;
            var account = new Account
            {
                Username = username,
                Role = new Role
                {
                    Name = role
                }
            };
            return account;
        }

        public async Task<Account> Login(string username, string password)
        {
            var account = await databaseContext.Accounts.Include(a => a.Role)
                                .SingleOrDefaultAsync(a => a.Username == username);
            if (account != null && VerifyPassword(account.Password, password))
            {
                return account;
            }
            return null;
        }

        public async Task Register(Account account)
        {
            //check existingAccount from user
            var existingAccount = await databaseContext.Accounts.SingleOrDefaultAsync(a => a.Username == account.Username);
            if (existingAccount != null)
            {
                throw new Exception("Existing Account");
            }
            account.Password = CreatePasswordHash(account.Password);
            databaseContext.Accounts.Add(account);
            await databaseContext.SaveChangesAsync();
        }

        private string CreatePasswordHash(string password)
        {
            //salt is ref data from random for hash password
            byte[] salt = new byte[128 / 8];
            //random ref data of salt
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            //hash base64 password by map
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA512,
                iterationCount: 10000,
                numBytesRequested: 258 / 8
            ));
            //convert salt and hashed to base64
            return $"{Convert.ToBase64String(salt)}.{hashed}";
        }

        //verify password
        private bool VerifyPassword(string hash, string password)
        {
            //split hashed and password
            var parts = hash.Split('.', 2);
            if (parts.Length != 2)
            {
                return false;
            }
            //hash password from database
            var salt = Convert.FromBase64String(parts[0]);
            var passwordHash = parts[1];
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
               password: password,
               salt: salt,
               prf: KeyDerivationPrf.HMACSHA512,
               iterationCount: 10000,
               numBytesRequested: 258 / 8
           ));
            //return password from user is equl hashed from database 
            return passwordHash == hashed;
        }

    }
}