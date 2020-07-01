using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WechatMall.WX.Model.Wx
{
    public class WxTempletRequeryModel : ICloneable
    {
        public string TempName { get; set; }
        public string WxOpenId { get; set; }
        public string LinkUrl { get; set; }
        public string[] Valuse { get; set; }

        public object Clone()
        {
            return new WxTempletRequeryModel()
            {
                LinkUrl = LinkUrl,
                TempName = TempName,
                Valuse = Valuse,
                WxOpenId = WxOpenId
            };
        }
    }
}
