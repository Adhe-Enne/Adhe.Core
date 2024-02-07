namespace Core.Abstractions
{
    public interface IConfigEntity
    {
        string Name { get; }

        string Description { get; }

        string ValueAsString { get; }

    }

    public interface IConfigEntity<T> : IConfigEntity
    {
        T Value { get; set; }

        void SetValue(string value);

        //string GetValueAsString();
    }
}
