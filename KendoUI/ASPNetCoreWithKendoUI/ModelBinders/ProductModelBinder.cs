using System.Threading.Tasks;
using ASPNetCoreWithKendoUI.Models.Product;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ASPNetCoreWithKendoUI.ModelBinders
{
    public class ProductModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var id = bindingContext.ValueProvider.GetValue("id");
            var model = new ProductModel();

            if (id.Values.Count > 0)
            {
                model.Id = int.Parse(id.FirstValue);
            }

            bindingContext.Result = ModelBindingResult.Failed();

            return Task.CompletedTask;
        }
    }
}
