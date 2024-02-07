namespace Core.Abstractions
{
    public interface IServiceCrud<T> : IService<T>
    {
        IGenericResult InsertOrUpdate(T model);

        IGenericResult Delete(int id);
    }
}
