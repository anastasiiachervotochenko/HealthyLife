using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using HealthyLife.Model;
using Microsoft.Extensions.Configuration;

#nullable disable

namespace HealthyLife.Context
{
    public partial class DevContext : DbContext
    {
        private readonly IConfiguration _config;
        public DevContext()
        {
           
        }

        public DevContext(DbContextOptions options, IConfiguration config)
            : base(options)
        {
            _config = config;
        }

        public virtual DbSet<AccountLog> AccountLogs { get; set; }
        public virtual DbSet<Admin> Admins { get; set; }
        public virtual DbSet<AthletToGroup> AthletToGroups { get; set; }
        public virtual DbSet<AthletsToRelative> AthletsToRelatives { get; set; }
        public virtual DbSet<BodyInformation> BodyInformations { get; set; }
        public virtual DbSet<CoachToInstitution> CoachToInstitutions { get; set; }
        public virtual DbSet<FoodRate> FoodRates { get; set; }
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<InformationFromDevice> InformationFromDevices { get; set; }
        public virtual DbSet<Institution> Institutions { get; set; }
        public virtual DbSet<InstitutionLog> InstitutionLogs { get; set; }
        public virtual DbSet<NutrionInformation> NutrionInformations { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }
        public virtual DbSet<Receipt> Receipts { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_config["ConnectionString"]);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Cyrillic_General_CI_AS");

            modelBuilder.Entity<AccountLog>(entity =>
            {
                entity.ToTable("AccountLog");

                entity.Property(e => e.Id)
                    .HasMaxLength(50)
                    .HasColumnName("id");

                entity.Property(e => e.Role)
                    .HasMaxLength(50)
                    .HasColumnName("role");

                entity.Property(e => e.UserId)
                    .HasMaxLength(50)
                    .HasColumnName("userId");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AccountLogs)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_AccountLog_User");
            });
            modelBuilder.Entity<Admin>(entity =>
            {
                entity.ToTable("Admin");

                entity.Property(e => e.Id)
                    .HasMaxLength(50)
                    .HasColumnName("id");

                entity.Property(e => e.Fio).HasColumnName("fio");

                entity.Property(e => e.Login)
                    .IsRequired()
                    .HasColumnName("login");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password");

                entity.Property(e => e.Salt)
                    .IsRequired()
                    .HasColumnName("salt");
                
                entity.Property(e => e.Active)
                    .IsRequired()
                    .HasColumnName("active");
            });
            modelBuilder.Entity<AthletToGroup>(entity =>
            {
                entity.ToTable("AthletToGroup");

                entity.Property(e => e.Id)
                    .HasMaxLength(50)
                    .HasColumnName("id");

                entity.Property(e => e.AthletId)
                    .HasMaxLength(50)
                    .HasColumnName("athletId");

                entity.Property(e => e.EndDate)
                    .HasColumnType("date")
                    .HasColumnName("endDate");

                entity.Property(e => e.GroupId)
                    .HasMaxLength(50)
                    .HasColumnName("groupId");

                entity.Property(e => e.StartDate)
                    .HasColumnType("date")
                    .HasColumnName("startDate");
            });

            modelBuilder.Entity<AthletsToRelative>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(50)
                    .HasColumnName("id");

                entity.Property(e => e.AthleteId)
                    .HasMaxLength(50)
                    .HasColumnName("athleteId");

                entity.Property(e => e.RelativeId)
                    .HasMaxLength(50)
                    .HasColumnName("relativeId");

                entity.Property(e => e.Role)
                    .HasMaxLength(50)
                    .HasColumnName("role");

                entity.HasOne(d => d.Athlete)
                    .WithMany(p => p.AthletsToRelativeAthletes)
                    .HasForeignKey(d => d.AthleteId)
                    .HasConstraintName("FK_AthletsToRelatives_User1");

