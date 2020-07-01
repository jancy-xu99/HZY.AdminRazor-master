using AutoMapper.Configuration;
using HZY.Admin.Services.Core;
using HZY.DapperCore.Dapper;
using HZY.EFCore;
using HZY.EFCore.Repository;
using HZY.Models.Act;
using HZY.Models.Sys;
using HZY.Toolkit;
using HZY.Toolkit.Entitys;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Threading.Tasks;

namespace HZY.Admin.Services.Act
{
    public class ActivityWebService : ServiceBase<Fct_Activity>
    {
        protected readonly DefaultRepository<Sys_User> userDb;
        private IConfiguration _configuration;
        protected DapperClient dapper;
        //private IDapper _dapper;

        //public ActivityWebService(IDapperFactory dapperFactory, IConfiguration configuration, DapperClient dapper)
        //{
        //    _configuration = configuration;
        //    //_factory = DapperFactory.GetInstance(configuration);
        //    _dapper = dapper;
        //}
        public ActivityWebService(IDapperFactory dapperFactory, EFCoreContext _db, DefaultRepository<Sys_User> _userDb)
           : base(_db)
        {
            this.userDb = _userDb;
            dapper = dapperFactory.CreateClient("MSSQL1");

        }


        public ActivityModel GetActivityById(int actid)
        {
            string sql = string.Format(@" select ActId,ActName,ActStartTime,ActEndTime,ActIsWechat,ActMustSubscribe,ActMustLogin,ActIsEnable,ActMemo,Design_Introduce
                                    From Fct_Activity where ActId={0} and Disabled=0 ", actid);
            List<ActivityModel> list = dapper.GetList<ActivityModel>(sql).Result;
            return list == null ? null : list.FirstOrDefault();
        }

        public bool InsertSubscribeMsg(SubscribeMsgModel model)
        {
            try
            {
                //todo
                var modifyby = "";
                var arrIds = "";

                List<string> sqlList = new List<string>();

                var sql = string.Format(@" UPDATE Fct_RegionManager SET Disabled=1,ModifyTime=GETDATE(),ModifyBy='{0}' WHERE Id IN ({1}) and Disabled=0 ", modifyby, arrIds);

                sqlList.Add(sql);

                var cusSql = string.Format(@" UPDATE Fct_CustomerDirector SET Disabled=1,ModifyTime=GETDATE(),ModifyBy='{0}' WHERE RegionManagerId IN ({1}) and Disabled=0 ", modifyby, arrIds);

                sqlList.Add(cusSql);

                return dapper.ExecuteTrans(sqlList);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        


    }
}
