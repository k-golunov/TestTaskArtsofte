namespace Logic.Models;

/// <summary>
/// Model for get error
/// </summary>
public class ErrorResponseModel
{
    public string Code { get; set; }
    public string Message { get; set; }

    public ErrorResponseModel(string code, string message)
    {
        Code = code;
        Message = message;
    }
}