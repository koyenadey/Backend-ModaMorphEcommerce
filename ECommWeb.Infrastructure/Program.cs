using Npgsql;
using ECommWeb.Business.src.ServiceAbstract.AuthServiceAbstract;
using ECommWeb.Business.src.ServiceAbstract.EntityServiceAbstract;
using ECommWeb.Business.src.ServiceImplement.EntityServiceImplement;
using ECommWeb.Core.src.RepoAbstract;
using ECommWeb.Core.src.ValueObject;
using ECommWeb.Infrastructure.src.Database;
using ECommWeb.Infrastructure.src.Middleware;
using ECommWeb.Infrastructure.src.Repo;
using ECommWeb.Infrastructure.src.Service;
using Microsoft.EntityFrameworkCore;
using Server.Service.src.ServiceImplement.AuthServiceImplement;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ECommWeb.Infrastructure.src;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var dataSourceBuilder = new NpgsqlDataSourceBuilder(builder.Configuration.GetConnectionString("DbConn"));
dataSourceBuilder.MapEnum<Role>();
dataSourceBuilder.MapEnum<Status>();

var dataSource = dataSourceBuilder.Build();

// adding db context into your ap
builder.Services.AddDbContext<AppDbContext>
(
    options =>
    options.UseNpgsql(dataSource)
    .UseSnakeCaseNamingConvention()
);

// service registration -> automatically create all instances of dependencies
builder.Services.AddScoped<IUserRepo, UserRepo>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAddressRepo, AddressRepo>();
builder.Services.AddScoped<IAddressService, AddressService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICategoryRepo, CategoryRepo>();

//builder.Services.AddScoped<IProductImageService, ProductImageService>();
//builder.Services.AddScoped<IProductImageRepo, ProductImageRepo>();

builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductRepo, ProductRepo>();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IPasswordService, PasswordService>();
builder.Services.AddScoped<ExceptionHanlerMiddleware>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(
    options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Secrets:JwtKey"])),
            ValidateIssuer = true,
            ValidateAudience = false,
            ValidateLifetime = true, //make sure it does not expire
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Secrets:Issuer"]
        };
    }
);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ExceptionHanlerMiddleware>();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();

