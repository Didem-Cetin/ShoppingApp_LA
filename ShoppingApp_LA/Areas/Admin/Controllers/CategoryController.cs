using Microsoft.AspNetCore.Mvc;
using Shopping.Data.Repository.IRepository;
using Shopping.Model;

namespace ShoppingApp_LA.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Category> categories = _unitOfWork.Category.GetAll();
            return View(categories);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Add(category);
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if(id== null || id <= 0)
            {
                return NotFound();
            }
            var category =_unitOfWork.Category.GetFirstOrDefault(x => x.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        [HttpPost]
        public IActionResult Edit(Category category) 
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Update(category);
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }

            return View(category);
        }

        public IActionResult Delete(int id)
        {
            if(id==null || id<=0)
            {
                return NotFound();
            }

            var category=_unitOfWork.Category.GetFirstOrDefault(x=>x.Id == id);
            if (category==null)
            {
                return NotFound();
            }

            _unitOfWork.Category.Remove(category);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
    }
}
