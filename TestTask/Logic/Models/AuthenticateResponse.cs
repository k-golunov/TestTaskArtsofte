using Dal.Entities;

namespace Logic.Models;

public class AuthenticateResponse
{
    public int Id { get; set; }
    // [Email]
    public string Email { get; set; }
    public string AccessToken { get; set; }
    public AuthenticateResponse(User user, string token)
    {
        Id = user.Id;
        Email = user.Email;
        AccessToken = token;
    }
}