using PeopleChatAPI.Models;

namespace PeopleChatAPI.Dto
{
    public record UserDto
    {
        public int Id { get; set; }

        public string UserFirstname { get; set; } = null!;

        public string UserLastname { get; set; } = null!;

        public byte[]? Image { get; set; }

        public DateOnly? BirthDate { get; set; }

        public virtual string Gender { get; set; } = "";

        public UserDto() { }

        public UserDto(User user)
        {
            Id = user.Id;
            UserFirstname = user.UserFirstname;
            UserLastname = user.UserLastname;
            Image = user.Image;
            BirthDate = user.BirthDate;
            Gender = user.Gender != null ? user.Gender.GenderName : "";
        }
    }
}
