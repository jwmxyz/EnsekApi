using System.ComponentModel.DataAnnotations;

namespace Ensek.DataAccess.DbModels;

public class EntityObject
{
    [Key] 
    public int Id { get; set; }

    public DateTimeOffset CreatedAt { get; set; } = DateTime.UtcNow;
}