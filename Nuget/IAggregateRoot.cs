namespace Nuget
{
    public interface IAggregateRoot<TKey>
    {
        TKey Id { get; set; }
    }
}