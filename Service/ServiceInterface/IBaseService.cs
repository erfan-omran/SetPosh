using Core.Model;
using DataBase;
using System.Data;

namespace Service.ServiceInterface
{
    public interface IBaseService<T> where T : class
    {
        public Task<bool> AddAsync(T entity);
        //------------------------------------------
        public Task<T> GetModelSimpleAsync(long SID);
        public Task<T> GetModelWithRelatedEntitiesAsync(long SID);

        public QueryBuilder GetSimple();
        public QueryBuilder GetWithRelatedEntities();
        //------------------------------------------
        public List<T> MapDTToModel(DataTable dt);
    }
}
