using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TLJ.PortsAndAdapters.Core.Domain.Book;

namespace TLJ.PortsAndAdapters.Infrastructure.Persistence.Configurations
{
    public class BookMatchConfiguration : IEntityTypeConfiguration<BookMatch>
    {
        public void Configure(EntityTypeBuilder<BookMatch> builder)
        {
            builder.ToTable("BookMatches");
            builder.HasKey(a => a.Id).IsClustered(false);
            builder.Property(s => s.Id).ValueGeneratedNever();
            builder.Property<long>("ClusteredId").UseIdentityColumn();

            builder.Property(a => a.MatchId)
                .IsRequired()
                .HasMaxLength(36);

            builder.Property(a => a.UserId)
                .IsRequired()
                .HasMaxLength(36);

            builder.Property(a => a.BookType)
                .IsRequired();

            builder.Property(a => a.Value)
                .IsRequired();
            
            builder.Property(a => a.Currency)
                .IsRequired();
            
            builder.Property(a => a.CreateDate)
                .IsRequired();
        }
    }
}