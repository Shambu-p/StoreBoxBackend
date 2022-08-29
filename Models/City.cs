using System;
using System.Collections.Generic;

namespace StoreBackend.Models
{
    public partial class City
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Country { get; set; } = null!;
        public int Population { get; set; }
    }
}
