using System.ComponentModel.DataAnnotations;
using Logic.Attributes;

namespace Logic.Models;

public class RegisterRequestModel
{
    [StringLength(250, ErrorMessage = "Слишком длинное ФИО")]
    public string FIO { get; set; }
    [StringLength(11, ErrorMessage = "Слишком длинный номер телефона")]
    [Attributes.Phone(ErrorMessage = "Номер телефона некорректен")]
    public string Phone { get; set; }
    [EmailAddress(ErrorMessage = "Почта некорректна")]
    [StringLength(150)]
    public string Email { get; set; }
    [StringLength(20, ErrorMessage = "Слишком длинный пароль")]
    public string Password { get; set; }
    [StringLength(20, ErrorMessage = "Слишком длинный пароль")]
    [Compare("Password", ErrorMessage = "Пароли не совпадают")]
    public string PasswordConfirm { get; set; }
}