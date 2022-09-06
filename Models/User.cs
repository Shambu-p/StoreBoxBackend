using System;
using System.Collections.Generic;

namespace StoreBackend.Models
{
    public partial class User
    {
        public User()
        {
            Boxes = new HashSet<Box>();
            Stores = new HashSet<Store>();
        }

        public uint Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public byte Role { get; set; }

        public virtual ICollection<Box> Boxes { get; set; }
        public virtual ICollection<Store> Stores { get; set; }
    }
}
