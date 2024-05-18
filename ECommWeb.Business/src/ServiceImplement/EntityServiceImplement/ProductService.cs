using AutoMapper;
using ECommWeb.Core.src.Common;
using ECommWeb.Core.src.Entity;
using ECommWeb.Core.src.RepoAbstract;
using ECommWeb.Business.src.DTO;
using ECommWeb.Business.src.ServiceAbstract.EntityServiceAbstract;
using ECommWeb.Business.src.Shared;

namespace ECommWeb.Business.src.ServiceImplement.EntityServiceImplement;

public class ProductService : IProductService
{
    private readonly IProductRepo _productRepo;
    private readonly ICategoryRepo _categoryRepo;
    private IMapper _mapper;

    public ProductService(IProductRepo productRepo, IMapper mapper, ICategoryRepo categoryRepo)
    {
        _productRepo = productRepo;
        _mapper = mapper;
        _categoryRepo = categoryRepo;
    }
    public async Task<IEnumerable<ProductReadDTO>> GetAllProductsAsync(QueryOptions options)
    {
        var products = await _productRepo.GetAllAsync(options);
        return _mapper.Map<IEnumerable<ProductReadDTO>>(products);
    }

    public async Task<ProductReadDTO> GetProductById(Guid id)
    {
        var product = await _productRepo.GetOneByIdAsync(id);
        if (product is not null)
        {
            return _mapper.Map<ProductReadDTO>(product);
        }
        else
        {
            throw CustomException.NotFoundException("Id does not exist");
        }
    }

    public async Task<IEnumerable<ProductReadDTO>> GetAllProductsByCategoryAsync(Guid categoryId, QueryOptions options)
    {
        var foundCategory = await _categoryRepo.GetOneByIdAsync(categoryId);
        if (foundCategory is not null)
        {
            var result = _productRepo.GetByCategory(categoryId, options);
            return _mapper.Map<IEnumerable<ProductReadDTO>>(result);
        }
        else
        {
            throw CustomException.NotFoundException("Category not found");
        }
    }

    public async Task<IEnumerable<ProductReadDTO>> GetMostPurchasedProductsAsync(int top)
    {
        var result = _productRepo.GetMostPurchased(top);
        return _mapper.Map<IEnumerable<Product>, IEnumerable<ProductReadDTO>>(result);
    }

    public async Task<ProductReadDTO> CreateProduct(ProductCreateDTO product)
    {
        var productToCreate = _mapper.Map<Product>(product);
        //new ProductCreateDTO().Transform(product);
        //_mapper.Map<ProductCreateDTO, Product>(product);
        var result = await _productRepo.CreateOneAsync(productToCreate);
        return _mapper.Map<ProductReadDTO>(result);
    }

    public async Task<ProductReadDTO> UpdateProduct(Guid id, ProductUpdateDTO product)
    {
        var foundItem = await _productRepo.GetOneByIdAsync(id);

        if (foundItem is not null && product is not null)
        {
            foundItem.Inventory = product.Inventory == 0 ? foundItem.Inventory : product.Inventory;
            foundItem.Price = (double)product.Price == default ? foundItem.Price : (double)product.Price;

            var result = await _productRepo.UpdateOneByIdAsync(foundItem);
            return _mapper.Map<ProductReadDTO>(result);
        }
        else
        {
            throw CustomException.NotFoundException("Id not found");
        }
    }

    public async Task<bool> DeleteProduct(Guid id)
    {
        var foundItem = await _productRepo.GetOneByIdAsync(id);
        if (foundItem is not null)
        {
            await _productRepo.DeleteOneByIdAsync(foundItem);
            return true;
        }
        else
        {
            return false;
        }
    }
}
