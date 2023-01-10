namespace Logic.Models;

public class RegisterRequestModel
{
    public string FIO { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string PasswordConfirm { get; set; }
}