namespace ProSolution.Core.Entities.Commons
{
    public abstract class BaseEntity : AuditedEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

    }
}
