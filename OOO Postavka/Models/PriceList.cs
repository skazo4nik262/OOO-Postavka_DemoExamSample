using System;
using System.Collections.Generic;

namespace OOO_Postavka.Models;

public partial class PriceList
{
    public int? ProductId { get; set; }

    public decimal? Price { get; set; }

    public int Id { get; set; }

    public virtual Product? Product { get; set; }
}
