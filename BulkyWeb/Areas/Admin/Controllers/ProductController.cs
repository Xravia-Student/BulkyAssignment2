using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            List<Product> objProductList = _unitOfWork.Product
                .GetAll(includeProperties: "Category").ToList();
            return View(objProductList);
        }

        public IActionResult Create()
        {
            ProductVM productVM = new()
            {
                CategoryList = _unitOfWork.Category.GetAll()
                    .Select(u => new SelectListItem
                    {
                        Text = u.Name,
                        Value = u.Id.ToString()
                    }),
                Product = new Product()
            };
            return View(productVM);
        }

        [HttpPost]
        public IActionResult Create(ProductVM productVM)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Product.Add(productVM.Product!);
                _unitOfWork.Save();
                TempData["success"] = "Product created successfully";
                return RedirectToAction("Index");
            }
            productVM.CategoryList = _unitOfWork.Category.GetAll()
                .Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
            return View(productVM);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0) return NotFound();
            ProductVM productVM = new()
            {
                Product = _unitOfWork.Product.Get(u => u.Id == id),
                CategoryList = _unitOfWork.Category.GetAll()
                    .Select(u => new SelectListItem
                    {
                        Text = u.Name,
                        Value = u.Id.ToString()
                    })
            };
            if (productVM.Product == null) return NotFound();
            return View(productVM);
        }

        [HttpPost]
        public IActionResult Edit(ProductVM productVM)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Product.Update(productVM.Product!);
                _unitOfWork.Save();
                TempData["success"] = "Product updated successfully";
                return RedirectToAction("Index");
            }
            productVM.CategoryList = _unitOfWork.Category.GetAll()
                .Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
            return View(productVM);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0) return NotFound();
            Product? productFromDb = _unitOfWork.Product.Get(u => u.Id == id);
            if (productFromDb == null) return NotFound();
            return View(productFromDb);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Product? obj = _unitOfWork.Product.Get(u => u.Id == id);
            if (obj == null) return NotFound();

            if (!string.IsNullOrEmpty(obj.ImageUrl))
            {
                var oldImagePath = Path.Combine(
                    _webHostEnvironment.WebRootPath,
                    obj.ImageUrl.TrimStart('\\'));
                if (System.IO.File.Exists(oldImagePath))
                    System.IO.File.Delete(oldImagePath);
            }

            _unitOfWork.Product.Remove(obj);
            _unitOfWork.Save();
            TempData["success"] = "Product deleted successfully";
            return RedirectToAction("Index");
        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> objProductList = _unitOfWork.Product
                .GetAll(includeProperties: "Category").ToList();
            return Json(new { data = objProductList });
        }
        #endregion
    }
}