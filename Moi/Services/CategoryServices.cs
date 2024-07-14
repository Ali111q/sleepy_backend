
using AutoMapper;
using BackEndStructuer.DATA.DTOs;
using BackEndStructuer.Entities;
using BackEndStructuer.Repository;
using BackEndStructuer.Services;
using GaragesStructure.Repository;

namespace BackEndStructuer.Services;


public interface ICategoryServices
{
Task<(Category? category, string? error)> Create(CategoryForm categoryForm );
Task<(List<CategoryDto> categorys, int? totalCount, string? error)> GetAll(CategoryFilter filter);
Task<(Category? category, string? error)> Update(Guid id , CategoryUpdate categoryUpdate);
Task<(Category? category, string? error)> Delete(Guid id);
}

public class CategoryServices : ICategoryServices
{
private readonly IMapper _mapper;
private readonly IRepositoryWrapper _repositoryWrapper;

public CategoryServices(
    IMapper mapper ,
    IRepositoryWrapper repositoryWrapper
    )
{
    _mapper = mapper;
    _repositoryWrapper = repositoryWrapper;
}
   
   
public async Task<(Category? category, string? error)> Create(CategoryForm categoryForm )
{
    
   var category = await _repositoryWrapper.Category.Add(_mapper.Map<Category>(categoryForm));
   return (category, null);

}

public async Task<(List<CategoryDto> categorys, int? totalCount, string? error)> GetAll(CategoryFilter filter)
{
    var (data, count) = await _repositoryWrapper.Category.GetAll<CategoryDto>(
       e=> filter.Name == null || e.Name.Contains(filter.Name), filter.PageNumber, filter.PageSize, filter.Deleted
        );
    return (data, count, null);
}

public async Task<(Category? category, string? error)> Update(Guid id ,CategoryUpdate categoryUpdate)
{
    var category = await _repositoryWrapper.Category.Update(_mapper.Map<Category>(categoryUpdate), id);
    return (category, null);
}

public async Task<(Category? category, string? error)> Delete(Guid id)
{
    var category = await _repositoryWrapper.Category.SoftDelete(id);
    return (category, null);

}

}
