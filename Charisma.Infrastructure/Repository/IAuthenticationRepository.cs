using Charisma.Domain.Dtos;
using Charisma.Domain.Entities;
using Charisma.Infrastructure.Core.Exceptions;
using Charisma.Infrastructure.Core.Extentions;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Charisma.Infrastructure.Repository
{
    public interface IAuthenticationRepository
    {
        Task<SingInDto.Response> SingIn(SingInDto.Request dto, CancellationToken cancellationToken);
        Task<SingUpDto.Response> SingUp(SingUpDto.Request dto, CancellationToken cancellationToken);
    }
    public class AuthenticationRepository : IAuthenticationRepository
    {
        private readonly DbContextService dbContext;

        public AuthenticationRepository(DbContextService dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<SingInDto.Response> SingIn(SingInDto.Request dto, CancellationToken cancellationToken)
        {
            var users = await dbContext.Customer
                .Where(x => x.Email.ToLower() == dto.Username.ToLower() && x.Password == dto.Password.GetHash("!@#Charisma#@!"))
                .ToListAsync(cancellationToken);

            if (!users.Any())
                throw new ApiResponseExeption(404, "User not found");

            var user = users[0];

            var token = await CreateJWTToken(user.Id, 30, cancellationToken);

            return new SingInDto.Response { Token = token };
        }

        public async Task<SingUpDto.Response> SingUp(SingUpDto.Request dto, CancellationToken cancellationToken)
        {
            var user = await dbContext.Customer.AddAsync(new Customer
            {
                FullName = dto.FullName,
                Email = dto.Email,
                Password = dto.Password.GetHash("!@#Charisma#@!"),
            }, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            var token = await CreateJWTToken(user.Entity.Id, 30, cancellationToken);

            return new SingUpDto.Response { Token = token };
        }

        private async Task<string> CreateJWTToken(int userId, int expireDay, CancellationToken cancellationToken)
        {
            var symmetricKey = Encoding.UTF8.GetBytes("!@#Charisma#@!");
            var tokenHandler = new JwtSecurityTokenHandler();

            var claims = new List<Claim>(new[]
            {
                    new Claim("Identity", userId.ToString()),
            });

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = "Charisma_Audience",

                Issuer = "Charisma_Issuer",

                Subject = new ClaimsIdentity(claims),

                Expires = DateTime.UtcNow.AddDays(expireDay),

                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(symmetricKey),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(securityToken);

            return token;
        }
    }
}
