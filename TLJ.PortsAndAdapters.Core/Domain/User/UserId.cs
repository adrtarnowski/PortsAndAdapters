using System;
using Kitbag.Builder.Core.Domain;

namespace TLJ.PortsAndAdapters.Core.Domain.User;

public class UserId : TypedIdValueBase
{
    public UserId(Guid value) : base(value) { }
        
    public static UserId Default => new UserId(default);
    public static UserId Generate() => new UserId(Guid.NewGuid());
}