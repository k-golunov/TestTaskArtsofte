using Dal.Entities;

namespace Dal.Interfaces;

public interface IUserRepository
{
    void Add(User user);
}