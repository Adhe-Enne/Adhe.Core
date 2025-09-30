using Core.Contracts.Model.Interfaces;

namespace Core.Contracts.Model
{
    public class AuditableUpdate : AuditableCreate, IAuditableCreateUpdate
    {
        public string? UserUpdated { get; set; } = string.Empty;
        public DateTime? DateUpdated { get; set; }
    }
}
