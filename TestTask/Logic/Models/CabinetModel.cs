namespace Logic.Models;

/// <summary>
/// Model for get user info
/// </summary>
public class CabinetModel
{
    public string FIO { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public DateTime LastLogin { get; set; }
}