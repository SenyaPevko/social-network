using Api.Controllers.UserProfiles.Requests;
using Api.Controllers.UserProfiles.Responses;
using Api.Controllers.Users.Requests;
using Api.Controllers.Users.Responses;
using Api.Listeners.RabbitMq;
using Core.Logic.Connections.RabbitMqLogic;
using Core.Logic.Logs;
using Core.Logic.Tracing.TraceIdLogic.TraceIdAccessors;
using Dal;
using Dal.FriendRequests;
using Dal.Friendships;
using Dal.RefreshTokens;
using Dal.Rights;
using Dal.Roles;
using Dal.Sessions;
using Dal.UserProfiles;
using Dal.Users;
using IdentityConnectionLib;
using Logic.UserProfiles.Managers;
using Logic.UserProfiles.Models;
using Logic.Users.Managers;
using Logic.Users.Models;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());
    options.ReturnHttpNotAcceptable = true;
    options.RespectBrowserAcceptHeader = true;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// adding dbcontext
string connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<IdentityServiceContext>(options => options.UseNpgsql(connection));

// adding dal refs
builder.Services.AddScoped<IFriendRequestRepository, FriendRequestRepository>();
builder.Services.AddScoped<IFriendshipRepository, FriendshipRepository>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
builder.Services.AddScoped<IRightRepository, RightRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<ISessionRepository, SessionRepository>();
builder.Services.AddScoped<IUserProfileRepository, UserProfileRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

// adding RabbitMq services
builder.Services.ConfigureConnections();
builder.Services.AddRabbitMqServices();
builder.Services.AddHostedService<UserProfileRabittMqListener>();
builder.Services.AddHostedService<UserRabbitMqListener>();

// adding logic refs
builder.Services.AddScoped<IUserProfileLogicManager, UserProfileLogicManager>();
builder.Services.AddScoped<IUserLogicManager, UserLogicManager>();

// adding logger
builder.Services.AddLoggerServices();
builder.Services.AddTraceId();
Log.Logger = new LoggerConfiguration().GetConfiguration().CreateLogger();
builder.Host.UseSerilog();
builder.Services.AddLogging(loggingBuilder =>
            loggingBuilder.AddSerilog(dispose: true));


// adding mappers
builder.Services.AddAutoMapper(cfg =>
{
    cfg.CreateMap<CreateUserRequest, UserLogic>();

    cfg.CreateMap<CreateUserRequest, UserLogic>();
}, Array.Empty<System.Reflection.Assembly>());

builder.Services.AddAutoMapper(cfg =>
{
    cfg.CreateMap<UserLogic, GetUserInfoResponse>();

    cfg.CreateMap<UserLogic, GetUserInfoResponse>();
}, Array.Empty<System.Reflection.Assembly>());

builder.Services.AddAutoMapper(cfg =>
{
    cfg.CreateMap<UpdateUserRequest, UserLogic>();

    cfg.CreateMap<UpdateUserRequest, UserLogic>();
}, Array.Empty<System.Reflection.Assembly>());

builder.Services.AddAutoMapper(cfg =>
{
    cfg.CreateMap<UserLogic, UpdateUserResponse>();

    cfg.CreateMap<UserLogic, UpdateUserResponse>();
}, Array.Empty<System.Reflection.Assembly>());

builder.Services.AddAutoMapper(cfg =>
{
    cfg.CreateMap<CreateUserProfileRequest, UserProfileLogic>();

    cfg.CreateMap<CreateUserProfileRequest, UserProfileLogic>();
}, Array.Empty<System.Reflection.Assembly>());

builder.Services.AddAutoMapper(cfg =>
{
    cfg.CreateMap<UserProfileLogic, GetUserProfileInfoResponse>();

    cfg.CreateMap<UserProfileLogic, GetUserProfileInfoResponse>();
}, Array.Empty<System.Reflection.Assembly>());


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseSerilogRequestLogging();

app.MapControllers();

app.Run();
