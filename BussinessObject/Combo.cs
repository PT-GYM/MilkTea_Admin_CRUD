using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BussinessObject;

public partial class Combo
{
    public int ComboId { get; set; }

    public string Name { get; set; } = null!;

    public decimal Price { get; set; }

    public string? ImageUrl { get; set; }

    public string? Description { get; set; }

    public string ProductIds { get; set; } = null!;

    public string? ToppingIds { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    [NotMapped] 
    public List<string> ProductNames { get; set; }

    [NotMapped]  
    public List<string> ToppingNames { get; set; }
}
