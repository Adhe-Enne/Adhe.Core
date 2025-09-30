namespace Core.Contracts.Model.Interfaces
{
    public interface IAuditableCreate
    {
        string UserAdded { get; set; }

        DateTime? DateAdded { get; set; }
    }
}
