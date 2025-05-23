﻿using System;
using System.Collections.Generic;

namespace BussinessObject;

public partial class Order
{
    public int OrderId { get; set; }

    public int? UserId { get; set; }

    public DateTime? OrderDate { get; set; }

    public decimal TotalAmount { get; set; }

    public string PhoneNumber { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string Status { get; set; } = null!;

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual User? User { get; set; }
}
