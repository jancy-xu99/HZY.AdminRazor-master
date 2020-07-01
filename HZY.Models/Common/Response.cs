namespace HZY.Models.Common
{
    public class Response
    {
        /// <summary>
        /// 返回码
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; }

        public Response()
        {
            Code = 200;
            Message = "操作成功";
        }
    }
}