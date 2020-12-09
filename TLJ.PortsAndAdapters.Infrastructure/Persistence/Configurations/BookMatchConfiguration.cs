using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TLJ.PortsAndAdapters.Core.Domain.Book;

namespace TLJ.PortsAndAdapters.Infrastructure.Persistence.Configurations
{
    public class BookMatchConfiguration : IEntityTypeConfiguration<BookMatch>
    {
        public void Configure(EntityTypeBuilder<BookMatch> builder)
        {
            builder.HasKey(a => a.Id);

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