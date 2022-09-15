using System;
using System.Collections.Generic;

namespace StoreBackend.Models
{
    public partial class Item
    {
        public uint Id { get; set; }
        public string Name { get; set; } = null!;
        public double Price { get; set; }
        
        public List<StoreItem> StoreItems {get; set;}
    }
}
