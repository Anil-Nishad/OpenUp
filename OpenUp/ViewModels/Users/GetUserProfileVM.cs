using OpenUpData.Models;

namespace OpenUp.ViewModels.Users;

public class GetUserProfileVM
{
    public User User { get; set; }
    public List<Post> Posts { get; set; }
}
