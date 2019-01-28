using System.ComponentModel.DataAnnotations;
using ASPNetCoreWithKendoUI.ModelBinders;
using Microsoft.AspNetCore.Mvc;

namespace ASPNetCoreWithKendoUI.Models.Product
{
    [ModelBinder(BinderType = typeof(ProductModelBinder))]
    public class ProductModel : BaseModel
    {
        [Required]
        [Display(Name = "产品名称")]
        public string ProductName { get; set; }
    }
}
