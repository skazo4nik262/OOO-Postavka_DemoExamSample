using System;
using System.Collections.Generic;

namespace OOO_Postavka.Models;

public partial class Factoring
{
    public int Id { get; set; }

    public int? ProductId { get; set; }

    public int? MaterialId { get; set; }

    public int? ProductCount { get; set; }

    public decimal? MaterialCount { get; set; }

    public virtual Product? Material { get; set; }

    public virtual Product? Product { get; set; }
}
