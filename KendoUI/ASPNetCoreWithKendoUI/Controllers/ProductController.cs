using System.Threading;
using ASPNetCoreWithKendoUI.Factories;
using ASPNetCoreWithKendoUI.Models.Product;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace ASPNetCoreWithKendoUI.Controllers
{
    public class ProductController : BaseController
    {
        [ViewContext]
        protected ViewContext viewContext { get; set; }

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
        public virtual IActionResult Update(ProductModel model)
        {
            ProductModelFactory.Update(model);
            return Ok(model);
        }

        public virtual IActionResult DetailContent(int id)
        {
            var model = ProductModelFactory.GetProductsById(id);
            return PartialView(model);
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
