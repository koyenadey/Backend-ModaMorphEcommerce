using AutoMapper;
using ECommWeb.Core.src.Common;
using ECommWeb.Core.src.Entity;
using ECommWeb.Core.src.RepoAbstract;
using ECommWeb.Business.src.DTO;
using ECommWeb.Business.src.ServiceAbstract.EntityServiceAbstract;
using ECommWeb.Business.src.Shared;

namespace Server.Service.src.ServiceImplement.AuthServiceImplement
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepo _categoryRepo;
        protected IMapper _mapper;
        public CategoryService(ICategoryRepo categoryRepo, IMapper mapper)
        {
            _categoryRepo = categoryRepo;
            _mapper = mapper;
        }
        public async Task<IEnumerable<CategoryReadDTO>> GetAllCategoriesAsync(QueryOptions options)
        {
            var categories = await _categoryRepo.GetAllAsync(options);
            return _mapper.Map<IEnumerable<CategoryReadDTO>>(categories);
        }
        public async Task<CategoryReadDTO> GetCategoryById(Guid id)
        {
            var result = await _categoryRepo.GetOneByIdAsync(id);
            if (result is not null)
            {
                return _mapper.Map<CategoryReadDTO>(result);
            }
            else
            {
                throw CustomException.NotFoundException("Id not found");
            }
        }
        public async Task<CategoryReadDTO> CreateCategory(CategoryCreateDTO category)
        {
            var categoryToBeCreated = _mapper.Map<Category>(category);
            var result = await _categoryRepo.CreateOneAsync(categoryToBeCreated);
            return _mapper.Map<CategoryReadDTO>(result);
        }
        public async Task<CategoryReadDTO> UpdateACategory(Guid id, CategoryUpdateDTO category)
        {
            var foundItem = await _categoryRepo.GetOneByIdAsync(id);
            if (foundItem is not null)
            {
                foundItem.Name = category.Name ?? foundItem.Name;
                foundItem.Image = category.Image ?? foundItem.Image;

                var result = await _categoryRepo.UpdateOneByIdAsync(foundItem);
                return _mapper.Map<CategoryReadDTO>(result);
            }
            else
            {
                throw CustomException.NotFoundException("Id not found");
            }
        }
        async Task<bool> ICategoryService.DeleteCategory(Guid id)
        {
            var foundItem = await _categoryRepo.GetOneByIdAsync(id);
            if (foundItem is not null)
            {
                await _categoryRepo.DeleteOneByIdAsync(foundItem);
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}