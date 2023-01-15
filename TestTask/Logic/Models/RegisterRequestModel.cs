using System.ComponentModel.DataAnnotations;

namespace Logic.Models;

public class RegisterRequestModel
{
    [StringLength(250, ErrorMessage = "Слишком длинное ФИО")]
    public string FIO { get; set; }
    [StringLength(11, ErrorMessage = "Слишком длинный номер телефона")]
    // [Phone]
    public string Phone { get; set; }
    // [EmailAddress(ErrorMessage = "Почта некорректна")]
    [StringLength(150)]
    [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Некорректный адрес")]
    public string Email { get; set; }
    [StringLength(20, ErrorMessage = "Слишком длинный пароль")]
    public string Password { get; set; }
    [StringLength(20, ErrorMessage = "Слишком длинный пароль")]
    [Compare("Password", ErrorMessage = "Пароли не совпадают")]
    public string PasswordConfirm { get; set; }
}