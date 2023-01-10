using Dal.Interfaces;
using Dal.Repositories;
using Logic.Interfaces;
using Logic.Managers;
using Logic.Profiles;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<IAccountManager, AccountManager>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddAutoMapper(typeof(UserProfile));

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();