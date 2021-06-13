﻿// <auto-generated />
using AutomatedDeployment.Api;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AutomatedDeployment.Api.Migrations
{
    [DbContext(typeof(EfgconfigurationdbContext))]
    partial class EfgconfigurationdbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.7")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("AutomatedDeployment.Api.Application", b =>
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

            modelBuilder.Entity("AutomatedDeployment.Api.Configuration", b =>
                {
                    b.Property<int>("HubID")
                        .HasColumnType("int");

                    b.Property<int>("AppID")
                        .HasColumnType("int");

                    b.Property<string>("ApprovedBy")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("AssemblyPath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BackupPath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ConfigurationPath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DeployedBy")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("RequestedBy")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("HubID", "AppID");

                    b.HasIndex("AppID");

                    b.ToTable("Configurations");
                });

            modelBuilder.Entity("AutomatedDeployment.Api.Hub", b =>
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

            modelBuilder.Entity("AutomatedDeployment.Api.Configuration", b =>
                {
                    b.HasOne("AutomatedDeployment.Api.Application", "App")
                        .WithMany("Configurations")
                        .HasForeignKey("AppID")
                        .HasConstraintName("FK_Configurations_Applications")
                        .IsRequired();

                    b.HasOne("AutomatedDeployment.Api.Hub", "Hub")
                        .WithMany("Configurations")
                        .HasForeignKey("HubID")
                        .HasConstraintName("FK_Configurations_Hubs")
                        .IsRequired();

                    b.Navigation("App");

                    b.Navigation("Hub");
                });

            modelBuilder.Entity("AutomatedDeployment.Api.Application", b =>
                {
                    b.Navigation("Configurations");
                });

            modelBuilder.Entity("AutomatedDeployment.Api.Hub", b =>
                {
                    b.Navigation("Configurations");
                });
#pragma warning restore 612, 618
        }
    }
}
