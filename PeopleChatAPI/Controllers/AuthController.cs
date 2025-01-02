using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeopleChatAPI.Dto;
using Microsoft.AspNetCore.Authorization;
using PeopleChatAPI.Interfaces;
using PeopleChatAPI.Models.PeopleChat;
using PeopleChatAPI.Services;

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

            UserDto userData = authDto.UserData!;
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
                Gender? gender = await _context.Genders.FirstOrDefaultAsync(x => x.GenderName == userData.Gender);

                User user = new()
                {
                    UserFirstname = userData.UserFirstname,
                    UserLastname = userData.UserLastname,
                    BirthDate = userData.BirthDate,
                    AuthId = auth.Id,
                    GenderId = gender!.Id
                };
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();

                userData.Id = user.Id;
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
