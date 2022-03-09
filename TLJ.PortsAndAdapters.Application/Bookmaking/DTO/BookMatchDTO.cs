using System;

namespace TLJ.PortsAndAdapters.Application.Bookmaking.DTO
{
    public class BookMatchDTO
    {
        public Guid MatchId { get; set; }
        
        public Guid UserId { get; set; }
        
        public decimal Stake { get; set; }
        
        public string? Currency { get; set; }
        
        public string? BookType { get; set; }
    }
}