using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Shopping.Data.Repository.IRepository;
using Shopping.Model;
using Shopping.Model.ViewModels;

namespace ShoppingApp_LA.Areas.Admin.Controllers
{

    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> products = _unitOfWork.Product.GetAll();
            return View(products);
        }


        public IActionResult Crup(int? id)
        {
            ProductViewModel productVM = new()
            {
                Product = new(),
                CategoryList = _unitOfWork.Category.GetAll().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString(),
                })
            };

            if (id == null || id <= 0)
            {
                return View(productVM);
            }

            productVM.Product = _unitOfWork.Product.GetFirstOrDefault(x => x.Id == id);

            if (productVM.Product == null)
            {
                return View(productVM);
            }

            return View(productVM);
        }

        [HttpPost]
        public IActionResult Crup(ProductViewModel productVM, IFormFile file)
        {
            string wwwRootPath = _hostEnvironment.WebRootPath;
            if (file != null)
            {
                string fileName = Guid.NewGuid().ToString();
                var uploadRoot = Path.Combine(wwwRootPath, @"img\products");
                var extension = Path.GetExtension(file.FileName);

                if (productVM.Product.Picture != null)
                {
                    var oldPicPath = Path.Combine(wwwRootPath, productVM.Product.Picture);
                    if (System.IO.File.Exists(oldPicPath))
                    {
                        System.IO.File.Delete(oldPicPath);
                    }

                }

                using (var fileStream = new FileStream(Path.Combine(uploadRoot, fileName + extension),
                    FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }

                productVM.Product.Picture = @"\img\products\" + fileName + extension;
            }


            if (productVM.Product.Id <= 0)
            {
                _unitOfWork.Product.Add(productVM.Product);
            }
            else
            {
                _unitOfWork.Product.Update(productVM.Product);
            }
            _unitOfWork.Save();

            return RedirectToAction(nameof(Index));

        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id <= 0)
            {
                return NotFound();
            }

            var product = _unitOfWork.Product.GetFirstOrDefault(x => x.Id == id);
            _unitOfWork.Product.Remove(product);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

    }
}
