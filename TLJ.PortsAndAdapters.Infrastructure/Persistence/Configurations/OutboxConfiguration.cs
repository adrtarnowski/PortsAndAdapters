using Kitbag.Builder.Outbox.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TLJ.PortsAndAdapters.Infrastructure.Persistence.Configurations
{
    public class OutboxConfiguration : IEntityTypeConfiguration<OutboxMessage>
    {
        public void Configure(EntityTypeBuilder<OutboxMessage> builder)
        {
            builder.ToTable("OutboxMessages");
            builder.HasKey(b => b.Id);
            builder.Property(b => b.Id).ValueGeneratedNever();

            builder.Property(o => o.Payload)
                .IsRequired()
                .HasMaxLength(4000);;

            builder.Property(o => o.Type)
                .IsRequired()
                .HasMaxLength(300);

            builder.Property(o => o.Discriminator)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(10);
        }
    }
}