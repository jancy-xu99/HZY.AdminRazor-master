using System.ComponentModel;

namespace WechatMall.WX.Enum
{
    public enum EnumWxSubMsgType : int
    {
        [Description("关注推送")]
        Subscribe = 1,
        [Description("菜单推送")]
        Menu = 2
    }
}
