using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using YourShares.Domain.Models;

namespace YourShares.Data
{
    public class YourSharesContext : DbContext
    {
        private readonly IHostingEnvironment _env;

        public virtual DbSet<Company> Company { get; set; }
        public virtual DbSet<FacebookAccount> FacebookAccount { get; set; }
        public virtual DbSet<GoogleAccount> GoogleAccount { get; set; }
        public virtual DbSet<RefShareTypeCode> RefShareTypeCode { get; set; }
        public virtual DbSet<RefShareholderTypeCode> RefShareholderTypeCode { get; set; }
        public virtual DbSet<RefTransactionStatusCode> RefTransactionStatusCode { get; set; }
        public virtual DbSet<RefTransactionTypeCode> RefTransactionTypeCode { get; set; }
        public virtual DbSet<RefUserAccountStatusCode> RefUserAccountStatusCode { get; set; }
        public virtual DbSet<RestrictedShare> RestrictedShare { get; set; }
        public virtual DbSet<Round> Round { get; set; }
        public virtual DbSet<RoundInvestor> RoundInvestor { get; set; }
        public virtual DbSet<ShareAccount> ShareAccount { get; set; }
        public virtual DbSet<Shareholder> Shareholder { get; set; }
        public virtual DbSet<Transaction> Transaction { get; set; }
        public virtual DbSet<TransactionRequest> TransactionRequest { get; set; }
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
                    .ValueGeneratedNever();

                entity.Property(e => e.Address)
                    .HasColumnName("address")
                    .HasMaxLength(50);

                entity.Property(e => e.AdminProfileId).HasColumnName("admin_profile_id");

                entity.Property(e => e.Capital).HasColumnName("capital");

                entity.Property(e => e.CompanyName)
                    .IsRequired()
                    .HasColumnName("company_name")
                    .HasMaxLength(50);

                entity.Property(e => e.OptionPollAmount).HasColumnName("option_poll_amount");

