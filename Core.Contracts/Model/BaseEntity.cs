using Core.Contracts.Model.Interfaces;

namespace Core.Contracts.Model
{
    public class BaseEntity : AuditableUpdate, IEntity
    {
        public Guid Id { get; set; }
        public bool IsActive { get; set; }
    }
}
