using Dapper;
using GYLib.Base.Utils;
using HZY.Admin.Services.Core;
using HZY.DapperCore.Dapper;
using HZY.EFCore;
using HZY.EFCore.Base;
using HZY.EFCore.Repository;
using HZY.Models.Act;
using HZY.Models.Common;
using HZY.Models.Sys;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace HZY.Admin.Services.Act
{
    public class ActivityService : ServiceBase<Fct_Activity>
    {


        protected readonly DefaultRepository<Sys_User> userDb;
        protected DapperClient dapper;
        public ActivityService(IDapperFactory dapperFactory, EFCoreContext _db, DefaultRepository<Sys_User> _userDb)
            : base(_db)
        {
            this.userDb = _userDb;
            dapper = dapperFactory.CreateClient("MSSQL1");

        }
        /// <summary>
        /// 获取数据源
        /// </summary>
        /// <param name="Page"></param>
        /// <param name="Rows"></param>
        /// <param name="Search"></param>
        /// <returns></returns>
        public async Task<TableViewModel> FindListAsync(int Page, int Rows, Fct_Activity act)
        {


            PagerModel model = new PagerModel();
            string strWhere = "  Disabled=0 ";

            if (act == null) { act = new Fct_Activity(); }
            if (!string.IsNullOrEmpty(act.ActName))
            {
                strWhere += " and  ( ActId=" + ConvertUtils.ConvertToInt(act.ActName) + " or  ActName=" + act.ActName + " ) ";
            }
            model.SortField = " ActId ";
            string table = @" dbo.Fct_Activity  ";
            string fields = @"     [ActId]
                                      ,[ActGuId]
                                      ,[ActName]
                                      ,[ActStartTime]
                                      ,[ActEndTime]
                                      ,[ActIsWechat]
                                      ,[ActMustSubscribe]
                                      ,[ActMustLogin]
                                      ,[ActIsEnable]
                                      ,[ActMemo]
                                      ,[Disabled]
                                      ,[CreateTime]
                                      ,[CreateBy]
                                      ,[ModifyTime]
                                      ,[ModifyBy]
                                      ,[Design_Introduce]";
            var param = new DynamicParameters();
            param.Add("@PageIndex", model.PageIndex);
            param.Add("@PageSize", model.PageSize);
            param.Add("@TableName", table);
            param.Add("@OrderFields", model.SortField);
            param.Add("@Fields", fields);
            param.Add("@Where", strWhere);
            //param.Add("@OrderType", model.SortDirection);
            //param.Add("@IsDistinct", 1);
            param.Add("@RecordCount", 0, DbType.Int32, ParameterDirection.Output);
            //var data = base.dapper.GetList<Fct_Activity>("Page_Query", param, CommandType.StoredProcedure);

            var data = await dapper.GetList<Fct_Activity>("Page_Query", param, CommandType.StoredProcedure);
            model.RecordCount = param.Get<int>("@RecordCount");
            return this.db.DataAsTableViewModel(data, Page, Rows, model.RecordCount, typeof(Fct_Activity));
        }







        /// <summary>
        /// 新增\修改
        /// </summary>
        /// <param name="model"></param>
        /// <param name="webRootPath"></param>
        /// <param name="Photo"></param>
        /// <param name="Files"></param>
        /// <returns></returns>
        public async Task<int> SaveAsync(Fct_Activity model, string webRootPath, List<IFormFile> Files)
        {
            if (Files.Count > 0)
            {
                var path = new List<string>();
                foreach (var item in Files)
                {
                    path.Add(this.HandleUploadFile(item, webRootPath));
                }
                //if (path.Count > 0) model.Design_FilePath = string.Join(",", path);
            }

            await this.InsertOrUpdateAsync(model);

            return (int)model.ActId;
        }


        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="Ids"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(List<int> Ids)
            => await this.DeleteAsync(w => Ids.Contains((int)w.ActId));

        /// <summary>
        /// 加载表单 数据
        /// </summary>
        /// <param name="ActId"></param>
        /// <returns></returns>
        public async Task<Dictionary<string, object>> LoadFormAsync(int ActId)
        {
            var res = new Dictionary<string, object>();

            //var Model = await this.FindByIdAsync(ActId);
      

            var Model = await dapper.GetByIDAsync<Fct_Activity>(ActId);

            //var User = await userDb.FindByIdAsync(Model?.ActId);

            res[nameof(ActId)] = ActId;
            res[nameof(Model)] = Model.ToNewByNull();
            //res[nameof(User)] = User.ToNewByNull();

            return res;
        }

    }
}
