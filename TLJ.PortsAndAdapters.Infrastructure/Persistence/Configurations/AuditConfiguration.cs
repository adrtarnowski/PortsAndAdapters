using Kitbag.Persistence.EntityFramework.Audit.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TLJ.PortsAndAdapters.Infrastructure.Persistence.Configurations
{
    public class AuditConfiguration : IEntityTypeConfiguration<Audit>
    {
        public void Configure(EntityTypeBuilder<Audit> builder)
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

            builder.Property(a => a.ChangeContext)
                .HasMaxLength(255);

            builder.Property(a => a.CorrelationId)
                .HasMaxLength(255);

            builder.Property(a => a.KeyValues)
                .IsRequired();
        }
    }
}