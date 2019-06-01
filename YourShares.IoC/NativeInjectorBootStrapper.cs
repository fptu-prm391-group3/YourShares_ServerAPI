using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using YourShares.Application;
using YourShares.Application.Interfaces;
using YourShares.Application.Services;
using YourShares.Data;
using YourShares.Data.Interfaces;
using YourShares.Data.Repository;
using YourShares.Data.UoW;
using YourShares.Domain.Models;
using YourShares.Identity.Authorization;
using YourShares.Identity.Models;
using YourShares.Identity.Services;

namespace YourShares.IoC
{
    public static class NativeInjectorBootStrapper
    {
        public static void RegisterServices(IServiceCollection services)
        {
            // ASP.NET HttpContext dependency
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // ASP.NET Authorization Polices
            services.AddSingleton<IAuthorizationHandler, ClaimsRequirementHandler>();

            // Application
            services.AddTransient<ICompanyService, CompanyService>();
            
            // Data- Repo
            services.AddScoped<IRepository<Company>, Repository<Company>>();
            services.AddScoped<IRepository<Administrator>, Repository<Administrator>>();
            services.AddScoped<IRepository<ShareAccounting>, Repository<ShareAccounting>>();
            services.AddScoped<IRepository<Shareholder>, Repository<Shareholder>>();
            services.AddScoped<IRepository<Transaction>, Repository<Transaction>>();

            // Infra - Data
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<YourSharesContext>();

            // Infra - Identity Services
            services.AddTransient<IEmailSender, AuthEmailMessageSender>();
            services.AddTransient<ISmsSender, AuthSmsMessageSender>();

            // Infra - Identity
            services.AddScoped<IUser, AspNetUser>();
        }
    }
}