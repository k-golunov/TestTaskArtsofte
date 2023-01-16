using Dal.Entities;

namespace Dal.Interfaces;

public interface IUserRepository
{
    void Add(User user);
    User? GetById(int id);
    List<User> GetAll();
    Task<int> AddAsync(User user);
    Task<int> UpdateAsync(User user);
    User? GetByPhone(string phone);
    User? GetByEmail(string phone);
}