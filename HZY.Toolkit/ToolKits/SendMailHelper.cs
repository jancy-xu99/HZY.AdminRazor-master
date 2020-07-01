

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HZY.Toolkit.ToolKits
{
    public class SendMailHelper
    {
        public static bool SendMail(List<string> mails, string title, string mailContent)
        {
            string displaymail="it-development@cftcn.com";

            string displayname="网站";
            try
            { 
                if (mails != null && mails.Count > 0)
                {
                    //if (GYNotice.DataAccess.Client.NoticeMehtods.SendMail(mails, title, mailContent, true, displaymail, displayname))
                    //{
                    //    return true;
                    //}
                    //else
                    //{
                       
                    //    LogHelp.Info(title+"邮件发送失败");
                    //    return false;
                    //}
                }
            }
            catch (Exception ex)
            {
               
                //LogHelp.Error("邮件发送失败"+ex.Message);
            }
            return false;
        }
    }
}
