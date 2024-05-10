namespace ECommWeb.Core.src.Entity;

public class BaseEntity : Timestamp
{
    public Guid Id { get; set; }
    protected BaseEntity()
    {
        Id = Guid.NewGuid();
    }
}