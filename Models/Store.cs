using System;
using System.Collections.Generic;

namespace StoreBackend.Models
{
    public partial class Store
    {
        public Store()
        {
            Boxes = new HashSet<Box>();
        }

        public uint Id { get; set; }
        public string Name { get; set; } = null!;
        public uint StoreKeeper { get; set; }

        public virtual User StoreKeeperNavigation { get; set; } = null!;
        public virtual ICollection<Box> Boxes { get; set; }
    }
}
