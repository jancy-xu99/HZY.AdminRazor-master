using System;
using System.Collections.Generic;
using System.Text;

namespace HZY.EFCore
{
    using Microsoft.EntityFrameworkCore;
    using System.Linq;
    using System.Threading.Tasks;
    using HZY.EFCore.Base;
    using HZY.EFCore.Repository;
    using HZY.Models;
    using HZY.Models.Sys;
    using System.Data;
    using Microsoft.Data.SqlClient;
    using System.Data.Common;
    using HZY.EFCore.Repository.Interface;
    using Microsoft.Extensions.Logging;
    using System.Collections;
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.EntityFrameworkCore.Storage;
    using HZY.Models.Mem;
    using HZY.Models.Act;

    public class EFCoreContext : DbContext, IUnitOfWork
    {

        public EFCoreContext(DbContextOptions<EFCoreContext> options) : base(options) { }

        #region DbSet
        //
        public DbSet<TABLES_COLUMNS> TABLES_COLUMNS_DbSet { get; set; }
        public DbSet<TABLE_NAME> TABLE_NAME_DbSet { get; set; }
        //
        public DbSet<Sys_AppLog> Sys_AppLogs { get; set; }
        public DbSet<Sys_Function> Sys_Functions { get; set; }
        public DbSet<Sys_Menu> Sys_Menus { get; set; }
        public DbSet<Sys_MenuFunction> Sys_MenuFunctions { get; set; }
        public DbSet<Sys_Role> Sys_Roles { get; set; }
        public DbSet<Sys_RoleMenuFunction> Sys_RoleMenuFunctions { get; set; }
        public DbSet<Sys_User> Sys_Users { get; set; }
        public DbSet<Sys_UserRole> Sys_UserRoles { get; set; }
        //
        public DbSet<Member> Members { get; set; }
        public DbSet<Fct_Member> Fct_Member { get; set; }
        public DbSet<Fct_MemSubscribe> Fct_MemSubscribe { get; set; }
        public DbSet<Fct_Activity> Fct_Activity { get; set; }

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TABLES_COLUMNS>().HasNoKey();
            modelBuilder.Entity<TABLE_NAME>().HasNoKey();
            //
            //modelBuilder.Entity<Sys_AppLog>().ToTable("Sys_AppLog");
            //modelBuilder.Entity<User>().ToTable("User");

            #region 扫描表 并 缓存 属性信息 
            var types = modelBuilder.Model.GetEntityTypes().Select(item => item.ClrType).ToList();
            ModelCache.Set(types);
            #endregion
        }

        #region IUnitOfWork

        public bool SaveState { get; set; } = true;

        public virtual void CommitOpen() => this.SaveState = false;

        public virtual int Commit()
        {
            this.SaveState = true;
            return this.Save();
        }

        public virtual Task<int> CommitAsync()
        {
            this.SaveState = true;
            return this.SaveAsync();
        }

        public virtual IDbContextTransaction BeginTransaction() => this.Database.BeginTransaction();

        public virtual Task<IDbContextTransaction> BeginTransactionAsync() => this.Database.BeginTransactionAsync();

        public virtual int Save()
        {
            if (this.SaveState)
                return this.SaveChanges();
            else
                return 1;
        }

        public virtual Task<int> SaveAsync()
        {
            if (this.SaveState)
                return this.SaveChangesAsync();
            else
                return Task.FromResult(1);
        }

        #endregion

        public TableViewModel DataAsTableViewModel<T>(List<T> datas, int Page, int Rows, int RecordCount, params Type[] EntityTypes)
        {


            TableViewModel _tableViewModel = new TableViewModel();

            _tableViewModel.Page = Page;
            _tableViewModel.Rows = Rows;
            _tableViewModel.TotalCount = RecordCount;
            _tableViewModel.TotalPage = (RecordCount / Rows);
            //DynamicQueryable.OrderBy(query, "name asc");


            var type = typeof(T);
            var fields = type.GetProperties();

            #region 组合一下 类型的属性和类型名称
            var TabAndField = new List<(string, string)>();
            foreach (var item in EntityTypes)
            {
                foreach (var prop in item.GetProperties())
                {
                    TabAndField.Add((prop.Name, item.Name));
                }
            }
            #endregion

            #region 填充 列 信息

            foreach (var field in fields)
            {
                var tableName = TabAndField.Find(w => w.Item1 == field.Name).Item2;

                var title = string.Empty;

                if (!string.IsNullOrWhiteSpace(tableName))
                {
                    var modelInfos = ModelCache.GetModelInfos(tableName);
                    title = modelInfos.FirstOrDefault(w => w.Name == field.Name)?.Remark;
                }

                _tableViewModel.Cols.Add(new TableViewCol()
                {
                    DataIndex = field.Name,
                    Show = field.Name != "_ukid",
                    Title = string.IsNullOrWhiteSpace(title) ? field.Name : title
                });
            }
            #endregion
            #region 和数据转换为 list Hashtable
            foreach (var item in datas)
            {
                var hashTable = new Hashtable();
                foreach (var field in fields) hashTable[field.Name] = field.GetValue(item);
                _tableViewModel.DataSource.Add(hashTable);
            }
            #endregion
            return _tableViewModel;
        }








