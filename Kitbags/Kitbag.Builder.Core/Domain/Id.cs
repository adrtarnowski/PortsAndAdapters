using System;

namespace Kitbag.Builder.Core.Domain
{
    public abstract class Id : IEquatable<Id>, IComparable
    {
        public readonly Guid Value;

        protected Id(Guid value)
        {
            Value = value;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Value.Equals(((Id)obj).Value);
        }

        public bool Equals(Id? other)
        {
            return Equals((object)other!);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public int CompareTo(object? obj)
        {
            if (obj is Id id)
                return String.CompareOrdinal(Value.ToString(), id.Value.ToString());
            return -1;
        }
        
        public static bool operator ==(Id? id1, Id? id2)
        {
            if (Equals(id1, null))
            {
                if (Equals(id2, null))
                    return true;
                return false;
            }
            return id1.Equals(id2);
        }

        public static bool operator !=(Id? id1, Id? id2)
        {
            return !(id1 == id2);
        }
    }
}