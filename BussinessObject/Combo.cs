using System;
using System.Collections.Generic;

namespace BussinessObject;

public partial class Combo
{
    public int ComboId { get; set; }

    public string Name { get; set; } = null!;

    public decimal Price { get; set; }

    public string? ImageUrl { get; set; }

    public int? Product1Id { get; set; }

    public int? Product2Id { get; set; }

    public int? Topping1Id { get; set; }

    public int? Topping2Id { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual Product? Product1 { get; set; }

    public virtual Product? Product2 { get; set; }

    public virtual Topping? Topping1 { get; set; }

    public virtual Topping? Topping2 { get; set; }
}
