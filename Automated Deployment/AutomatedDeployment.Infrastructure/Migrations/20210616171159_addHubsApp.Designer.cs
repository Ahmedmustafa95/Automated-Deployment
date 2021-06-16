﻿// <auto-generated />
using AutomatedDeployment.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AutomatedDeployment.Infrastructure.Migrations
{
    [DbContext(typeof(EfgconfigurationdbContext))]
    [Migration("20210616171159_addHubsApp")]
    partial class addHubsApp
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.7")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("AutomatedDeployment.Domain.Entities.Application", b =>
                {
                    b.Property<int>("AppID")
                        .HasColumnType("int");

                    b.Property<string>("AppName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("AppID");

                    b.ToTable("Applications");
                });

            modelBuilder.Entity("AutomatedDeployment.Domain.Entities.Hub", b =>
                {
                    b.Property<int>("HubID")
                        .HasColumnType("int");

                    b.Property<string>("HubName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("HubID");

                    b.ToTable("Hubs");
                });

            modelBuilder.Entity("AutomatedDeployment.Domain.Entities.HubsApplications", b =>
                {
                    b.Property<int>("HubID")
                        .HasColumnType("int");

                    b.Property<int>("AppID")
                        .HasColumnType("int");

                    b.Property<string>("AssemblyPath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BackupPath")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("HubID", "AppID");

                    b.HasIndex("AppID");

                    b.ToTable("HubsApplications");
                });

            modelBuilder.Entity("AutomatedDeployment.Domain.Entities.HubsApplications", b =>
                {
                    b.HasOne("AutomatedDeployment.Domain.Entities.Application", "Application")
                        .WithMany("HubsApplications")
                        .HasForeignKey("AppID")
                        .IsRequired();

                    b.HasOne("AutomatedDeployment.Domain.Entities.Hub", "Hub")
                        .WithMany("HubsApplications")
                        .HasForeignKey("HubID")
                        .IsRequired();

                    b.Navigation("Application");

                    b.Navigation("Hub");
                });

            modelBuilder.Entity("AutomatedDeployment.Domain.Entities.Application", b =>
                {
                    b.Navigation("HubsApplications");
                });

            modelBuilder.Entity("AutomatedDeployment.Domain.Entities.Hub", b =>
                {
                    b.Navigation("HubsApplications");
                });
#pragma warning restore 612, 618
        }
    }
}
