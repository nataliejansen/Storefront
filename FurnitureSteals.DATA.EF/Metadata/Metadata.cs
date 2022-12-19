using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FurnitureSteals.DATA.EF.Models
{

    public class CategoryMetaData
    {
        [Required(ErrorMessage = "Category is required")]
        [StringLength(50, ErrorMessage = "*Cannot exceed 50 characters*")]
        [Display(Name = "Category")]
        public string? CategoryName { get; set; }
    }

    public class ManufacturerMetaData
    {
        [Required(ErrorMessage = "Manufacturer is required")]
        [StringLength(50, ErrorMessage = "*Cannot exceed 50 characters*")]
        [Display(Name = "Manufacturer")]
        public string CategoryName { get; set; } = null!;
    }

    public class ProductMetaData
    {
        [Required(ErrorMessage = "Collection Name is required")]
        [StringLength(50, ErrorMessage = "*Cannot exceed 50 characters*")]
        [Display(Name = "Collection Name")]
        public string CollectionName { get; set; } = null!;

        [StringLength(75, ErrorMessage = "*Cannot exceed 75 characters*")]
        [Display(Name = "Product Description")]
        public string? ProductDescription { get; set; }

        [Required(ErrorMessage = "Manufacturer ID is required")]
        [Display(Name = "Manufacturer ID")]
        public int ManufacturerId { get; set; }

        [Required(ErrorMessage = "Product Status is required")]
        [Display(Name = "Product Status")]
        public int ProductStatus { get; set; }

        [Required(ErrorMessage = "Category ID is required")]
        [Display(Name = "Category ID")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Product Price is required")]
        [DisplayFormat(DataFormatString = "{0:c}", ApplyFormatInEditMode = false)]
        [Range(0, (double)decimal.MaxValue)]
        [Display(Name = "Price")]
        public decimal Price { get; set; }

        public int? Quantity { get; set; }

        [Display(Name = "Image")]
        public string? ProductImage { get; set; }
    }

    public class ProductStatusMetaData
    {
        [Required(ErrorMessage = "Product Status is required")]
        [StringLength(50, ErrorMessage = "*Cannot exceed 50 characters*")]
        [Display(Name = "Product Status")]
        public string StatusName { get; set; } = null!;
    }

    
}
