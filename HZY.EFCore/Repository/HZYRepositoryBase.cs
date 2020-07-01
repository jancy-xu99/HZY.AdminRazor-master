/*
 * *******************************************************
 *
 * 作者：hzy
 *
 * 开源地址：https://gitee.com/hzy6
 *
 * *******************************************************
 */
//
//https://github.com/borisdj/EFCore.BulkExtensions
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HZY.EFCore.Repository
{
    using global::EFCore.BulkExtensions;
    using HZY.EFCore.Repository.Interface;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// 基础仓储
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TDbContext"></typeparam>
    public abstract class HZYRepositoryBase<T, TDbContext> : IRepository<T, TDbContext>
        where T : class, new()
        where TDbContext : EFCoreContext
    {
        protected HZYRepositoryBase(TDbContext context)
        {
            this.Context = context;
        }

        public TDbContext Context { get; set; }

        public DbSet<T> Set => this.Context.Set<T>();

        #region KeyWhere

        public Expression<Func<T, bool>> GetKeyWhere(T model)
        {
            model = model.ToNewByNull();
            var keyProperty = model.GetType().GetKeyProperty();
            var expWhere = HZYEFCoreExtensions.ToWhere<T>(keyProperty.Name, keyProperty.GetValue(model));
            // var expMemberInit = HZYEFCoreExtensions.ToMemberInitByModel(model);
            return expWhere;
        }

        #endregion

        #region 插入
        public virtual T Insert(T model)
        {
            this.Set.Add(model);
            this.Context.Save();
            return model;
        }
        public virtual int InsertRange(IEnumerable<T> model)
        {
            this.Set.AddRange(model);
            return this.Context.Save();
        }

        public virtual async Task<T> InsertAsync(T model)
        {
            await this.Set.AddAsync(model);
            await this.Context.SaveAsync();
            return model;
        }
        public virtual Task<int> InsertRangeAsync(IEnumerable<T> model)
        {
            this.Set.AddRangeAsync(model);
            return this.Context.SaveAsync();
        }
        #endregion

        #region 更新
        public virtual int Update(T model)
        {
            //Extensions
            this.Set.Update(model);
            return this.Context.Save();
        }
        public virtual int UpdateById(T model)
        {
            var expWhere = this.GetKeyWhere(model);
            var entity = this.Set.FirstOrDefault(expWhere);
            if (entity == null) return -1;
            return this.Update(entity, model);
        }
        public virtual int Update(T oldModel, T newModel)
        {
            this.Context.Entry(oldModel).CurrentValues.SetValues(newModel);
            return this.Context.Save();
        }
        /// <summary>
        /// 批量更新 如果使用事务 请使用 db.BeginTransaction() 不能使用 db.Commit()
        /// </summary>
        /// <param name="updateExpression"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual int BatchUpdate(Expression<Func<T, T>> updateExpression, Expression<Func<T, bool>> predicate)
            => this.Set.Where(predicate).BatchUpdate(updateExpression);
        /// <summary>
        /// 批量更新 [更新全字段] 如果使用事务 请使用 db.BeginTransaction() 不能使用 db.Commit()
        /// </summary>
        /// <param name="updateExpression"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual int BatchUpdate(T updateExpression, Expression<Func<T, bool>> predicate)
            => this.Set.Where(predicate).BatchUpdate(updateExpression);

        public virtual Task<int> UpdateAsync(T model)
        {
            this.Set.Update(model);
            return this.Context.SaveAsync();
        }
        public virtual async Task<int> UpdateByIdAsync(T model)
        {
            var expWhere = this.GetKeyWhere(model);
            var entity = await this.Set.FirstOrDefaultAsync(expWhere);
            if (entity == null) return -1;
            return await this.UpdateAsync(entity, model);
        }
        public virtual Task<int> UpdateAsync(T oldModel, T newModel)
        {
            this.Context.Entry(oldModel).CurrentValues.SetValues(newModel);
            return this.Context.SaveAsync();
        }
        /// <summary>
        /// 批量更新 异步 如果使用事务 请使用 db.BeginTransactionAsync() 不能使用 db.CommitAsync()
        /// </summary>
        /// <param name="updateExpression"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual Task<int> BatchUpdateAsync(Expression<Func<T, T>> updateExpression, Expression<Func<T, bool>> predicate)
            => this.Set.Where(predicate).BatchUpdateAsync(updateExpression);
        /// <summary>
        /// 批量更新 [更新全字段] 异步 如果使用事务 请使用 db.BeginTransactionAsync() 不能使用 db.CommitAsync()
        /// </summary>
        /// <param name="updateExpression"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual Task<int> BatchUpdateAsync(T updateExpression, Expression<Func<T, bool>> predicate)
            => this.Set.Where(predicate).BatchUpdateAsync(updateExpression);
        #endregion

        #region 插入或者更新
        public virtual T InsertOrUpdate(T model)
        {
            var expWhere = this.GetKeyWhere(model);
            var entity = this.Set.FirstOrDefault(expWhere);
            if (entity == null)
                this.Insert(model);
            else
                this.Update(entity, model);
            return model;
        }

        public virtual async Task<T> InsertOrUpdateAsync(T model)
        {
            var expWhere = this.GetKeyWhere(model);
            var entity = await this.Set.FirstOrDefaultAsync(expWhere);
            if (entity == null)
                await this.InsertAsync(model);
            else
                await this.UpdateAsync(entity, model);
            return model;
        }
        #endregion

        #region 删除
        public virtual int Delete(T model)
        {
            this.Set.Remove(model);
            return this.Context.Save();
        }
        public virtual int Delete(IEnumerable<T> models)
        {
            this.Set.RemoveRange(models);
            return this.Context.Save();
        }
        public virtual int Delete(Expression<Func<T, bool>> expWhere)
            => this.Delete(this.Query().Where(expWhere));

        public virtual Task<int> DeleteAsync(T model)
        {
            this.Set.Remove(model);
            return this.Context.SaveAsync();
        }
        public virtual Task<int> DeleteAsync(IEnumerable<T> models)
        {
            this.Set.RemoveRange(models);
            return this.Context.SaveAsync();
        }
        public virtual Task<int> DeleteAsync(Expression<Func<T, bool>> expWhere)
            => this.DeleteAsync(this.Query().Where(expWhere));
        #endregion

        #region 查询 复杂型
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="IsTracking">是否追踪</param>
        /// <returns></returns>
        public virtual IQueryable<T> Query(bool IsTracking = false)
            => IsTracking ? this.Set.AsQueryable() : this.Set.AsQueryable().AsNoTracking();
        #endregion

        #region 查询 单条
        public virtual T Find(Expression<Func<T, bool>> expWhere)
            => this.Query().Where(expWhere).FirstOrDefault();
        public virtual T FindById(object Key)
            => this.Set.Find(Key);

        public virtual Task<T> FindAsync(Expression<Func<T, bool>> expWhere)
            => this.Query().Where(expWhere).FirstOrDefaultAsync();
        public virtual async Task<T> FindByIdAsync(object Key)
            => await this.Set.FindAsync(Key);
        #endregion

        #region 查询 多条
        public virtual List<T> ToList(Expression<Func<T, bool>> expWhere)
            => this.Query().Where(expWhere).ToList();
        public virtual List<T> ToListAll()
            => this.Query().ToList();

        public virtual Task<List<T>> ToListAsync(Expression<Func<T, bool>> expWhere)
            => this.Query().Where(expWhere).ToListAsync();
        public virtual Task<List<T>> ToListAllAsync()
            => this.Query().ToListAsync();
        #endregion

        #region 是否存在 、 数量
        public virtual int Count()
            => this.Query().Count();
        public virtual long CountLong()
            => this.Query().LongCount();
        public virtual bool Any(Expression<Func<T, bool>> expWhere)
            => this.Query().Where(expWhere).Any();

        public virtual Task<int> CountAsync()
            => this.Query().CountAsync();
        public virtual Task<long> CountLongAsync()
            => this.Query().LongCountAsync();
        public virtual Task<bool> AnyAsync(Expression<Func<T, bool>> expWhere)
            => this.Query().Where(expWhere).AnyAsync();
        #endregion

    }
}
