using System;

namespace Core.Contracts
{
    public interface IAuditableCreateUpdate : IAuditableCreate
    {
        string? UserUpdated { get; set; }

        DateTime? DateUpdated { get; set; }
    }
}
