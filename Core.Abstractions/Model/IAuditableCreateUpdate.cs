using System;

namespace Core.Abstractions
{
    public interface IAuditableCreateUpdate : IAuditableCreate
    {
        string? UserUpdated { get; set; }

        DateTime? DateUpdated { get; set; }
    }
}
