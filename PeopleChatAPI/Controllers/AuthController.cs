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
using PeopleChatAPI.Dto;
using Microsoft.AspNetCore.Authorization;
using PeopleChatAPI.Interfaces;

namespace PeopleChatAPI.Controllers
{
    [Route("api/Auth")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController(PeopleChatContext context) : ControllerBase
    {
        private readonly PeopleChatContext _context = context;

        [HttpPost("Register")]
        public async Task<IResult> CreateNewUser([FromBody] AuthDto authDto, [FromServices] IJwtService jwtService)
        {
            string userLogin = authDto.UserLogin;
            byte[] userPassword = authDto.UserPassword;

            Auth auth = new();

            UserDto userData;
            try
            {
                auth.UserLogin = userLogin;
                auth.UserPassword = userPassword;

                Role? role = await _context.Roles.FirstOrDefaultAsync(x => x.RoleName == "user");

                auth.RoleId = role!.Id;

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

                userData = new UserDto(user);
                string accessToken = jwtService.GenerateToken(authDto);

                var response = new
                {
                    accessToken,
                    userData
                };

                return Results.Json(response);
            }
            catch {
                return Results.Unauthorized();
            }
        }

        [HttpPost("Login")]
        public async Task<IResult> PostAuth([FromBody] AuthDto authDto, [FromServices] IJwtService jwtService)
        {
            string userLogin = authDto.UserLogin;
            byte[] userPassword = authDto.UserPassword;
            Auth? newAuth = new Auth();
            User? user = new User();
            UserDto userData;
            try
            {
                newAuth = await _context.Auths.FirstOrDefaultAsync(x => x.UserLogin == userLogin && x.UserPassword == userPassword);
                user = await _context.Users.FirstOrDefaultAsync(x => x.AuthId == newAuth!.Id);

                if (user == null) return Results.Unauthorized();

                userData = new UserDto(user);
                string accessToken = jwtService.GenerateToken(authDto);

                var response = new
                {
                    accessToken,
                    userData
                };

                return Results.Json(response);
            }
            catch
            {
                return Results.Unauthorized();
            }
        }
    }
}
