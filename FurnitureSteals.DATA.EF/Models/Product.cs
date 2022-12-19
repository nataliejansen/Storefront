using System;
using System.Collections.Generic;

namespace FurnitureSteals.DATA.EF.Models
{
    public partial class Product
    {
        public int ProductId { get; set; }
        public string CollectionName { get; set; } = null!;
        public string? ProductDescription { get; set; }
        public int ManufacturerId { get; set; }
        public int ProductStatus { get; set; }
        public int CategoryId { get; set; }
        public decimal Price { get; set; }
        public int? Quantity { get; set; }
        public string? ProductImage { get; set; }

        public virtual Category Category { get; set; } = null!;
        public virtual Manufacturer Manufacturer { get; set; } = null!;
        public virtual ProductStatus ProductStatusNavigation { get; set; } = null!;
    }
}
