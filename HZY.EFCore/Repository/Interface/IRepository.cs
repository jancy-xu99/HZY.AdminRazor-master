using System;
using System.Collections.Generic;
using System.Text;

namespace HZY.EFCore.Repository.Interface
{
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;

    public interface IRepository<T, TDbContext>
        where T : class
        where TDbContext : DbContext
    {

        TDbContext Context { get; set; }

        DbSet<T> Set => null;

        #region KeyWhere
        Expression<Func<T, bool>> GetKeyWhere(T model);
        #endregion

        #region 插入
        T Insert(T model);
        int InsertRange(IEnumerable<T> model);

        Task<T> InsertAsync(T model);
        Task<int> InsertRangeAsync(IEnumerable<T> model);
        #endregion

        #region 更新
        int Update(T model);
        int UpdateById(T model);
        int Update(T oldModel, T newModel);
        int BatchUpdate(Expression<Func<T, T>> updateExpression, Expression<Func<T, bool>> predicate);
        int BatchUpdate(T updateExpression, Expression<Func<T, bool>> predicate);

        Task<int> UpdateAsync(T model);
        Task<int> UpdateByIdAsync(T model);
        Task<int> UpdateAsync(T oldModel, T newModel);
        Task<int> BatchUpdateAsync(Expression<Func<T, T>> updateExpression, Expression<Func<T, bool>> predicate);
        Task<int> BatchUpdateAsync(T updateExpression, Expression<Func<T, bool>> predicate);
        #endregion

        #region 插入或者更新
        T InsertOrUpdate(T model);

        Task<T> InsertOrUpdateAsync(T model);
        #endregion

        #region 删除
        int Delete(T model);
        int Delete(IEnumerable<T> models);
        int Delete(Expression<Func<T, bool>> expWhere);

        Task<int> DeleteAsync(T model);
        Task<int> DeleteAsync(IEnumerable<T> models);
        Task<int> DeleteAsync(Expression<Func<T, bool>> expWhere);
        #endregion

        #region 查询 复杂型
        IQueryable<T> Query(bool IsTracking);
        #endregion

        #region 查询 单条
        T Find(Expression<Func<T, bool>> expWhere);
        T FindById(object Key);

        Task<T> FindAsync(Expression<Func<T, bool>> expWhere);
        Task<T> FindByIdAsync(object Key);
        #endregion

        #region 查询 多条
        List<T> ToList(Expression<Func<T, bool>> expWhere);
        List<T> ToListAll();

        Task<List<T>> ToListAsync(Expression<Func<T, bool>> expWhere);
        Task<List<T>> ToListAllAsync();
        #endregion

        #region 是否存在 、 数量
        int Count();
        long CountLong();
        bool Any(Expression<Func<T, bool>> Where);

        Task<int> CountAsync();
        Task<long> CountLongAsync();
        Task<bool> AnyAsync(Expression<Func<T, bool>> Where);
        #endregion

    }
}
