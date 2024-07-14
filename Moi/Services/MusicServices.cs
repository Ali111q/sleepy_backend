
using AutoMapper;
using BackEndStructuer.DATA.DTOs;
using BackEndStructuer.Entities;
using BackEndStructuer.Repository;
using BackEndStructuer.Services;
using GaragesStructure.Repository;

namespace BackEndStructuer.Services;


public interface IMusicServices
{
Task<(Music? music, string? error)> Create(MusicForm musicForm );
Task<(List<MusicDto> musics, int? totalCount, string? error)> GetAll(MusicFilter filter);
Task<(Music? music, string? error)> Update(Guid id , MusicUpdate musicUpdate);
Task<(Music? music, string? error)> Delete(Guid id);
}

public class MusicServices : IMusicServices
{
private readonly IMapper _mapper;
private readonly IRepositoryWrapper _repositoryWrapper;

public MusicServices(
    IMapper mapper ,
    IRepositoryWrapper repositoryWrapper
    )
{
    _mapper = mapper;
    _repositoryWrapper = repositoryWrapper;
}
   
   
public async Task<(Music? music, string? error)> Create(MusicForm musicForm )
{
    var music = await _repositoryWrapper.Music.Add(_mapper.Map<Music>(musicForm));
    return (music, null);

}

public async Task<(List<MusicDto> musics, int? totalCount, string? error)> GetAll(MusicFilter filter)
{
    var (data, count) = await _repositoryWrapper.Music.GetAll<MusicDto>(e =>
            (filter.Name == null || e.Name.Contains(filter.Name) &&
                (filter.CategoryId == null || filter.CategoryId == e.CategoryId))
        , filter.PageNumber, filter.PageSize, filter.Deleted
    );

    return (data, count, null);
}

public async Task<(Music? music, string? error)> Update(Guid id ,MusicUpdate musicUpdate)
    {
        throw new NotImplementedException();
      
    }

public async Task<(Music? music, string? error)> Delete(Guid id)
    {
        throw new NotImplementedException();
   
    }

}
