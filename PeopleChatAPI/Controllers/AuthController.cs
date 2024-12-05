using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Microsoft.IdentityModel.Tokens;
using PeopleChatAPI.Models;
using Microsoft.CodeAnalysis.Elfie.Model.Strings;

namespace PeopleChatAPI.Controllers
{
    [Route("api/Auth")]
    [ApiController]
    public class AuthController(PeopleChatContext context) : ControllerBase
    {
        private readonly PeopleChatContext _context = context;

        [HttpPost("Register")]
        public async Task<IResult> CreateNewUser(string userLogin, Byte[] userPassword)
        {
            Auth auth = new Auth();
            try
            {
                auth.UserLogin = userLogin;
                auth.UserPassword = userPassword;
                auth.RoleId = _context.Roles.FirstOrDefaultAsync(x => x.RoleName == "user")!.Id;
                await _context.Auths.AddAsync(auth);
                await _context.SaveChangesAsync();
            }
            catch {
                return Results.Unauthorized();
            }

            try
            {
                User user = new User();
                user.UserFirstname = "";
                user.UserLastname = "";
                user.AuthId = auth.Id;
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();

                string encodedJwt = GetJWT(userLogin);

                var response = new
                {
                    access_token = encodedJwt,
                    user_data = user
                };

                return Results.Json(response);
            }
            catch {
                return Results.Unauthorized();
            }
        }

        [HttpPost("Login")]
        public async Task<IResult> PostAuth(string userLogin, Byte[] userPassword)
        {
            Auth? newAuth = new Auth();
            User? user = new User();
            try
            {
                newAuth = await _context.Auths.FirstOrDefaultAsync(x => x.UserLogin == userLogin && x.UserPassword == userPassword);
                user = await _context.Users.FirstOrDefaultAsync(x => x.AuthId == newAuth!.Id);

                if (user == null) return Results.Unauthorized();

                string encodedJwt = GetJWT(userLogin);

                var response = new
                {
                    access_token = encodedJwt,
                    user_data = user
                };

                return Results.Json(response);
            }
            catch
            {
                return Results.Unauthorized();
            }
        }

        private string GetJWT(string UserLogin)
        {
            var claims = new List<Claim> { new Claim(ClaimTypes.Name, UserLogin) };

            var jwt = new JwtSecurityToken
            (
                issuer: AuthOptions.ISSUER,
                audience: AuthOptions.AUDIENCE,
                claims: claims,
                expires: DateTime.UtcNow.Add(TimeSpan.FromDays(30)),
                signingCredentials: new SigningCredentials
                (
                    AuthOptions.GetSymmetricSecurityKey(),
                    SecurityAlgorithms.HmacSha256
                )
            );

            string encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return encodedJwt;
        }
    }
}
