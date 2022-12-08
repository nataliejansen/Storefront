using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema; //Added for access to Data Annotations
using Microsoft.AspNetCore.Mvc;

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

    }

    [ModelMetadataType(typeof(ProductStatusMetaData))]
    public partial class ProductStatus
    {

    }






}
