using Kitbag.Builder.Persistence.Core.Common.Logs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TLJ.PortsAndAdapters.Infrastructure.Persistence.Configurations
{
    public class AuditConfiguration : IEntityTypeConfiguration<AuditTrail>
    {
        public void Configure(EntityTypeBuilder<AuditTrail> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property(a => a.TableName)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(a => a.Entity)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(a => a.DateTime)
                .IsRequired();

            builder.Property(a => a.KeyValues)
                .IsRequired();
        }
    }
}