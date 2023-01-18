using System.ComponentModel.DataAnnotations;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;

namespace Logic.Attributes;

/// <summary>
/// Check pgone on valid (starts with 7 and consists of digits)
/// </summary>
public class PhoneAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        var r = new Regex(@"^7\d");
        if (value == null || string.IsNullOrEmpty(value.ToString()))
            return false;
        return r.Match((string)value).Success;
    }
}