                entity.HasOne(d => d.Relative)
                    .WithMany(p => p.AthletsToRelativeRelatives)
                    .HasForeignKey(d => d.RelativeId)
                    .HasConstraintName("FK_AthletsToRelatives_User");
            });

            modelBuilder.Entity<BodyInformation>(entity =>
            {
                entity.ToTable("BodyInformation");

                entity.Property(e => e.Id)
                    .HasMaxLength(50)
                    .HasColumnName("id");

                entity.Property(e => e.AbdominalGirth).HasColumnName("abdominalGirth");

                entity.Property(e => e.AthletId)
                    .HasMaxLength(50)
                    .HasColumnName("athletId");

                entity.Property(e => e.ButtocksGirth).HasColumnName("buttocksGirth");

                entity.Property(e => e.ChestGirth).HasColumnName("chestGirth");

                entity.Property(e => e.Date)
                    .HasColumnType("date")
                    .HasColumnName("date");

                entity.Property(e => e.Height).HasColumnName("height");

                entity.Property(e => e.ThighGirth).HasColumnName("thighGirth");

                entity.Property(e => e.WaistCircumference).HasColumnName("waistCircumference");

                entity.Property(e => e.Weight).HasColumnName("weight");

                entity.HasOne(d => d.Athlet)
                    .WithMany(p => p.BodyInformations)
                    .HasForeignKey(d => d.AthletId)
                    .HasConstraintName("FK_BodyInformation_User");
            });

            modelBuilder.Entity<CoachToInstitution>(entity =>
            {
                entity.ToTable("CoachToInstitution");

                entity.Property(e => e.Id)
                    .HasMaxLength(50)
                    .HasColumnName("id");

                entity.Property(e => e.CoachId)
                    .HasMaxLength(50)
                    .HasColumnName("coachId");

                entity.Property(e => e.EndDate)
                    .HasColumnType("date")
                    .HasColumnName("endDate");

                entity.Property(e => e.InstitutionId)
                    .HasMaxLength(50)
                    .HasColumnName("institutionId");

                entity.Property(e => e.Position).HasColumnName("position");

                entity.Property(e => e.StartDate)
                    .HasColumnType("date")
                    .HasColumnName("startDate");

                entity.HasOne(d => d.Coach)
                    .WithMany(p => p.CoachToInstitutions)
                    .HasForeignKey(d => d.CoachId)
                    .HasConstraintName("FK_CoachToInstitution_User");

                entity.HasOne(d => d.Institution)
                    .WithMany(p => p.CoachToInstitutions)
                    .HasForeignKey(d => d.InstitutionId)
                    .HasConstraintName("FK_CoachToInstitution_Institution");
            });

            modelBuilder.Entity<FoodRate>(entity =>
            {
                entity.ToTable("FoodRate");

                entity.Property(e => e.Id)
                    .HasMaxLength(50)
                    .HasColumnName("id");

                entity.Property(e => e.AmountOfWater).HasColumnName("amountOfWater");

                entity.Property(e => e.AthletId)
                    .HasMaxLength(50)
                    .HasColumnName("athletId");

                entity.Property(e => e.Carbohydrates).HasColumnName("carbohydrates");

                entity.Property(e => e.CoachId)
                    .HasMaxLength(50)
                    .HasColumnName("coachId");

                entity.Property(e => e.Date)
                    .HasColumnType("date")
                    .HasColumnName("date");

                entity.Property(e => e.Fats).HasColumnName("fats");

                entity.Property(e => e.Kcal).HasColumnName("kcal");

                entity.Property(e => e.Proteins).HasColumnName("proteins");

                entity.HasOne(d => d.Athlet)
                    .WithMany(p => p.FoodRateAthlets)
                    .HasForeignKey(d => d.AthletId)
                    .HasConstraintName("FK_FoodRate_User");

                entity.HasOne(d => d.Coach)
                    .WithMany(p => p.FoodRateCoaches)
                    .HasForeignKey(d => d.CoachId)
                    .HasConstraintName("FK_FoodRate_User1");
            });

            modelBuilder.Entity<Group>(entity =>
            {
                entity.ToTable("Group");

                entity.Property(e => e.Id)
                    .HasMaxLength(50)
                    .HasColumnName("id");

                entity.Property(e => e.CoachInInstitutionId)
                    .HasMaxLength(50)
                    .HasColumnName("coachInInstitutionId");

                entity.Property(e => e.EndDate)
                    .HasColumnType("date")
                    .HasColumnName("endDate");

                entity.Property(e => e.GroupName)
                    .HasMaxLength(50)
                    .HasColumnName("groupName");

                entity.Property(e => e.Sport)
                    .HasMaxLength(50)
                    .HasColumnName("sport");

                entity.Property(e => e.StartDate)
                    .HasColumnType("date")
                    .HasColumnName("startDate");

                entity.Property(e => e.Type)
                    .HasMaxLength(50)
                    .HasColumnName("type");
            });

            modelBuilder.Entity<InformationFromDevice>(entity =>
            {
                entity.ToTable("InformationFromDevice");

                entity.Property(e => e.Id)
                    .HasMaxLength(50)
                    .HasColumnName("id");

                entity.Property(e => e.AthletId)
                    .HasMaxLength(50)
                    .HasColumnName("athletId");

                entity.Property(e => e.OxygenLevel).HasColumnName("oxygenLevel");

                entity.Property(e => e.PositionX).HasColumnName("positionX");

                entity.Property(e => e.PositionY).HasColumnName("positionY");

                entity.Property(e => e.PositionZ).HasColumnName("positionZ");

                entity.Property(e => e.Pulse).HasColumnName("pulse");

                entity.Property(e => e.Temperature).HasColumnName("temperature");
            });

            modelBuilder.Entity<Institution>(entity =>
            {
                entity.ToTable("Institution");

                entity.Property(e => e.Id)
                    .HasMaxLength(50)
                    .HasColumnName("id");

                entity.Property(e => e.Address).HasColumnName("address");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .HasColumnName("email");

                entity.Property(e => e.Name).HasColumnName("name");

                entity.Property(e => e.Phone)
                    .HasMaxLength(50)
                    .HasColumnName("phone");

                entity.Property(e => e.SiteLink).HasColumnName("siteLink");
                
                entity.Property(e => e.Active).HasColumnName("active");
            });

            modelBuilder.Entity<InstitutionLog>(entity =>
            {
                entity.ToTable("InstitutionLog");

                entity.Property(e => e.Id)
                    .HasMaxLength(50)
                    .HasColumnName("id");

                entity.Property(e => e.InstitutionId)
                    .HasMaxLength(50)
                    .HasColumnName("institutionId");

                entity.Property(e => e.Login).HasColumnName("login");

                entity.Property(e => e.Password)
                    .HasMaxLength(50)
                    .HasColumnName("password");

                entity.Property(e => e.Salt)
                    .HasMaxLength(50)
                    .HasColumnName("salt");

                entity.HasOne(d => d.Institution)
                    .WithMany(p => p.InstitutionLogs)
                    .HasForeignKey(d => d.InstitutionId)
                    .HasConstraintName("FK_InstitutionLog_Institution");
            });

            modelBuilder.Entity<NutrionInformation>(entity =>
            {
                entity.ToTable("NutrionInformation");

                entity.Property(e => e.Id)
                    .HasMaxLength(50)
                    .HasColumnName("id");

                entity.Property(e => e.AmountOfWater).HasColumnName("amountOfWater");

                entity.Property(e => e.AthletId)
                    .HasMaxLength(50)
                    .HasColumnName("athletId");

                entity.Property(e => e.Carbohydrates).HasColumnName("carbohydrates");

                entity.Property(e => e.Date)
                    .HasColumnType("date")
                    .HasColumnName("date");

                entity.Property(e => e.Fats).HasColumnName("fats");

                entity.Property(e => e.Kcal).HasColumnName("kcal");

                entity.Property(e => e.Proteins).HasColumnName("proteins");

                entity.HasOne(d => d.Athlet)
                    .WithMany(p => p.NutrionInformations)
                    .HasForeignKey(d => d.AthletId)
                    .HasConstraintName("FK_NutrionInformation_User");
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.ToTable("Payment");

                entity.Property(e => e.Id)
                    .HasMaxLength(50)
                    .HasColumnName("id");

                entity.Property(e => e.AthletId)
                    .HasMaxLength(50)
                    .HasColumnName("athletId");

                entity.Property(e => e.Date)
                    .HasColumnType("date")
                    .HasColumnName("date");

                entity.Property(e => e.PayerId)
                    .HasMaxLength(50)
                    .HasColumnName("payerId");

                entity.Property(e => e.ReceiptId)
                    .HasMaxLength(50)
                    .HasColumnName("receiptId");

                entity.Property(e => e.Sum).HasColumnName("sum");
            });

            modelBuilder.Entity<Receipt>(entity =>
            {
                entity.ToTable("Receipt");

                entity.Property(e => e.Id)
                    .HasMaxLength(50)
                    .HasColumnName("id");

                entity.Property(e => e.Date)
                    .HasColumnType("date")
                    .HasColumnName("date");

                entity.Property(e => e.GroupId)
                    .HasMaxLength(50)
                    .HasColumnName("groupId");

                entity.Property(e => e.Sum).HasColumnName("sum");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.Id)
                    .HasMaxLength(50)
                    .HasColumnName("id");

                entity.Property(e => e.BirthdayDate)
                    .HasColumnType("date")
                    .HasColumnName("birthdayDate");

                entity.Property(e => e.Email).HasColumnName("email");

                entity.Property(e => e.Fio).HasColumnName("fio");

                entity.Property(e => e.Login)
                    .IsRequired()
                    .HasColumnName("login");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password");

                entity.Property(e => e.Phone)
                    .HasMaxLength(50)
                    .HasColumnName("phone");

                entity.Property(e => e.Salt)
                    .IsRequired()
                    .HasColumnName("salt");

                entity.Property(e => e.Sex)
                    .HasMaxLength(6)
                    .HasColumnName("sex");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
