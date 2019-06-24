using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using YourShares.Domain.Models;

namespace YourShares.Data
{
    public class YourSharesContext : DbContext
    {
        private readonly IHostingEnvironment _env;

        public virtual DbSet<BonusShare> BonusShare { get; set; }
        public virtual DbSet<Company> Company { get; set; }
        public virtual DbSet<ShareAccount> ShareAccount { get; set; }
        public virtual DbSet<Shareholder> Shareholder { get; set; }
        public virtual DbSet<Transaction> Transaction { get; set; }
        public virtual DbSet<User> User { get; set; }

        public YourSharesContext(IHostingEnvironment env)
        {
            _env = env;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.4-servicing-10062");

            modelBuilder.Entity<BonusShare>(entity =>
            {
                entity.ToTable("Bonus_Share");

                entity.Property(e => e.BonusShareId)
                    .HasColumnName("Bonus_Share_ID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.AssignDate)
                    .IsRequired()
                    .HasColumnName("Assign_Date")
                    .IsRowVersion();

                entity.Property(e => e.ConvertibleRatio).HasColumnName("Convertible_Ratio");

                entity.Property(e => e.ConvertibleTime).HasColumnName("Convertible_Time");

                entity.Property(e => e.ShareAccountId).HasColumnName("Share_Account_ID");
            });

            modelBuilder.Entity<Company>(entity =>
            {
                entity.Property(e => e.CompanyId)
                    .HasColumnName("Company_ID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Address)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.AdminUserId).HasColumnName("Admin_User_ID");

                entity.Property(e => e.CompanyName)
                    .IsRequired()
                    .HasColumnName("Company_Name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TotalShare)
                    .IsRequired()
                    .HasColumnName("Total_Share");

                entity.Property(e => e.OptionPollAmount).HasColumnName("Option_Poll_Amount");

                entity.Property(e => e.Phone)
                    .HasMaxLength(15)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ShareAccount>(entity =>
            {
                entity.ToTable("Share_Account");

                entity.Property(e => e.ShareAccountId)
                    .HasColumnName("Share_Account_ID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.ShareAmount).HasColumnName("Share_Amount");

                entity.Property(e => e.ShareType).HasColumnName("Share_Type");

                entity.Property(e => e.ShareholderId).HasColumnName("Shareholder_ID");
            });

            modelBuilder.Entity<Shareholder>(entity =>
            {
                entity.Property(e => e.ShareholderId)
                    .HasColumnName("Shareholder_ID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.CompanyId).HasColumnName("Company_ID");

                entity.Property(e => e.ShareholderRole).HasColumnName("Shareholder_Role");

                entity.Property(e => e.UserId).HasColumnName("User_ID");
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.Property(e => e.TransactionId)
                    .HasColumnName("Transaction_ID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.ShareAccountId).HasColumnName("Share_Account_ID");

                entity.Property(e => e.TransactionAmount).HasColumnName("Transaction_Amount");

                entity.Property(e => e.TransactionDate)
                    .IsRequired()
                    .HasColumnName("Transaction_Date")
                    .IsRowVersion();

                entity.Property(e => e.TransactionStatus).HasColumnName("Transaction_Status");

                entity.Property(e => e.TransactionType).HasColumnName("Transaction_Type");

                entity.Property(e => e.TransactionValue).HasColumnName("Transaction_Value");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.UserId)
                    .HasColumnName("User_ID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Address)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasColumnName("First_Name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .HasColumnName("Last_Name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Phone)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserRole).HasColumnName("User_Role");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(50)
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