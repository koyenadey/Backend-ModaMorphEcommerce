using ECommWeb.Core.src.Common;
using ECommWeb.Business.src.DTO;
using Microsoft.AspNetCore.Http;

namespace ECommWeb.Business.src.ServiceAbstract.EntityServiceAbstract;

public interface IProductService
{
    Task<int> GetProductsCount(string SearchKey);
    Task<bool> CheckIfEmailExists(string email);
    Task<int> GetProductsCountByCategory(Guid categoryId);
    Task<IEnumerable<ProductReadDTO>> GetAllProductsAsync(QueryOptions options);
    Task<ProductReadDTO> GetProductById(Guid id);
    Task<ProductReadDTO> CreateProduct(ProductCreateDTO prod);
    Task<ProductReadDTO> UpdateProduct(Guid id, ProductUpdateDTO prodImg);
    Task<ProductReadDTO> DeleteProduct(Guid id);
    Task<IEnumerable<ProductReadDTO>> GetAllProductsByCategoryAsync(Guid categoryId, QueryOptions options);
    Task<IEnumerable<ProductReadDTO>> GetMostPurchasedProductsAsync(int top);
}
