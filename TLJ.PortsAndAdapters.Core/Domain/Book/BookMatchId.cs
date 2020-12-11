using System;
using Kitbag.Builder.Core.Domain;

namespace TLJ.PortsAndAdapters.Core.Domain.Book
{
    public class BookMatchId : TypedIdValueBase
    {
        public BookMatchId(Guid value) : base(value) { }
        
        public static BookMatchId Default => new BookMatchId(default);
        public static BookMatchId Generate() => new BookMatchId(Guid.NewGuid());
    }
}