namespace Mgls.Domain.Entities;

public class Player
{
    public Guid Id { get; set; }
    public string DisplayName { get; set; } = string.Empty;
    public DateTimeOffset CreatedAt { get; set; }
}
