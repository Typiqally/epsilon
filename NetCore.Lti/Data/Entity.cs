using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetCore.Lti.Data;

public abstract record Entity<TKey> : IEntity<TKey>
{
    protected Entity(TKey id)
    {
        Id = id;
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public TKey? Id { get; set; }

    object? IEntity.Id
    {
        get => Id;
        init => Id = value is TKey key ? key : default;
    }
}