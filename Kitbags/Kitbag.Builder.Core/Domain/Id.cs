using System;

namespace Kitbag.Builder.Core.Domain
{
    public abstract class Id<TRawValue> where TRawValue : IEquatable<TRawValue>
    {
        public readonly TRawValue Value;

        protected Id(TRawValue value)
        {
            Value = value;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Value.Equals(((Id<TRawValue>)obj).Value);
        }

        public bool Equals(Id<TRawValue> other)
        {
            return Equals((object)other);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}