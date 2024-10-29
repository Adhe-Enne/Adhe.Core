using System;

namespace Core.Contracts
{
    public interface IAuditableCreate
    {
        string UserAdded { get; set; }

        DateTime DateAdded { get; set; }
    }
}
