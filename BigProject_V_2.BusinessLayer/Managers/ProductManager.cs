using BigProject_V_2.BusinessLayer.Interfaces;
using BigProject_V_2.DataAccessLayer.Entities;
using BigProject_V_2.DataAccessLayer.UnitOfWork;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigProject_V_2.BusinessLayer.Managers
{
    public class ProductManager : IProductManager
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProductManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ProductModel> AddProduct(ProductModel model)
        {
            var product = new ProductModel
            {
                Name = model.Name,
                Description = model.Description,
                ImageUrl = model.ImageUrl,
                OutOfStock = model.OutOfStock,
                Price = model.Price
            };

            await _unitOfWork.GetRepository<ProductModel>().AddAsync(product);
            await _unitOfWork.SaveAsync();
            return product;
        } 
        public async Task<IEnumerable<ProductModel>> GetAllProducts()
        {
            var products = await _unitOfWork.GetRepository<ProductModel>().GetAllAsync();
            return products;
        }
        public async Task<ProductModel> UpdateProduct(int id, ProductModel model)
        {
            var findProduct = await _unitOfWork.GetRepository<ProductModel>().GetAsync(id);

            findProduct.Name = model.Name;
            findProduct.Description = model.Description;
            findProduct.OutOfStock = model.OutOfStock;
            findProduct.ImageUrl = model.ImageUrl;
            findProduct.Price = model.Price;

            var updated = await _unitOfWork.GetRepository<ProductModel>().UpdateAsync(findProduct, id);
            await _unitOfWork.SaveAsync();
            return updated;
        }
        public async Task<IEnumerable<ProductModel>> DeleteProduct(int id)
        {
            var product = await _unitOfWork.GetRepository<ProductModel>().GetAsync(id);
            _unitOfWork.GetRepository<ProductModel>().Delete(product);
            await _unitOfWork.SaveAsync();
            return await GetAllProducts();
        }
    }
}
