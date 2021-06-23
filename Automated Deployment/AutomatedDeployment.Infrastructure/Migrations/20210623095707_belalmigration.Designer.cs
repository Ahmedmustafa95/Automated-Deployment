﻿// <auto-generated />
using System;
using AutomatedDeployment.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AutomatedDeployment.Infrastructure.Migrations
{
    [DbContext(typeof(EfgconfigurationdbContext))]
    [Migration("20210623095707_belalmigration")]
    partial class belalmigration
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
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AppName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("AppID");

                    b.ToTable("Applications");
                });

            modelBuilder.Entity("AutomatedDeployment.Domain.Entities.Deployment", b =>
                {
                    b.Property<int>("DeploymentID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AppID")
                        .HasColumnType("int");

                    b.Property<string>("ApprovedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DeployedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DeploymentDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("HubID")
                        .HasColumnType("int");

                    b.Property<string>("RequestedBy")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("DeploymentID");

                    b.HasIndex("AppID");

                    b.HasIndex("HubID");

                    b.ToTable("Deployments");
                });

            modelBuilder.Entity("AutomatedDeployment.Domain.Entities.DeploymentFiles", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("DeploymentID")
                        .HasColumnType("int");

                    b.Property<string>("FilesName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("DeploymentID");

                    b.ToTable("DeploymentFiles");
                });

            modelBuilder.Entity("AutomatedDeployment.Domain.Entities.Hub", b =>
                {
                    b.Property<int>("HubID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

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

            modelBuilder.Entity("AutomatedDeployment.Domain.Entities.Deployment", b =>
                {
                    b.HasOne("AutomatedDeployment.Domain.Entities.Application", "Application")
                        .WithMany("Deployments")
                        .HasForeignKey("AppID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AutomatedDeployment.Domain.Entities.Hub", "Hub")
                        .WithMany("Deployments")
                        .HasForeignKey("HubID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Application");

                    b.Navigation("Hub");
                });

            modelBuilder.Entity("AutomatedDeployment.Domain.Entities.DeploymentFiles", b =>
                {
                    b.HasOne("AutomatedDeployment.Domain.Entities.Deployment", "Deployments")
                        .WithMany("DeploymentFiles")
                        .HasForeignKey("DeploymentID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Deployments");
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
                    b.Navigation("Deployments");

                    b.Navigation("HubsApplications");
                });

            modelBuilder.Entity("AutomatedDeployment.Domain.Entities.Deployment", b =>
                {
                    b.Navigation("DeploymentFiles");
                });

            modelBuilder.Entity("AutomatedDeployment.Domain.Entities.Hub", b =>
                {
                    b.Navigation("Deployments");

                    b.Navigation("HubsApplications");
                });
#pragma warning restore 612, 618
        }
    }
}
