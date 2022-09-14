using System;
using System.Collections.Generic;

namespace StoreBackend.Models
{
    public partial class StoreItem
    {
        public uint StoreId { get; set; }
        public uint ItemId { get; set; }
        public uint TotalAmount { get; set; }
        public uint UnboxedAmount { get; set; }

        public virtual Item Item { get; set; } = null!;
        public Store Store { get; set; }
    }
}
