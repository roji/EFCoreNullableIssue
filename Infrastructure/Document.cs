namespace Infrastructure
{
    using Nuget;

    public class Document : IAggregateRoot<int>
    {
        public int Id { get; set; }
    }
}