using GYWx.Reply.NormalReply;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WechatMall.WX.Model.Wx
{
    public class ContentModel
    {
        /// <summary>
        /// Content 中Xml反序列化后的产物 
        /// </summary>
        public NewsModel[] News { get; set; }

        public ContentModel()
        {
            News = new List<NewsModel>().ToArray();
        }

        public void SetNewsModelArray(object[] value)
        {
            var list = new List<NewsModel>();
            int lastLength = 4;
            if (value != null)
            {
                for (int i = 0; i < value.Length; i++)
                {
                    list.Add(value[i] as NewsModel);
                    lastLength--;
                }
            }

            if (lastLength > 0)
            {
                for (int i = 0; i < lastLength; i++)
                {
                    list.Add(new NewsModel());
                }
            }

            News = list.ToArray();
        }

        public NewsModel[] GetNewsModelArray()
        {
            return News;
        }
    }
}
