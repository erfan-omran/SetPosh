
namespace Service.ServiceInterface
{
    public interface IBaseNormalService<T> : IBaseService<T> where T : class
    {
        public Task<bool> EditAsync(T entity);
        public Task BlockAsync(long SID);
        public Task DeleteAsync(long SID);
    }
}
