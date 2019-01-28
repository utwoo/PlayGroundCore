using System.Collections.Generic;
using System.Linq;
using ASPNetCoreWithKendoUI.Models.Product;

namespace ASPNetCoreWithKendoUI.Factories
{
    public class ProductModelFactory
    {
        private static readonly List<ProductModel> _list = CreateProductModels();

        private static List<ProductModel> CreateProductModels()
        {
            return new List<ProductModel>
            {
                 new ProductModel{ Id = 1,  ProductName = "Switch" },
                 new ProductModel{ Id = 2,  ProductName = "WII" },
                 new ProductModel{ Id = 3,  ProductName = "Super Famicon" },
                 new ProductModel{ Id = 4,  ProductName = "Famicon" }
            };
        }

        public static ProductListModel GetAllProducts(ProductSearchModel searchModel)
        {
            var model = new ProductListModel
            {
                //fill in model values from the entity
                Data = _list.Skip((searchModel.Page - 1) * searchModel.PageSize).Take(searchModel.PageSize),
                Total = _list.Count
            };

            return model;
        }
    }
}
