namespace Core.SharedServices
{
    public interface IServiceCrud<T> : Core.SharedServices.IGenericService<T> where T : Core.Contracts.Model.BaseEntity
    {
        Task InsertOrUpdate(T model);

        Task Delete(int id);
    }
}
