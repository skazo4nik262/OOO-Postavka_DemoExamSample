using System;
using System.Collections.Generic;

namespace OOO_Postavka.Models;

public partial class Client
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Inn { get; set; }

    public string? Addres { get; set; }

    public string? Phone { get; set; }

    public bool? Saleman { get; set; }

    public bool? Buyer { get; set; }
}
