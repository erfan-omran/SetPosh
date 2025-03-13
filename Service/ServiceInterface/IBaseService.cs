using Core.Model;
using DataBase;
using System.Data;

namespace Service.ServiceInterface
{
    public interface IBaseService<T> where T : class
    {
        public Task<bool> AddAsync(T entity);
        public Task<bool> EditAsync(T entity);
        public Task BlockAsync(long SID);
        public Task DeleteAsync(long SID);
        //------------------------------------------
        public Task<T> GetSimpleModelAsync(long SID);
        public Task<T> GetModelWithRelatedEntitiesAsync(long SID);

        public QueryBuilder GetSimple();
        public QueryBuilder GetWithRelatedEntities();
        //------------------------------------------
        public List<T> MapDTToModel(DataTable dt);
    }
}
