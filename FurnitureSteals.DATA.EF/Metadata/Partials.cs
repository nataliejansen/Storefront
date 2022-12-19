using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureSteals.DATA.EF.Models//.Metadata
{
    [ModelMetadataType(typeof(CategoryMetaData))]
    public partial class Category
    {

    }

    [ModelMetadataType(typeof(ManufacturerMetaData))]
    public partial class Manufacturer
    {

    }    
    
    [ModelMetadataType(typeof(ProductMetaData))]
    public partial class Product
    {
        [NotMapped]
        public IFormFile? Image { get; set; }

    }

    [ModelMetadataType(typeof(ProductStatusMetaData))]
    public partial class ProductStatus
    {

    }






}
