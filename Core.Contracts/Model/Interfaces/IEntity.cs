namespace Core.Contracts.Model.Interfaces
{
    public interface IEntity: IAuditableCreateUpdate
    {
        Guid Id { get; set; }
        bool IsActive { get; set; }
    }
}
