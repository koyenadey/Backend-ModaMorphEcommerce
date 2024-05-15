using ECommWeb.Core.src.Common;
using ECommWeb.Business.src.DTO;

namespace ECommWeb.Business.src.ServiceAbstract.EntityServiceAbstract;

public interface IProductImageService
{
    Task<IEnumerable<ProductImageReadDTO>> GetAllProductImagesAsync(QueryOptions options);
    Task<ProductImageReadDTO> GetProductImageById(Guid id);
    Task<ProductImageReadDTO> CreateProductImage(ProductImageCreateDTO prodImg);
    Task<ProductImageReadDTO> UpdateProductImage(Guid id, ProductImageUpdateDTO prodImg);
    Task<bool> DeleteProductImage(Guid id);
}
