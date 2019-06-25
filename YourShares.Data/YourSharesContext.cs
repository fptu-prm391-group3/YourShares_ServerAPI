using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using YourShares.Domain.Models;
using YourShares.RestApi.Models;

namespace YourShares.Data
{
    public class YourSharesContext : DbContext
    {
        private readonly IHostingEnvironment _env;

        public virtual DbSet<Company> Company { get; set; }
        public virtual DbSet<GoogleAccount> GoogleAccount { get; set; }
        public virtual DbSet<RefShareTypeCode> RefShareTypeCode { get; set; }
        public virtual DbSet<RefShareholderTypeCode> RefShareholderTypeCode { get; set; }
        public virtual DbSet<RefTransactionStatusCode> RefTransactionStatusCode { get; set; }
        public virtual DbSet<RefTransactionTypeCode> RefTransactionTypeCode { get; set; }
        public virtual DbSet<RefUserAccountStatusCode> RefUserAccountStatusCode { get; set; }
        public virtual DbSet<RestrictedShare> RestrictedShare { get; set; }
        public virtual DbSet<ShareAccount> ShareAccount { get; set; }
        public virtual DbSet<Shareholder> Shareholder { get; set; }
        public virtual DbSet<Transaction> Transaction { get; set; }
        public virtual DbSet<UserAccount> UserAccount { get; set; }
        public virtual DbSet<UserProfile> UserProfile { get; set; }

        public YourSharesContext(IHostingEnvironment env)
        {
            _env = env;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.4-servicing-10062");

            modelBuilder.Entity<Company>(entity =>
            {
                entity.ToTable("company");

                entity.Property(e => e.CompanyId)
                    .HasColumnName("company_id")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Address)
                    .HasColumnName("address")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.AdminProfileId).HasColumnName("admin_profile_id");

                entity.Property(e => e.Capital).HasColumnName("capital");

                entity.Property(e => e.CompanyName)
                    .IsRequired()
                    .HasColumnName("company_name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OptionPollAmount).HasColumnName("option_poll_amount");

                entity.Property(e => e.Phone)
                    .HasColumnName("phone")
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.TotalShares).HasColumnName("total_shares");
            });

