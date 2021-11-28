using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

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

        public virtual DbSet<Config> Config { get; set; }
        public virtual DbSet<Contract> Contract { get; set; }
        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<CustomerLine> CustomerLine { get; set; }
        public virtual DbSet<DailyCost> DailyCost { get; set; }
        public virtual DbSet<House> House { get; set; }
        public virtual DbSet<Notification> Notification { get; set; }
        public virtual DbSet<NotificationType> NotificationType { get; set; }
        public virtual DbSet<Request> Request { get; set; }
        public virtual DbSet<RequestType> RequestType { get; set; }
        public virtual DbSet<SpecialRate> SpecialRate { get; set; }
        public virtual DbSet<StatusContract> StatusContract { get; set; }
        public virtual DbSet<StatusCustomer> StatusCustomer { get; set; }
        public virtual DbSet<StatusCustomerLine> StatusCustomerLine { get; set; }
        public virtual DbSet<StatusHouse> StatusHouse { get; set; }
        public virtual DbSet<StatusNotification> StatusNotification { get; set; }
        public virtual DbSet<StatusRequest> StatusRequest { get; set; }
        public virtual DbSet<StatusTransaction> StatusTransaction { get; set; }
        public virtual DbSet<StatusUser> StatusUser { get; set; }
        public virtual DbSet<Transaction> Transaction { get; set; }
        public virtual DbSet<TransactionType> TransactionType { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserAccess> UserAccess { get; set; }
        public virtual DbSet<UserPermission> UserPermission { get; set; }

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
            modelBuilder.Entity<Config>(entity =>
            {
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
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ApproverId).HasColumnName("ApproverID");

                entity.Property(e => e.ContractId)
                    .HasColumnName("ContractID")
                    .HasMaxLength(50);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");

                entity.Property(e => e.GuarantorId).HasColumnName("GuarantorID");

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<Customer>(entity =>
            {
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
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.CustomerLineName).HasMaxLength(100);

                entity.Property(e => e.HouseId).HasColumnName("HouseID");

                entity.Property(e => e.Remark).HasMaxLength(500);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<DailyCost>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.CustomerLineId).HasColumnName("CustomerLineID");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.HouseId).HasColumnName("HouseID");

                entity.Property(e => e.OtherDetail).HasMaxLength(500);

                entity.Property(e => e.OtherIncomeRemark).HasMaxLength(100);

                entity.Property(e => e.PoliceRemark1).HasMaxLength(500);

                entity.Property(e => e.PoliceRemark2).HasMaxLength(500);

                entity.Property(e => e.PoliceRemark3).HasMaxLength(500);

                entity.Property(e => e.Remark).HasMaxLength(500);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<House>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.HouseName)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Province).HasMaxLength(100);

                entity.Property(e => e.Region).HasMaxLength(100);

                entity.Property(e => e.Remark).HasMaxLength(500);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<Notification>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.CustomerLineId).HasColumnName("CustomerLineID");

                entity.Property(e => e.HouseId).HasColumnName("HouseID");

                entity.Property(e => e.Message).HasMaxLength(200);

                entity.Property(e => e.Remark).HasMaxLength(500);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<NotificationType>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Request>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AgentId).HasColumnName("AgentID");

                entity.Property(e => e.ApproverId).HasColumnName("ApproverID");

                entity.Property(e => e.ContractId).HasColumnName("ContractID");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<RequestType>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<SpecialRate>(entity =>
            {
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
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ContractId).HasColumnName("ContractID");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.CustomerLineId).HasColumnName("CustomerLineID");

                entity.Property(e => e.Remark).HasMaxLength(500);
            });

            modelBuilder.Entity<TransactionType>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<User>(entity =>
            {
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

                entity.Property(e => e.UserId).HasColumnName("UserID");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
