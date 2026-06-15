using System;
using System.Collections.Generic;

namespace OOO_Postavka.Models;

public partial class Order
{
    public int? ProductId { get; set; }

    public decimal? Count { get; set; }

    public decimal? Sum { get; set; }

    public int Id { get; set; }

    public virtual Product? Product { get; set; }
}