            modelBuilder.Entity<GoogleAccount>(entity =>
            {
                entity.HasKey(e => e.UserProfileId)
                    .HasName("PK_google_account_user_profile_id");

                entity.ToTable("google_account");

                entity.Property(e => e.UserProfileId)
                    .HasColumnName("user_profile_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.GoogleAccountId)
                    .IsRequired()
                    .HasColumnName("google_account_id")
                    .HasMaxLength(30)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<RefShareTypeCode>(entity =>
            {
                entity.HasKey(e => e.ShareTypeCode)
                    .HasName("PK_ref_share_type_code_share_type_code");

                entity.ToTable("ref_share_type_code");

                entity.Property(e => e.ShareTypeCode)
                    .HasColumnName("share_type_code")
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<RefShareholderTypeCode>(entity =>
            {
                entity.HasKey(e => e.ShareholderTypeCode)
                    .HasName("PK_ref_shareholder_type_code_shareholder_type_code");

                entity.ToTable("ref_shareholder_type_code");

                entity.Property(e => e.ShareholderTypeCode)
                    .HasColumnName("shareholder_type_code")
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<RefTransactionStatusCode>(entity =>
            {
                entity.HasKey(e => e.TransactionStatusCode)
                    .HasName("PK_ref_transaction_status_code_transaction_status_code");

                entity.ToTable("ref_transaction_status_code");

                entity.Property(e => e.TransactionStatusCode)
                    .HasColumnName("transaction_status_code")
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<RefTransactionTypeCode>(entity =>
            {
                entity.HasKey(e => e.TransactionTypeCode)
                    .HasName("PK_ref_transaction_type_code_transaction_type_code");

                entity.ToTable("ref_transaction_type_code");

                entity.Property(e => e.TransactionTypeCode)
                    .HasColumnName("transaction_type_code")
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<RefUserAccountStatusCode>(entity =>
            {
                entity.HasKey(e => e.UserAccountStatusCode)
                    .HasName("PK_ref_user_account_status_code_user_account_status_code");

                entity.ToTable("ref_user_account_status_code");

                entity.Property(e => e.UserAccountStatusCode)
                    .HasColumnName("user_account_status_code")
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<RestrictedShare>(entity =>
            {
                entity.HasKey(e => e.ShareAccountId)
                    .HasName("PK_restricted_share_share_account_id");

                entity.ToTable("restricted_share");

                entity.Property(e => e.ShareAccountId)
                    .HasColumnName("share_account_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.AssignDate)
                    .IsRequired()
                    .HasColumnName("assign_date")
                    .IsRowVersion();

                entity.Property(e => e.ConvertibleRatio).HasColumnName("convertible_ratio");

                entity.Property(e => e.ConvertibleTime).HasColumnName("convertible_time");
            });

            modelBuilder.Entity<ShareAccount>(entity =>
            {
                entity.ToTable("share_account");

                entity.Property(e => e.ShareAccountId)
                    .HasColumnName("share_account_id")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.ShareAmount).HasColumnName("share_amount");

                entity.Property(e => e.ShareTypeCode)
                    .HasColumnName("share_type_code")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ShareholderId).HasColumnName("shareholder_id");
            });

            modelBuilder.Entity<Shareholder>(entity =>
            {
                entity.ToTable("shareholder");

                entity.Property(e => e.ShareholderId)
                    .HasColumnName("shareholder_id")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.CompanyId).HasColumnName("company_id");

                entity.Property(e => e.ShareholderTypeCode)
                    .IsRequired()
                    .HasColumnName("shareholder_type_code")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.UserId).HasColumnName("user_id");
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.ToTable("transaction");

                entity.Property(e => e.TransactionId)
                    .HasColumnName("transaction_id")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.ShareAccountId).HasColumnName("share_account_id");

                entity.Property(e => e.TransactionAmount).HasColumnName("transaction_amount");

                entity.Property(e => e.TransactionDate)
                    .IsRequired()
                    .HasColumnName("transaction_date")
                    .IsRowVersion();

                entity.Property(e => e.TransactionStatusCode)
                    .IsRequired()
                    .HasColumnName("transaction_status_code")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.TransactionTypeCode)
                    .IsRequired()
                    .HasColumnName("transaction_type_code")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.TransactionValue).HasColumnName("transaction_value");
            });

            modelBuilder.Entity<UserAccount>(entity =>
            {
                entity.HasKey(e => e.UserProfileId)
                    .HasName("PK_user_account_user_profile_id");

                entity.ToTable("user_account");

                entity.Property(e => e.UserProfileId)
                    .HasColumnName("user_profile_id")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.EmailConfirmationToken)
                    .HasColumnName("email_confirmation_token")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.PasswordHash)
                    .IsRequired()
                    .HasColumnName("password_hash")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.PasswordHashAlgorithm)
                    .IsRequired()
                    .HasColumnName("password_hash_algorithm")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.PasswordReminderToken)
                    .HasColumnName("password_reminder_token")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UserAccountStatusCode)
                    .IsRequired()
                    .HasColumnName("user_account_status_code")
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<UserProfile>(entity =>
            {
                entity.ToTable("user_profile");

                entity.Property(e => e.UserProfileId)
                    .HasColumnName("user_profile_id")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Address)
                    .HasColumnName("address")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .HasColumnName("first_name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .HasColumnName("last_name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Phone)
                    .HasColumnName("phone")
                    .HasMaxLength(15)
                    .IsUnicode(false);
            });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // get the configuration from the app settings
            var config = new ConfigurationBuilder()
                .SetBasePath(_env.ContentRootPath)
                .AddJsonFile("appsettings.json")
                .Build();

            // define the database to use
            optionsBuilder.UseSqlServer(config.GetConnectionString("DefaultConnection"));
        }
    }
}