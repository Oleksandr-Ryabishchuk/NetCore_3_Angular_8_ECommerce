using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using BigProject_V_2.DataAccessLayer.Entities;

namespace BigProject_V_2.BusinessLayer.Interfaces
{
    public interface IProductManager
    {
        Task<IEnumerable<ProductModel>> GetAllProducts();
        Task<ProductModel> AddProduct(ProductModel model);
        Task<ProductModel> UpdateProduct(int id, ProductModel model);
        Task<IEnumerable<ProductModel>> DeleteProduct(int id);
    }
}
