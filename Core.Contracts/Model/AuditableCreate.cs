using Core.Contracts.Model.Interfaces;

namespace Core.Contracts.Model
{
    public class AuditableCreate : IAuditableCreate
    {
        public string UserAdded { get; set; } = string.Empty;
        public DateTime? DateAdded { get; set; }
    }
}
