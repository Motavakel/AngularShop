using Domain.Entities.Base;
using Domain.Entities.Identity;

namespace Domain.Entities.ProductEntity;

public class ProductType : BaseAuditableEntity, ICommands
{
    public string Title { get; set; }
    public bool IsActive { get; set; }
    public User User { get; set; }
}