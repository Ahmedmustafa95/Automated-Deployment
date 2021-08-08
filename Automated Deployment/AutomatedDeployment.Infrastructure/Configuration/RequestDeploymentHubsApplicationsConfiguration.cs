using AutomatedDeployment.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatedDeployment.Infrastructure.Configuration
{
    public class RequestDeploymentHubsApplicationsConfiguration : IEntityTypeConfiguration<RequestDeploymentHubsApplications>
    {
        public void Configure(EntityTypeBuilder<RequestDeploymentHubsApplications> builder)
        {
            builder.HasKey(RDHA => new { RDHA.ApplicationId, RDHA.HubId, RDHA.RequestDeploymentId });
            builder.HasOne(H => H.Hub)
                   .WithMany(RDHA => RDHA.RequestDeploymentHubsApplications)
                   .HasForeignKey(RDHA => RDHA.HubId)
                   .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(H => H.Application)
                  .WithMany(RDHA => RDHA.RequestDeploymentHubsApplications)
                  .HasForeignKey(RDHA => RDHA.ApplicationId)
                  .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(H => H.RequestDeployment)
                  .WithMany(RDHA => RDHA.RequestDeploymentHubsApplications)
                  .HasForeignKey(RDHA => RDHA.RequestDeploymentId)
                  .OnDelete(DeleteBehavior.ClientSetNull);            
        }
    }
}
