using System;
using System.Collections.Generic;
using InsuranceLib.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace InsuranceLib.DataBaseContollers
{
    public partial class InsuranceContext : DbContext
    {
        public InsuranceContext()
        {
        }

        public InsuranceContext(DbContextOptions<InsuranceContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Client> Clients { get; set; } = null!;
        public virtual DbSet<Employee> Employees { get; set; } = null!;
        public virtual DbSet<Policy> Policies { get; set; } = null!;
        public virtual DbSet<Risk> Risks { get; set; } = null!;
        public virtual DbSet<TypesOfPoliciesAndRisk> TypesOfPoliciesAndRisks { get; set; } = null!;
        public virtual DbSet<TypesOfPolicy> TypesOfPolicies { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(DataBaseConnection.Instance.GetConnection());
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>(entity =>
            {
                entity.Property(e => e.Adres)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.BornDate).HasColumnType("date");

                entity.Property(e => e.FirstName)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Gender)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.MiddleName)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Passport)
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.Property(e => e.Age).HasDefaultValueSql("((18))");

                entity.Property(e => e.FirstName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Gender)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.MiddleName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Post)
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Policy>(entity =>
            {
                entity.Property(e => e.FinishDate).HasColumnType("date");

                entity.Property(e => e.StartDate).HasColumnType("date");

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.Policies)
                    .HasForeignKey(d => d.ClientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Policies__Client__3D5E1FD2");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.Policies)
                    .HasForeignKey(d => d.EmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Policies__Employ__3E52440B");

                entity.HasOne(d => d.TypesOfPolicie)
                    .WithMany(p => p.Policies)
                    .HasForeignKey(d => d.TypesOfPolicieId)
                    .HasConstraintName("FK__Policies__TypesO__3C69FB99");
            });

            modelBuilder.Entity<Risk>(entity =>
            {
                entity.HasIndex(e => e.Name, "UQ__Risks__737584F63EA97FE8")
                    .IsUnique();

                entity.Property(e => e.Description)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TypesOfPoliciesAndRisk>(entity =>
            {
                entity.HasOne(d => d.Risk)
                    .WithMany(p => p.TypesOfPoliciesAndRisks)
                    .HasForeignKey(d => d.RiskId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TypesOfPo__RiskI__32E0915F");

                entity.HasOne(d => d.TypesOfPolicie)
                    .WithMany(p => p.TypesOfPoliciesAndRisks)
                    .HasForeignKey(d => d.TypesOfPolicieId)
                    .HasConstraintName("FK__TypesOfPo__Types__31EC6D26");
            });

            modelBuilder.Entity<TypesOfPolicy>(entity =>
            {
                entity.HasIndex(e => e.Name, "UQ__TypesOfP__737584F65A52D714")
                    .IsUnique();

                entity.HasIndex(e => e.Condition, "UQ__TypesOfP__B44D2EFA18223EDC")
                    .IsUnique();

                entity.Property(e => e.Condition)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Description)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
