using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using IntaxExterno.Application.Mappings;
using IntaxExterno.Application.Interfaces;
using IntaxExterno.Application.Services;
using IntaxExterno.Domain.Interfaces;
using IntaxExterno.Domain.Enums;
using IntaxExterno.Infra.Data.Context;
using IntaxExterno.Infra.Data.Identity;
using IntaxExterno.Infra.Data.Repositories;
using IntaxExterno.Infra.Data.Helpers;
using System.Text;

namespace IntaxExterno.Infra.IoC;

public static class DependecyInjection
{
    public static IServiceCollection AddInsfrastructure(this IServiceCollection services,
        IConfiguration configuration)
    {
        #region Configurações da database
        services.AddDbContext<ApplicationDbContext>(options =>
           options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"),
               b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
        #endregion

        #region Identity
        services.AddIdentity<User, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        services.Configure<IdentityOptions>(options =>
        {
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.AllowedForNewUsers = true;
            options.SignIn.RequireConfirmedEmail = false;
        });
        #endregion

        #region Autorization
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer = configuration["jwt:issuer"],
                ValidAudience = configuration["jwt:audience"],
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(configuration["jwt:secretKey"]!)),
                ClockSkew = TimeSpan.Zero,
                RoleClaimType = "Roles"
            };
        });
        #endregion

        #region Policies
        services.AddAuthorization(options =>
        {
            options.AddPolicy(RoleType.Admin.ToString(), policy => policy.RequireRole(RoleType.Admin.ToString()));
            options.AddPolicy(RoleType.Cliente.ToString(), policy => policy.RequireRole(RoleType.Cliente.ToString()));
        });
        #endregion

        #region Seed
        services.AddScoped<ISeedUserAndRoleInitial, SeedUserAndRoleInitial>();
        #endregion

        #region AutoMapper

        services.AddAutoMapper(x => x.AddProfile(typeof(DomainToDtoMappingProfile)));

        #endregion AutoMapper 

        #region Repositories
        services.AddScoped<IParceiroRepository, ParceiroRepository>();
        services.AddScoped<IClienteRepository, ClienteRepository>();
        services.AddScoped<IPropostaRepository, PropostaRepository>();
        services.AddScoped<IPropostaTesesRepository, PropostaTesesRepository>();
        services.AddScoped<ITesesRepository, TesesRepository>();
        services.AddScoped<IRelatorioDeCreditoPerseRepository, RelatorioDeCreditoPerseRepository>();
        services.AddScoped<IItemRelatorioDeCreditoPerseRepository, ItemRelatorioDeCreditoPerseRepository>();
        services.AddScoped<IExclusaoIcmsResultadoRepository, ExclusaoIcmsResultadoRepository>();
        services.AddScoped<IOportunidadeRepository, OportunidadeRepository>();
        services.AddScoped<ISpedContribuicoesRepository, SpedContribuicoesRepository>();
        services.AddScoped<ISpedFiscalRepository, SpedFiscalRepository>();
        #endregion

        #region Services
        services.AddScoped<IParceiroService, ParceiroService>();
        services.AddScoped<IClienteService, ClienteService>();
        services.AddScoped<IPropostaService, PropostaService>();
        services.AddScoped<ITesesService, TesesService>();
        services.AddScoped<IRelatorioDeCreditoPerseService, RelatorioDeCreditoPerseService>();
        services.AddScoped<IExclusaoIcmsService, ExclusaoIcmsService>();
        services.AddScoped<IOportunidadeService, OportunidadeService>();
        services.AddScoped<ISpedContribuicoesService, SpedContribuicoesService>();
        services.AddScoped<ISpedFiscalService, SpedFiscalService>();
        services.AddScoped<ISpedParserService, SpedParserService>();
        #endregion

        #region Configuration
        services.AddHttpContextAccessor();
        #endregion

        #region CORS
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAnyOrigin",
                builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyHeader()
                           .AllowAnyMethod();
                });
        });
        #endregion

        return services;
    }
}
