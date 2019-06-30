using Microsoft.Extensions.DependencyInjection;
using YourShares.Application.Interfaces;
using YourShares.Application.Services;
using YourShares.Data;
using YourShares.Data.Interfaces;
using YourShares.Data.Repository;
using YourShares.Data.UoW;
using YourShares.Domain.Models;

namespace YourShares.IoC
{
    public static class NativeInjectorBootstrapper
    {
        public static void RegisterServices(IServiceCollection services)
        {
            // ASP.NET HttpContext dependency
            // services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // ASP.NET Authorization Polices
            // services.AddSingleton<IAuthorizationHandler, ClaimsRequirementHandler>();
            

            // Application
            services.AddTransient<ICompanyService, CompanyService>();
            services.AddTransient<IUserAccountService, UserAccountService>();
            services.AddTransient<IUserGoogleAccountService, GoogleAccountService>();
            services.AddTransient<IUserProfileService, UserProfileService>();
            services.AddTransient<IShareholderService, ShareholderService>();
            services.AddTransient<ISharesAccountService, SharesAccountService>();
            services.AddTransient<IRoundService, RoundService>();
            services.AddTransient<IRoundInvestorService, RoundInvestorService>();
            services.AddTransient<IRetrictedSharesService, RetrictedSharesService>();
            
            // Data- Repo
            services.AddScoped<IRepository<RefShareholderTypeCode>, Repository<RefShareholderTypeCode>>();
            services.AddScoped<IRepository<RefShareTypeCode>, Repository<RefShareTypeCode>>();
            services.AddScoped<IRepository<RefTransactionStatusCode>, Repository<RefTransactionStatusCode>>();
            services.AddScoped<IRepository<RefUserAccountStatusCode>, Repository<RefUserAccountStatusCode>>();
            services.AddScoped<IRepository<RefTransactionTypeCode>, Repository<RefTransactionTypeCode>>();
            services.AddScoped<IRepository<UserProfile>, Repository<UserProfile>>();
            services.AddScoped<IRepository<UserAccount>, Repository<UserAccount>>();
            services.AddScoped<IRepository<GoogleAccount>, Repository<GoogleAccount>>();
            services.AddScoped<IRepository<FacebookAccount>, Repository<FacebookAccount>>();
            services.AddScoped<IRepository<RestrictedShare>, Repository<RestrictedShare>>();
            services.AddScoped<IRepository<Company>, Repository<Company>>();
            services.AddScoped<IRepository<ShareAccount>, Repository<ShareAccount>>();
            services.AddScoped<IRepository<Shareholder>, Repository<Shareholder>>();
            services.AddScoped<IRepository<Transaction>, Repository<Transaction>>();
            services.AddScoped<IRepository<TransactionRequest>, Repository<TransactionRequest>>();
            services.AddScoped<IRepository<Round>, Repository<Round>>();
            services.AddScoped<IRepository<RoundInvestor>, Repository<RoundInvestor>>();
            

            // Infra - Data
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<YourSharesContext>();

            // Infra - Identity Services
            // services.AddTransient<IEmailSender, AuthEmailMessageSender>();
            // .AddTransient<ISmsSender, AuthSmsMessageSender>();

            // Infra - Identity
            // services.AddScoped<IUser, AspNetUser>();
        }
    }
}