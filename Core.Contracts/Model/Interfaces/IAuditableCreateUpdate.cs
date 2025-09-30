namespace Core.Contracts.Model.Interfaces
{
    public interface IAuditableCreateUpdate : IAuditableCreate
    {
        string? UserUpdated { get; set; }

        DateTime? DateUpdated { get; set; }
    }
}
