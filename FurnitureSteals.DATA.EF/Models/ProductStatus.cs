using System;
using System.Collections.Generic;

namespace FurnitureSteals.DATA.EF.Models
{
    public partial class ProductStatus
    {
        public ProductStatus()
        {
            Products = new HashSet<Product>();
        }

        public int StatusId { get; set; }
        public string StatusName { get; set; } = null!;

        public virtual ICollection<Product> Products { get; set; }
    }
}
