using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TLJ.PortsAndAdapters.Core.Domain.User;

namespace TLJ.PortsAndAdapters.Infrastructure.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");
            builder.HasKey(s => s.Id).IsClustered(false);
            builder.Property(s => s.Id).ValueGeneratedNever();
            builder.Property<long>("ClusteredId").UseIdentityColumn();
            builder.HasIndex("ClusteredId")
                .IsClustered();

            builder.Property(a => a.FullDomainName)
                .IsRequired()
                .HasMaxLength(36);

            builder.Property(a => a.UserType)
                .IsRequired();
            
            builder.Property(a => a.UserStatus)
                .IsRequired();

            builder.Property(a => a.CreationDate)
                .IsRequired();
            
            builder.Property(a => a.LastUpdateDate)
                .IsRequired();
        }
    }
}