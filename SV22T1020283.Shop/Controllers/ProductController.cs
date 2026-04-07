using Microsoft.AspNetCore.Mvc;
using SV22T1020283.BusinessLayers;
using SV22T1020283.Models.Catalog;
using SV22T1020283.Models.Common;
using SV22T1020283.Models.Shop;

namespace SV22T1020283.Shop.Controllers
{
    public class ProductController : Controller
    {
        private const int PAGE_SIZE = 12;

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1, int categoryId = 0,
            decimal? minPrice = null, decimal? maxPrice = null, string searchValue = "")
        {
            var input = new ProductSearchInput
            {
                Page = page,
                PageSize = PAGE_SIZE,
                CategoryID = categoryId,
                SupplierID = 0,
                MinPrice = minPrice,
                MaxPrice = maxPrice,
                SearchValue = searchValue
            };

            var products = await CatalogDataService.ListProductsAsync(input);

            // Lấy danh sách categories cho filter
            var categoriesInput = new PaginationSearchInput { Page = 1, PageSize = 100, SearchValue = "" };
            var categories = await CatalogDataService.ListCategoriesAsync(categoriesInput);

            var model = new ProductSearchViewModel
            {
                SearchInput = input,
                SearchResult = products,
                Categories = categories.DataItems
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var product = await CatalogDataService.GetProductAsync(id);
            if (product == null)
            {
                return RedirectToAction("Index");
            }

            // Lấy thêm thuộc tính và ảnh của sản phẩm
            var attributes = await CatalogDataService.ListAttributesAsync(id);
            var photos = await CatalogDataService.ListPhotosAsync(id);

            ViewBag.Attributes = attributes;
            ViewBag.Photos = photos;

            return View(product);
        }

        [HttpGet]
        public async Task<IActionResult> Search(string keyword, int page = 1)
        {
            var input = new ProductSearchInput
            {
                Page = page,
                PageSize = PAGE_SIZE,
                SearchValue = keyword ?? "",
                CategoryID = 0,
                SupplierID = 0
            };

            var products = await CatalogDataService.ListProductsAsync(input);
            return View("Index", products);
        }

        [HttpGet]
        public async Task<IActionResult> GetProductsByCategory(int categoryId, int page = 1)
        {
            return RedirectToAction("Index", new { categoryId, page });
        }
    }
}