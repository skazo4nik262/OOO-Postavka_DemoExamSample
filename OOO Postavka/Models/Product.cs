using System;
using System.Collections.Generic;

namespace OOO_Postavka.Models;

public partial class Product
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public string? Code { get; set; }

    public string? Units { get; set; }

    public virtual ICollection<Factoring> FactoringMaterials { get; set; } = new List<Factoring>();

    public virtual ICollection<Factoring> FactoringProducts { get; set; } = new List<Factoring>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<PriceList> PriceLists { get; set; } = new List<PriceList>();

    public virtual ICollection<Specification> SpecificationMaterials { get; set; } = new List<Specification>();

    public virtual ICollection<Specification> SpecificationProducts { get; set; } = new List<Specification>();
}
