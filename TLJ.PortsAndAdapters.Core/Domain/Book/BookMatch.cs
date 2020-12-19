using System;
using Kitbag.Builder.Core.Common;
using Kitbag.Builder.Core.Domain;

namespace TLJ.PortsAndAdapters.Core.Domain.Book
{
    public class BookMatch : IAggregateRoot<BookMatchId>
    {
        public BookMatchId Id { get; private set; }
        public Guid MatchId { get; private set; }
        
        public Guid UserId { get; private set;}
        
        public decimal Stake { get; private set;}
        
        public string Currency { get; private set;}
        
        public BookType BookType { get; private set;}

        public  DateTimeOffset CreateDate { get; private set;}

        public BookMatch(
            BookMatchId id,
            Guid matchId, 
            Guid userId,
            decimal stake,
            string currency,
            BookType bookType)
        {
            Id = id;
            MatchId = matchId;
            UserId = userId;
            Stake = stake;
            Currency = currency;
            BookType = bookType;
            CreateDate = SystemTime.OffsetNow();
        }
        
        public void ChangeBookValue(
            decimal stake,
            string currency,
            BookType bookType)
        {
            Stake = stake;
            Currency = currency;
            BookType = bookType;
        }
    }

    public enum BookType
    {
        Host,
        Guest,
        Draw
    }
}