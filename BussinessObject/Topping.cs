using System;
using System.Collections.Generic;

namespace BussinessObject;

public partial class Topping
{
    public int ToppingId { get; set; }

    public string Name { get; set; } = null!;

    public decimal Price { get; set; }

    public string? ImageUrl { get; set; }

    public int Stock { get; set; }

    public virtual ICollection<Combo> ComboTopping1s { get; set; } = new List<Combo>();

    public virtual ICollection<Combo> ComboTopping2s { get; set; } = new List<Combo>();
}
