using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

 #nullable disable

namespace DailyLoan.Model.Entities.DailyLoan
{
    public partial class DailyLoanContext : DbContext
    {
        public DailyLoanContext()
        {
        }

        public DailyLoanContext(DbContextOptions<DailyLoanContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Config> Configs { get; set; }
        public virtual DbSet<Contract> Contracts { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<CustomerLine> CustomerLines { get; set; }
        public virtual DbSet<DailyCost> DailyCosts { get; set; }
        public virtual DbSet<House> Houses { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }
        public virtual DbSet<NotificationType> NotificationTypes { get; set; }
        public virtual DbSet<Request> Requests { get; set; }
        public virtual DbSet<RequestType> RequestTypes { get; set; }
        public virtual DbSet<SpecialRate> SpecialRates { get; set; }
        public virtual DbSet<StatusContract> StatusContracts { get; set; }
        public virtual DbSet<StatusCustomer> StatusCustomers { get; set; }
        public virtual DbSet<StatusCustomerLine> StatusCustomerLines { get; set; }
        public virtual DbSet<StatusHouse> StatusHouses { get; set; }
        public virtual DbSet<StatusNotification> StatusNotifications { get; set; }
        public virtual DbSet<StatusRequest> StatusRequests { get; set; }
        public virtual DbSet<StatusTransaction> StatusTransactions { get; set; }
        public virtual DbSet<StatusUser> StatusUsers { get; set; }
        public virtual DbSet<Transaction> Transactions { get; set; }
        public virtual DbSet<TransactionType> TransactionTypes { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserAccess> UserAccesses { get; set; }
        public virtual DbSet<UserPermission> UserPermissions { get; set; }


        private readonly string _connectionString;

        public DailyLoanContext(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new Exception("ConnectionString can't be empty");
            }

            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            if (!string.IsNullOrEmpty(_connectionString))
            {
                builder.UseSqlServer(_connectionString);
                builder.EnableSensitiveDataLogging();
                base.OnConfiguring(builder);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Thai_CI_AS");

            modelBuilder.Entity<Config>(entity =>
            {
                entity.ToTable("Config");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.HouseId).HasColumnName("HouseID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Contract>(entity =>
            {
                entity.ToTable("Contract");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ApproverId).HasColumnName("ApproverID");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");

                entity.Property(e => e.GuarantorId).HasColumnName("GuarantorID");

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("Customer");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Address).HasMaxLength(200);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.CustomerLineId).HasColumnName("CustomerLineID");

                entity.Property(e => e.Firstname).HasMaxLength(100);

                entity.Property(e => e.Idcard)
                    .IsRequired()
                    .HasColumnName("IDcard")
                    .HasMaxLength(50);

                entity.Property(e => e.Lastname).HasMaxLength(100);

                entity.Property(e => e.Nickname).HasMaxLength(50);

                entity.Property(e => e.Phone1).HasMaxLength(15);

                entity.Property(e => e.Phone2).HasMaxLength(15);

                entity.Property(e => e.ShortAddress).HasMaxLength(50);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<CustomerLine>(entity =>
            {
                entity.ToTable("CustomerLine");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.CustomerLineName).HasMaxLength(100);

                entity.Property(e => e.HouseId).HasColumnName("HouseID");

                entity.Property(e => e.Remark).HasMaxLength(500);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<DailyCost>(entity =>
            {
                entity.ToTable("DailyCost");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.CustomerLineId).HasColumnName("CustomerLineID");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.HouseId).HasColumnName("HouseID");

                entity.Property(e => e.OtherDetail).HasMaxLength(500);

                entity.Property(e => e.PoliceRemark1).HasMaxLength(500);

                entity.Property(e => e.PoliceRemark2).HasMaxLength(500);

                entity.Property(e => e.PoliceRemark3).HasMaxLength(500);

                entity.Property(e => e.Remark).HasMaxLength(500);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<House>(entity =>
            {
                entity.ToTable("House");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Address).HasMaxLength(500);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.District).HasMaxLength(100);

                entity.Property(e => e.HouseName)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Province).HasMaxLength(100);

                entity.Property(e => e.Remark).HasMaxLength(500);

                entity.Property(e => e.SubDistrict).HasMaxLength(100);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<Notification>(entity =>
            {
                entity.ToTable("Notification");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Message).HasMaxLength(200);

                entity.Property(e => e.Remark).HasMaxLength(500);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<NotificationType>(entity =>
            {
                entity.ToTable("NotificationType");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Request>(entity =>
            {
                entity.ToTable("Request");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AgentId).HasColumnName("AgentID");

                entity.Property(e => e.ApproverId).HasColumnName("ApproverID");

                entity.Property(e => e.ContractId).HasColumnName("ContractID");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<RequestType>(entity =>
            {
                entity.ToTable("RequestType");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<SpecialRate>(entity =>
            {
                entity.ToTable("SpecialRate");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.EndDate).HasColumnType("date");

                entity.Property(e => e.HouseId).HasColumnName("HouseID");

                entity.Property(e => e.MinCutDay).HasColumnName("MinCut(Day)");

                entity.Property(e => e.StartDate).HasColumnType("date");

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<StatusContract>(entity =>
            {
                entity.ToTable("Status_Contract");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<StatusCustomer>(entity =>
            {
                entity.ToTable("Status_Customer");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<StatusCustomerLine>(entity =>
            {
                entity.ToTable("Status_CustomerLine");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<StatusHouse>(entity =>
            {
                entity.ToTable("Status_House");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<StatusNotification>(entity =>
            {
                entity.ToTable("Status_Notification");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<StatusRequest>(entity =>
            {
                entity.ToTable("Status_Request");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<StatusTransaction>(entity =>
            {
                entity.ToTable("Status_Transaction");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<StatusUser>(entity =>
            {
                entity.ToTable("Status_User");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.ToTable("Transaction");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AgentId).HasColumnName("AgentID");

                entity.Property(e => e.ContractId).HasColumnName("ContractID");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Remark).HasMaxLength(500);
            });

            modelBuilder.Entity<TransactionType>(entity =>
            {
                entity.ToTable("TransactionType");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Firstname).HasMaxLength(100);

                entity.Property(e => e.HouseId).HasColumnName("HouseID");

                entity.Property(e => e.Lastname).HasMaxLength(100);

                entity.Property(e => e.Nickname).HasMaxLength(50);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Phone1).HasMaxLength(15);

                entity.Property(e => e.Phone2).HasMaxLength(15);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<UserAccess>(entity =>
            {
                entity.ToTable("UserAccess");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.UserAccess1)
                    .IsRequired()
                    .HasColumnName("UserAccess")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<UserPermission>(entity =>
            {
                entity.ToTable("User_Permission");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.CustomerLineId).HasColumnName("CustomerLineID");

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.Property(e => e.UserId).HasColumnName("UserID");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
