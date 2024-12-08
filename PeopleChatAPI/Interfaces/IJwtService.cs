using PeopleChatAPI.Dto;

namespace PeopleChatAPI.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(AuthDto? user);
    }
}
