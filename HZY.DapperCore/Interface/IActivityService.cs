using HZY.Models.Act;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HZY.DapperCore.Interface
{
    public interface IActivityService
    {
        ActivityModel GetActivityById(int id);

        //插入关注公众号后推送信息
        int InsertSubscribeMsg(SubscribeMsgModel model);
    }
}
