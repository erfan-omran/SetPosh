
namespace Service.ServiceInterface
{
    public interface IBaseNormalService<T> : IBaseService<T> where T : class
    {
        public Task<bool> EditAsync(T entity);
        public Task<bool> BlockAsync(long SID);
        public Task<bool> UnBlockAsync(long SID);
        public Task<bool> DeleteAsync(long SID);
        public Task<bool> UnDeleteAsync(long SID);
    }
}
