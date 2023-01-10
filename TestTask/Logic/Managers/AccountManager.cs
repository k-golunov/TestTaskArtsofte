using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Dal.Entities;
using Dal.Interfaces;
using Logic.Interfaces;
using Logic.Models;
using Microsoft.IdentityModel.Tokens;

namespace Logic.Managers;

public class AccountManager : IAccountManager
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;
    public AccountManager(IUserRepository userRepository, IConfiguration configuration, IMapper mapper)
    {
        _userRepository = userRepository;
        _configuration = configuration;
        _mapper = mapper;
    }
    
    public AuthenticateResponse Authenticate(LoginRequestModel model)
    {
        var user = _userRepository
            .GetAll()
            .FirstOrDefault(x => x.Phone == model.Phone && x.Password == model.Password);

        if (user == null)
            return null;
            

        var token = GenerateJwtToken(user);
        return new AuthenticateResponse(user, token);
    }

    /// <summary>
    /// Register user and create AuthenticateResponse with method Authenticate
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task<AuthenticateResponse> Register(RegisterRequestModel model)
    {
        var userModel = _mapper.Map<User>(model);
        var addedUser = await _userRepository.AddAsync(userModel);
            
        var response = Authenticate(new LoginRequestModel
        {
            Phone = userModel.Phone,
            Password = userModel.Password
        });
            
        return response;
    }

    public User GetById(int userId)
    {
        return _userRepository.GetById(userId);
    }
    
    private string GenerateJwtToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["Secret"]);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] { new Claim("UserId", user.Id.ToString()) }),
            Expires = DateTime.UtcNow.AddDays(30),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}