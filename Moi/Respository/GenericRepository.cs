using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using GaragesStructure.Extensions;
using GaragesStructure.DATA;
using GaragesStructure.Entities;
using GaragesStructure.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;


namespace GaragesStructure.Repository{
    public class GenericRepository<T, TId> : IGenericRepository<T, TId> where T : BaseEntity<TId>{
        protected readonly DataContext _dbContext;
        protected readonly IMapper _mapper;

        protected GenericRepository(DataContext context, IMapper mapper) {
            _dbContext = context;
            _mapper = mapper;
        }

        public async Task<T> GetById(TId id) {
            var result = await _dbContext.Set<T>().FindAsync(id);
            return result;
        }


        public async Task<(List<TDto> data, int totalCount)> GetAll<TDto>(int pageNumber = 0, int pageSize = 10,
            bool deleted = false) {
            var query = _dbContext.Set<T>().AsNoTracking().Where(t => t.Deleted == deleted);
            return await ExecuteQuery<TDto>(query, pageNumber, pageSize);
        }

        public async Task<(List<TDto> data, int totalCount)> GetAll<TDto>(Expression<Func<T, bool>> predicate,
            int pageNumber = 0, int pageSize = 10, bool deleted = false) {
            var query = _dbContext.Set<T>().AsNoTracking().Where(predicate).Where(t => t.Deleted == deleted);
            return await ExecuteQuery<TDto>(query, pageNumber, pageSize);
        }

        private async Task<(List<TDto> data, int totalCount)> ExecuteQuery<TDto>(IQueryable<T> query, int pageNumber,
            int pageSize) {
            var totalCount = await query.CountAsync();
            var data = pageNumber == 0
                ? await query.OrderByDescending(x => x.CreationDate).ProjectTo<TDto>(_mapper.ConfigurationProvider).ToListAsync()
                : await query.OrderByDescending(x =>x.CreationDate).Skip(pageSize * (pageNumber - 1)).Take(pageSize)
                    .ProjectTo<TDto>(_mapper.ConfigurationProvider).ToListAsync();

            return (data, totalCount);
        }

        public async Task<T> Add(T entity, Guid? userId = null) {
            await _dbContext.Set<T>().AddAsync(entity);
            try {
                await _dbContext.SaveChangesAsync(userId);
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
                return null;
            }

            return entity;
        }

        public async Task<List<T>> AddRange(List<T> entities, Guid? userId = null) {
            await _dbContext.Set<T>().AddRangeAsync(entities);
            try {
                await _dbContext.SaveChangesAsync(userId);
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
                return null;
            }

            return entities;
        }
        
        

        public async Task<T> Delete(TId id, Guid? userId = null) {
            var result = await GetById(id);
            if (result == null) return null;
            _dbContext.Set<T>().Remove(result);
            await _dbContext.SaveChangesAsync(userId);
            return result;
        }

        public async Task<T> SoftDelete(TId id, Guid? userId = null) {
            var result = await GetById(id);
            if (result == null) return null;
            if (result.Deleted != null && (bool)result.Deleted) return null;

            result.Deleted = true;
            _dbContext.Set<T>().Update(result);

            try {
                await _dbContext.SaveChangesAsync(userId);
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
                return null;
            }

            return result;
        }

        public async Task<T> Update(T entity, Guid? userId = null) {
            _dbContext.Set<T>().Update(entity);
            try {
                await _dbContext.SaveChangesAsync(userId);
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
                return null;
            }

            return entity;
        }

        public async Task<List<TDto>> UpdateAll<TDto>(List<T> entities, Guid? userId = null)
        {
            var updatedData = new List<T>();
            
            foreach (var entity in entities)
            {
                _dbContext.Set<T>().Update(entity);
                updatedData.Add(entity);
            }
            
            try {
                await _dbContext.SaveChangesAsync(userId);
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
                return null;
            }

            
            return _mapper.Map<List<TDto>>(updatedData);;
        }
        
        public async Task<List<T>> UpdateAll(List<T> entities, Guid? userId = null)
        {
            var updatedData = new List<T>();
            
            foreach (var entity in entities)
            {
                _dbContext.Set<T>().Update(entity);
                updatedData.Add(entity);
            }
            
            try {
                await _dbContext.SaveChangesAsync(userId);
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
                return null;
            }

            
            return updatedData;
        }


        public async Task<T> Get(Expression<Func<T, bool>> predicate) {
            var entity = await _dbContext.Set<T>()
                .AsNoTracking()
                .Where(predicate)
                .FirstOrDefaultAsync();

            return entity;
        }

        public async Task<TDto?> Get<TDto>(Expression<Func<T, bool>> predicate) {
            var entity = await _dbContext.Set<T>()
                .AsNoTracking()
                .Where(predicate)
                .ProjectTo<TDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

            return entity;
        }

        public async Task<T> Get(Expression<Func<T, bool>> predicate,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include
        ) {
            var query = _dbContext.Set<T>()
                .AsQueryable();
            query = predicate != null ? query.Where(predicate) : query;
            if (include != null) query = include(query);
            return await query.FirstOrDefaultAsync();
        }

        public async Task<T> Get(Func<IQueryable<T>, IIncludableQueryable<T, object>> include) =>
            await Get(null, include);

        public async Task<(List<T> data, int totalCount)> GetAll(int pageNumber = 0, int pageSize = 10,
            bool deleted = false) {
            return await GetAll(null, null, pageNumber, pageSize, deleted);
        }

        public async Task<(List<T> data, int totalCount)> GetAll(Expression<Func<T, bool>> predicate,
            int pageNumber = 0, int pageSize = 10, bool deleted = false) {
            return await GetAll(predicate, null, pageNumber, pageSize, deleted);
        }

        public async Task<(List<T> data, int totalCount)> GetAll(
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include, int pageNumber = 0, int pageSize = 10,
            bool deleted = false) {
            return await GetAll(null, include, pageNumber, pageSize, deleted);
        }

        public async Task<(List<T> data, int totalCount)> GetAll(Expression<Func<T, bool>> predicate,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include, int pageNumber = 0, int pageSize = 10 ,
            bool deleted = false) {
            var query = predicate == null
                ? _dbContext.Set<T>().Where(t => t.Deleted == deleted)
                : _dbContext.Set<T>().Where(t => t.Deleted == deleted)
                    .Where(predicate);
            query = include != null ? include(query) : query;
            query = query.OrderByDescending(model => model.CreationDate);
            return (await (pageNumber == 0
                    ? query.ToListAsync()
                    : query.Skip(pageSize * (pageNumber - 1))
                        .Take(pageSize)
                        .ToListAsync()),
                await query.CountAsync());
        }
    }
}