using System;

namespace Core.Abstractions
{
    public interface IAuditableCreate
    {
        string UserAdded { get; set; }

        DateTime DateAdded { get; set; }
    }
}
