using Dal.Entities;
using Logic.Models;

namespace Logic.Interfaces;

public interface IAccountManager
{
    Task<AuthenticateResponse?> Register(RegisterRequestModel model);
    User GetById(int userId);
    public AuthenticateResponse Authenticate(LoginRequestModel model);
    public User GetByPhone(string phone);
    public CabinetModel GetInfo(int userId);
}