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
}
