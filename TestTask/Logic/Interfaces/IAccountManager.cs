using Logic.Models;

namespace Logic.Interfaces;

public interface IAccountManager
{
    void Register(RegisterRequestModel model);
}