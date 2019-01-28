using ASPNetCoreWithKendoUI.Factories;
using ASPNetCoreWithKendoUI.Models.Product;
using Microsoft.AspNetCore.Mvc;

namespace ASPNetCoreWithKendoUI.Controllers
{
    public class ProductController : BaseController
    {
        public virtual IActionResult Index()
        {
            return RedirectToAction("List");
        }

        public virtual IActionResult List()
        {
            //prepare search model
            var searchModel = new ProductSearchModel();
            return View(searchModel);
        }

        [HttpPost]
        public virtual IActionResult List(ProductSearchModel searchModel)
        {
            var model = ProductModelFactory.GetAllProducts(searchModel);
            return Json(model);
        }

        public virtual IActionResult Create()
        {
            var model = new ProductModel();
            return View(model);
        }

        [HttpPost]
        public virtual IActionResult Create(ProductModel model)
        {
            var result = model;
            return View(result);
        }

        #region Api

        [Route("~/Api/Product/List")]
        public virtual IActionResult ApiList(ProductSearchModel searchModel)
        {
            var model = ProductModelFactory.GetAllProducts(searchModel);
            return Json(model);
        }

        #endregion
    }
}
