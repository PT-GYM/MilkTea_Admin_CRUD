using System;
using System.Collections.Generic;

namespace BussinessObject;

public partial class Product
{
    public int ProductId { get; set; }

    public string Name { get; set; } = null!;

    public decimal Price { get; set; }

    public int? CategoryId { get; set; }

    public string? ImageUrl { get; set; }

    public int Stock { get; set; }

    public virtual Category? Category { get; set; }

    public virtual ICollection<Combo> ComboProduct1s { get; set; } = new List<Combo>();

    public virtual ICollection<Combo> ComboProduct2s { get; set; } = new List<Combo>();

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
