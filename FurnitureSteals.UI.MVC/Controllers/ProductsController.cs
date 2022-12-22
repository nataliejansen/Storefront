using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FurnitureSteals.DATA.EF.Models;
using Microsoft.AspNetCore.Authorization;
using System.Drawing;
using FurnitureSteals.UI.MVC.Utilities;
using Microsoft.AspNetCore.Hosting;

namespace FurnitureSteals.UI.MVC.Controllers
{
    public class ProductsController : Controller
    {
        private readonly storefrontContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductsController(storefrontContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: NewProducts
        public async Task<IActionResult> Index()
        {
            var storefrontContext = _context.Products.Include(p => p.Category).Include(p => p.Manufacturer).Include(p => p.ProductStatusNavigation);
            return View(await storefrontContext.ToListAsync());
        }
        [AllowAnonymous]
        public async Task<IActionResult> TiledProducts(int categoryId = 0, int page = 1)
        {

            var products = _context.Products.Include(p => p.Category).Include(p => p.Manufacturer).Include(p => p.ProductStatusNavigation);

            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName");
            ViewBag.Category = 0; //Added this variable to persist (save) the selected Category

            if (categoryId != 0)
            {
                products = (Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<Product, ProductStatus?>)products.Where(p => p.CategoryId == categoryId).ToList();

                //Repopulate the dropdown with the current category selected
                ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", categoryId);
                ViewBag.Category = categoryId;
            }

            return View(await products.ToListAsync());
        }

            // GET: NewProducts/Details/5
            public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Manufacturer)
                .Include(p => p.ProductStatusNavigation)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: NewProducts/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryId");
            ViewData["ManufacturerId"] = new SelectList(_context.Manufacturers, "ManufacturerId", "CategoryName");
            ViewData["ProductStatus"] = new SelectList(_context.ProductStatuses, "StatusId", "StatusName");
            return View();
        }

        // POST: NewProducts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("ProductId,CollectionName,ProductDescription,ManufacturerId,ProductStatus,CategoryId,Price,Quantity,ProductImage,Image")] Product product)
        {
            if (ModelState.IsValid)
            {
                #region File Upload - CREATE
                //Check to see if a file was uploaded
                if (product.Image != null)
                {
                    //Check the file type 
                    //- retrieve the extension of the uploaded file
                    string ext = Path.GetExtension(product.Image.FileName);

                    //- Create a list of valid extensions to check against
                    string[] validExts = { ".jpeg", ".jpg", ".gif", ".png" };

                    //- verify the uploaded file has an extension matching one of the extensions in the list above
                    //- AND verify file size will work with our .NET app
                    if (validExts.Contains(ext.ToLower()) && product.Image.Length < 4_194_303)
                    {
                        //Generate a unique filename
                        product.ProductImage = Guid.NewGuid() + ext;

                        //Save the file to the web server (here, saving to wwwroot/images)
                        //To access wwwroot, add a property to the controller for the _webHostEnvironment (see the top of this class for our example)
                        //Retrieve the path to wwwroot
                        string webRootPath = _webHostEnvironment.WebRootPath;
                        //variable for the full image path --> this is where we will save the image
                        string fullImagePath = webRootPath + "/images/";

                        //Create a MemoryStream to read the image into the server memory
                        using (var memoryStream = new MemoryStream())
                        {
                            await product.Image.CopyToAsync(memoryStream);//transfer file from the request to server memory
                            using (var img = Image.FromStream(memoryStream))//add a using statement for the Image class (using System.Drawing)
                            {
                                //now, send the image to the ImageUtility for resizing and thumbnail creation
                                //items needed for the ImageUtility.ResizeImage()
                                //1) (int) maximum image size
                                //2) (int) maximum thumbnail image size
                                //3) (string) full path where the file will be saved
                                //4) (Image) an image
                                //5) (string) filename
                                int maxImageSize = 500;//in pixels
                                int maxThumbSize = 100;

                                ImageUtility.ResizeImage(fullImagePath, product.ProductImage, img, maxImageSize, maxThumbSize);
                                //myFile.Save("path/to/folder", "filename"); - how to save something that's NOT an image

                            }
                        }
                    }
                }
                else
                {
                    //If no image was uploaded, assign a default filename
                    //Will also need to download a default image and name it 'noimage.png' -> copy it to the /images folder
                    product.ProductImage = "noimage.png";
                }

                #endregion
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryId", product.CategoryId);
            ViewData["ManufacturerId"] = new SelectList(_context.Manufacturers, "ManufacturerId", "CategoryName", product.ManufacturerId);
            ViewData["ProductStatus"] = new SelectList(_context.ProductStatuses, "StatusId", "StatusName", product.ProductStatus);
            return View(product);
        }

        // GET: NewProducts/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryId", product.CategoryId);
            ViewData["ManufacturerId"] = new SelectList(_context.Manufacturers, "ManufacturerId", "CategoryName", product.ManufacturerId);
            ViewData["ProductStatus"] = new SelectList(_context.ProductStatuses, "StatusId", "StatusName", product.ProductStatus);
            return View(product);
        }

        // POST: NewProducts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,CollectionName,ProductDescription,ManufacturerId,ProductStatus,CategoryId,Price,Quantity,ProductImage,Image")] Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryId", product.CategoryId);
            ViewData["ManufacturerId"] = new SelectList(_context.Manufacturers, "ManufacturerId", "CategoryName", product.ManufacturerId);
            ViewData["ProductStatus"] = new SelectList(_context.ProductStatuses, "StatusId", "StatusName", product.ProductStatus);
            return View(product);
        }

        // GET: NewProducts/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Manufacturer)
                .Include(p => p.ProductStatusNavigation)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: NewProducts/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Products == null)
            {
                return Problem("Entity set 'storefrontContext.Products'  is null.");
            }
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
          return _context.Products.Any(e => e.ProductId == id);
        }
    }
}