        public async Task<TableViewModel> AsTableViewModelAsync<T>(IQueryable<T> query, int Page, int Rows, params Type[] EntityTypes)
        {
            var _tableViewModel = new TableViewModel();

            _tableViewModel.Page = Page;
            _tableViewModel.Rows = Rows;
            _tableViewModel.TotalCount = await query.CountAsync();
            _tableViewModel.TotalPage = (_tableViewModel.TotalCount / Rows);
            //DynamicQueryable.OrderBy(query, "name asc");
            var Datas = await query.Skip((Page - 1) * Rows).Take(Rows).ToListAsync();

            var type = typeof(T);
            var fields = type.GetProperties();

            #region 组合一下 类型的属性和类型名称
            var TabAndField = new List<(string, string)>();
            foreach (var item in EntityTypes)
            {
                foreach (var prop in item.GetProperties())
                {
                    TabAndField.Add((prop.Name, item.Name));
                }
            }
            #endregion

            #region 填充 列 信息

            foreach (var field in fields)
            {
                var tableName = TabAndField.Find(w => w.Item1 == field.Name).Item2;

                var title = string.Empty;

                if (!string.IsNullOrWhiteSpace(tableName))
                {
                    var modelInfos = ModelCache.GetModelInfos(tableName);
                    title = modelInfos.FirstOrDefault(w => w.Name == field.Name)?.Remark;
                }

                _tableViewModel.Cols.Add(new TableViewCol()
                {
                    DataIndex = field.Name,
                    Show = field.Name != "_ukid",
                    Title = string.IsNullOrWhiteSpace(title) ? field.Name : title
                });
            }

            #endregion

            #region 和数据转换为 list Hashtable
            foreach (var item in Datas)
            {
                var hashTable = new Hashtable();
                foreach (var field in fields) hashTable[field.Name] = field.GetValue(item);
                _tableViewModel.DataSource.Add(hashTable);
            }
            #endregion

            return _tableViewModel;
        }



        /// <summary>
        /// 根据表名称 获取列
        /// </summary>
        /// <param name="TableName"></param>
        /// <returns></returns>
        public virtual async Task<List<TABLES_COLUMNS>> GetColsByTableNameAsync(string TableName)
        {
            // SELECT 
            //     表名       = case when a.colorder=1 then d.name else '' end,
            //     表说明     = case when a.colorder=1 then isnull(f.value,'') else '' end,
            //     字段序号   = a.colorder,
            //     字段名     = a.name,
            //     标识       = case when COLUMNPROPERTY( a.id,a.name,'IsIdentity')=1 then '√'else '' end,
            //     主键       = case when exists(SELECT 1 FROM sysobjects where xtype='PK' and parent_obj=a.id and name in (
            //                      SELECT name FROM sysindexes WHERE indid in( SELECT indid FROM sysindexkeys WHERE id = a.id AND colid=a.colid))) then '√' else '' end,
            //     类型       = b.name,
            //     占用字节数 = a.length,
            //     长度       = COLUMNPROPERTY(a.id,a.name,'PRECISION'),
            //     小数位数   = isnull(COLUMNPROPERTY(a.id,a.name,'Scale'),0),
            //     允许空     = case when a.isnullable=1 then '√'else '' end,
            //     默认值     = isnull(e.text,''),
            //     字段说明   = isnull(g.[value],'')
            // FROM 
            //     syscolumns a
            // left join 
            //     systypes b 
            // on 
            //     a.xusertype=b.xusertype
            // inner join 
            //     sysobjects d 
            // on 
            //     a.id=d.id  and d.xtype='U' and  d.name<>'dtproperties'
            // left join 
            //     syscomments e 
            // on 
            //     a.cdefault=e.id
            // left join 
            // sys.extended_properties   g 
            // on 
            //     a.id=G.major_id and a.colid=g.minor_id  
            // left join
            // sys.extended_properties f
            // on 
            //     d.id=f.major_id and f.minor_id=0
            // where  
            //     d.name='member' 
            // order by  
            //     a.id,a.colorder

            string SqlString = $@"

SELECT 
    d.name AS TabName,
	isnull(f.value,'') AS TabNameRemark,
	convert(int,a.colorder) AS ColOrder,
	a.name AS ColName,
	case when COLUMNPROPERTY( a.id,a.name,'IsIdentity')=1 then 1 else 0 end ColIsIdentity,
	case when exists(SELECT 1 FROM sysobjects where xtype='PK' and parent_obj=a.id and name in (
                     SELECT name FROM sysindexes WHERE indid in( SELECT indid FROM sysindexkeys WHERE id = a.id AND colid=a.colid))) then 1 else 0 end ColIsKey,
	b.name AS ColType,
	convert(int,a.length) AS ColLength,
	convert(int,COLUMNPROPERTY(a.id,a.name,'PRECISION')) AS ColMaxLength,
	convert(int,isnull(COLUMNPROPERTY(a.id,a.name,'Scale'),0)) AS ColScale,
	case when a.isnullable=1 then 1 else 0 end ColIsNull,
	isnull(e.text,'') AS ColDefaultValue,
    isnull(g.[value],'') AS ColRemark
FROM syscolumns a
left join systypes b on a.xusertype=b.xusertype
inner join sysobjects d on a.id=d.id  and d.xtype='U' and  d.name<>'dtproperties'
left join syscomments e on a.cdefault=e.id
left join sys.extended_properties g on a.id=G.major_id and a.colid=g.minor_id  
left join sys.extended_properties f on d.id=f.major_id and f.minor_id=0
where d.name='{TableName}'
order by a.id,a.colorder

";

            return await this.TABLES_COLUMNS_DbSet.FromSqlRaw(SqlString).ToListAsync();
        }

        /// <summary>
        /// 获取所有的表名称
        /// </summary>
        /// <returns></returns>
        public virtual async Task<List<TABLE_NAME>> GetAllTableAsync()
        {
            string SqlString = @"SELECT TABLE_NAME AS Name FROM INFORMATION_SCHEMA.TABLES order by TABLE_NAME";

            return await this.TABLE_NAME_DbSet.FromSqlRaw(SqlString).ToListAsync();
        }




    }
}
