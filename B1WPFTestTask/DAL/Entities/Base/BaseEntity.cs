using System.ComponentModel.DataAnnotations;

namespace B1WPFTestTask.DAL.Entities.Base;

public abstract class BaseEntity
{
    [Key]
    public int Id { get; init; }
}