using System.ComponentModel.DataAnnotations;

namespace Logic.Models;

/// <summary>
/// Model
/// </summary>
public class LoginRequestModel
{
    [StringLength(11, ErrorMessage = "Слишком длинный номер телефона")]
    [Attributes.Phone(ErrorMessage = "Номер телефона некорректен")]
    public string Phone { get; set; }
    [StringLength(20, ErrorMessage = "Слишком длинный пароль")]
    public string Password { get; set; }
}