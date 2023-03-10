using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using Dal.Entities;
using Dal.Interfaces;
using Logic.Interfaces;
using Logic.Models;
using Microsoft.AspNetCore.Identity;
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
    
    /// <summary>
    /// Authenticate user and update Last Login
    /// </summary>
    /// <param name="model">model for login user (Phone, Password)</param>
    /// <returns>AuthenticateResponse</returns>
    public AuthenticateResponse? Authenticate(LoginRequestModel model)
    {
        // hash password!
        model.Password = GetHashPassword(model.Password);
        var user = _userRepository
            .GetAll()
            .FirstOrDefault(x => x.Phone == model.Phone && x.Password == model.Password);

        if (user == null)
            return null;
        user.LastLogin = DateTime.UtcNow;
        _userRepository.UpdateAsync(user);          

        var token = GenerateJwtToken(user);
        return new AuthenticateResponse(user, token);
    }

    /// <summary>
    /// Register user and create AuthenticateResponse with method Authenticate
    /// </summary>
    /// <param name="model">model with data for register (FIO, Phone, Email, Password)</param>
    /// <returns>task AuthenticateResponse</returns>
    public async Task<AuthenticateResponse?> Register(RegisterRequestModel model)
    {
        if (!IsUseEmailOrPhone(model.Email, model.Phone))
            return null;
        // hash password!
        var userModel = _mapper.Map<User>(model);
        userModel.Password = GetHashPassword(model.Password);
        var addedUser = await _userRepository.AddAsync(userModel);
            
        var response = Authenticate(new LoginRequestModel
        {
            Phone = userModel.Phone,
            Password = model.Password
        });
            
        return response;
    }

    /// <summary>
    /// Get user by id
    /// </summary>
    /// <param name="userId">user id</param>
    /// <returns>User entity</returns>
    public User GetById(int userId) => _userRepository.GetById(userId);
    

    /// <summary>
    /// Generate new JWT token for authorize
    /// </summary>
    /// <param name="user">user entity</param>
    /// <returns>string JWT token</returns>
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

    public CabinetModel GetInfo(int userId)
    {
        var user = GetById(userId);
        var cabinet = _mapper.Map<CabinetModel>(user);
        return cabinet;
    }
    
    public User GetByPhone(string phone) => _userRepository.GetByPhone(phone);

    public bool IsUseEmailOrPhone(string email, string phone) => _userRepository.GetByPhone(phone) == null && _userRepository.GetByEmail(email) == null;

    private string GetHashPassword(string password)
    {
        var bytes = Encoding.UTF8.GetBytes(password);
        var hash = MD5.HashData(bytes);
        var result = new StringBuilder();
        foreach (var h in hash)
            result.Append(h.ToString("x2"));
        return result.ToString();
    }
}