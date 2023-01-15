using System.ComponentModel.DataAnnotations;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;

namespace Logic.Attributes;

public class PhoneAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        var r = new Regex(@"^7\d");
        if (string.IsNullOrEmpty(value.ToString()))
            return false;
        return r.Match((string)value).Success;
    }
}