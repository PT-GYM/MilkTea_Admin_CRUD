using System;
using System.Collections.Generic;

namespace BussinessObject;

public partial class OrderDetail
{
    public int OrderDetailId { get; set; }

    public int? OrderId { get; set; }

    public int? ProductId { get; set; }

    public int? ComboId { get; set; }

    public int Quantity { get; set; }

    public decimal SubTotal { get; set; }

    public string? ToppingIds { get; set; }

    public virtual Combo? Combo { get; set; }

    public virtual Order? Order { get; set; }

    public virtual Product? Product { get; set; }
}
