using HZY.DapperCore.Dapper;
using HZY.DapperCore.Interface;
using HZY.Models.Act;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HZY.DapperCore
{
    public class ActivityService : IActivityService
    {
        //private IConfiguration _configuration;
        //private DapperClient _dapper;
        ////private IDapper _dapper;

        //public ActivityService(IConfiguration configuration, DapperClient dapper)
        //{
        //    _configuration = configuration;
        //    //_factory = DapperFactory.GetInstance(configuration);
        //    _dapper = dapper;
        //}



        //public ActivityModel GetActivityById(int actid)
        //{
        //    string sql = string.Format(@" select ActId,ActName,ActStartTime,ActEndTime,ActIsWechat,ActMustSubscribe,ActMustLogin,ActIsEnable,ActMemo,Design_Introduce
        //                            From Fct_Activity where ActId={0} and Disabled=0 ", actid);
        //    return _dapper.GetList<ActivityModel>(sql).FirstOrDefault();
        //}

        //public bool InsertSubscribeMsg(SubscribeMsgModel model)
        //{
        //    try
        //    {
        //        List<string> sqlList = new List<string>();

        //        var sql = string.Format(@" UPDATE Fct_RegionManager SET Disabled=1,ModifyTime=GETDATE(),ModifyBy='{0}' WHERE Id IN ({1}) and Disabled=0 ", mdifyBy, arrIds);

        //        sqlList.Add(sql);

        //        var cusSql = string.Format(@" UPDATE Fct_CustomerDirector SET Disabled=1,ModifyTime=GETDATE(),ModifyBy='{0}' WHERE RegionManagerId IN ({1}) and Disabled=0 ", mdifyBy, arrIds);

        //        sqlList.Add(cusSql);

        //        return _dapper.ExecuteTrans(sqlList);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        public ActivityModel GetActivityById(int id)
        {
            throw new NotImplementedException();
        }

        public int InsertSubscribeMsg(SubscribeMsgModel model)
        {
            throw new NotImplementedException();
        }
    }
}
