using AutomatedDeployment.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatedDeployment.Infrastructure.Context
{
    public partial class EfgconfigurationdbContext:DbContext
    {
        public EfgconfigurationdbContext()
        {
        }

        public EfgconfigurationdbContext(DbContextOptions<EfgconfigurationdbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Application> Applications { get; set; }
        public virtual DbSet<HubsApplications> HubsApplications { get; set; }
        public virtual DbSet<Hub> Hubs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=automated-deployment.database.windows.net;Initial Catalog=EfgConfigurationDB;Persist Security Info=True;User ID=EFgTeam;Password=Efg123456789")
                    .EnableSensitiveDataLogging().LogTo( i=>Debug.WriteLine(i));
                    
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Application>(entity =>
            {
                entity.Property(e => e.AppID).ValueGeneratedNever();
            });

            //modelBuilder.Entity<Configuration>(entity =>
            //{
            //    entity.HasKey(e => new { e.HubID, e.AppID });

            //    entity.HasOne(d => d.App)
            //        .WithMany(p => p.Configurations)
            //        .HasForeignKey(d => d.AppID)
            //        .OnDelete(DeleteBehavior.ClientSetNull)
            //        .HasConstraintName("FK_Configurations_Applications");

            //    entity.HasOne(d => d.Hub)
            //        .WithMany(p => p.Configurations)
            //        .HasForeignKey(d => d.HubID)
            //        .OnDelete(DeleteBehavior.ClientSetNull)
            //        .HasConstraintName("FK_Configurations_Hubs");
            //});

            modelBuilder.Entity<HubsApplications>(entity =>
           {
               entity.HasKey(e => new { e.HubID, e.AppID });


               entity.HasOne(d => d.Application)
                   .WithMany(p => p.HubsApplications)
                   .HasForeignKey(d => d.AppID)
                   .OnDelete(DeleteBehavior.ClientSetNull);
                 

           });

            modelBuilder.Entity<HubsApplications>(entity =>
            {
                entity.HasKey(e => new { e.HubID, e.AppID });


                entity.HasOne(d => d.Hub)
                    .WithMany(p => p.HubsApplications)
                    .HasForeignKey(d => d.HubID)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<Hub>(entity =>
            {
                entity.Property(e => e.HubID).ValueGeneratedNever();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
