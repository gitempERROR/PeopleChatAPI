namespace PeopleChatAPI.Dto
{
    public record AuthDto(Byte[] UserPassword, string UserLogin, UserDto? UserData = null);
}