                entity.Property(e => e.Phone)
                    .HasColumnName("phone")
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.TotalShares).HasColumnName("total_shares");
            });

            modelBuilder.Entity<FacebookAccount>(entity =>
            {
                entity.HasKey(e => e.UserProfileId)
                    .HasName("PK_facebook_account_user_profile_id");

                entity.ToTable("facebook_account");

                entity.Property(e => e.UserProfileId)
                    .HasColumnName("user_profile_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.FacebookAccountId)
                    .IsRequired()
                    .HasColumnName("facebook_account_id")
                    .HasMaxLength(30)
                    .IsUnicode(false);
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

                entity.Property(e => e.AssignDate).HasColumnName("assign_date");

                entity.Property(e => e.ConvertibleRatio).HasColumnName("convertible_ratio");

                entity.Property(e => e.ConvertibleTime).HasColumnName("convertible_time");
            });

            modelBuilder.Entity<Round>(entity =>
            {
                entity.ToTable("round");

                entity.Property(e => e.RoundId)
                    .HasColumnName("round_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.CompanyId).HasColumnName("company_id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(50);

                entity.Property(e => e.PostRoundShares).HasColumnName("post_round_shares");

                entity.Property(e => e.PreRoundShares).HasColumnName("pre_round_shares");
            });

            modelBuilder.Entity<RoundInvestor>(entity =>
            {
                entity.ToTable("round_investor");

                entity.Property(e => e.RoundInvestorId)
                    .HasColumnName("round_investor_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.InvestedValue).HasColumnName("invested_value");

                entity.Property(e => e.InvestorName)
                    .IsRequired()
                    .HasColumnName("investor_name")
                    .HasMaxLength(100);

                entity.Property(e => e.RoundId).HasColumnName("round_id");

                entity.Property(e => e.ShareAmount).HasColumnName("share_amount");

                entity.Property(e => e.SharesHoldingPercentage).HasColumnName("shares_holding_percentage");
            });

            modelBuilder.Entity<ShareAccount>(entity =>
            {
                entity.ToTable("share_account");

                entity.Property(e => e.ShareAccountId)
                    .HasColumnName("share_account_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.ShareAmount).HasColumnName("share_amount");

                entity.Property(e => e.ShareTypeCode)
                    .IsRequired()
                    .HasColumnName("share_type_code")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ShareholderId).HasColumnName("shareholder_id");

                entity.HasOne(d => d.ShareTypeCodeNavigation)
                    .WithMany(p => p.ShareAccount)
                    .HasForeignKey(d => d.ShareTypeCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_share_account_share_type_code");
            });

            modelBuilder.Entity<Shareholder>(entity =>
            {
                entity.ToTable("shareholder");

                entity.Property(e => e.ShareholderId)
                    .HasColumnName("shareholder_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.CompanyId).HasColumnName("company_id");

                entity.Property(e => e.ShareholderTypeCode)
                    .IsRequired()
                    .HasColumnName("shareholder_type_code")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.UserProfileId).HasColumnName("user_profile_id");

                entity.HasOne(d => d.ShareholderTypeCodeNavigation)
                    .WithMany(p => p.Shareholder)
                    .HasForeignKey(d => d.ShareholderTypeCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_shareholder_shareholder_type_code");
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.ToTable("transaction");

                entity.Property(e => e.TransactionId)
                    .HasColumnName("transaction_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.ShareAccountId).HasColumnName("share_account_id");

                entity.Property(e => e.TransactionAmount).HasColumnName("transaction_amount");

                entity.Property(e => e.TransactionDate).HasColumnName("transaction_date");

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

                entity.HasOne(d => d.TransactionStatusCodeNavigation)
                    .WithMany(p => p.Transaction)
                    .HasForeignKey(d => d.TransactionStatusCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_transaction_transaction_status_code");

                entity.HasOne(d => d.TransactionTypeCodeNavigation)
                    .WithMany(p => p.Transaction)
                    .HasForeignKey(d => d.TransactionTypeCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_transaction_transaction_type_code");
            });

            modelBuilder.Entity<TransactionRequest>(entity =>
            {
                entity.ToTable("transaction_request");

                entity.Property(e => e.TransactionRequestId)
                    .HasColumnName("transaction_request_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.ApproverId).HasColumnName("approver_id");

                entity.Property(e => e.RequestMessage).HasColumnName("request_message");

                entity.Property(e => e.TransactionInId).HasColumnName("transaction_in_id");

                entity.Property(e => e.TransactionOutId).HasColumnName("transaction_out_id");
            });

            modelBuilder.Entity<UserAccount>(entity =>
            {
                entity.HasKey(e => e.UserProfileId)
                    .HasName("PK_user_account_user_profile_id");

                entity.ToTable("user_account");

                entity.Property(e => e.UserProfileId)
                    .HasColumnName("user_profile_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email")
                    .HasMaxLength(200)
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

                entity.Property(e => e.PasswordSalt).HasColumnName("password_salt");

                entity.Property(e => e.UserAccountStatusCode)
                    .IsRequired()
                    .HasColumnName("user_account_status_code")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.HasOne(d => d.UserAccountStatusCodeNavigation)
                    .WithMany(p => p.UserAccount)
                    .HasForeignKey(d => d.UserAccountStatusCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_user_account_user_account_status_code");
            });

            modelBuilder.Entity<UserProfile>(entity =>
            {
                entity.ToTable("user_profile");

                entity.Property(e => e.UserProfileId)
                    .HasColumnName("user_profile_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Address)
                    .HasColumnName("address")
                    .HasMaxLength(200);

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasColumnName("first_name")
                    .HasMaxLength(50);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasColumnName("last_name")
                    .HasMaxLength(50);

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
            optionsBuilder.UseSqlServer(config.GetConnectionString("LocalConnection"));
        }
    }
}