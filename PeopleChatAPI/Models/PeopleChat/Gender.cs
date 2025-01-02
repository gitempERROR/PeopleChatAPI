using System;
using System.Collections.Generic;

namespace PeopleChatAPI.Models.PeopleChat;

public partial class Gender
{
    public int Id { get; set; }

    public string GenderName { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
