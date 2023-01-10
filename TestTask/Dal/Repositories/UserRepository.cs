using Dal.Entities;
using Dal.Interfaces;

namespace Dal.Repositories;

public class UserRepository : IUserRepository
{
    private readonly DataContext _context;

    public UserRepository(DataContext context)
    {
        _context = context;
    }
    public void Add(User user)
    {
        throw new NotImplementedException();
    }

    public User GetById(int id) => _context.Users.FirstOrDefault(u => u.Id == id);
    
    public List<User> GetAll() => _context.Set<User>().ToList();
    
    public async Task<int> AddAsync(User user)
    {
        var result = _context.Set<User>().Add(user);
        await _context.SaveChangesAsync();
        return result.Entity.Id;
    }
}