/*
 * *******************************************************
 *
 * 作者：hzy
 *
 * 开源地址：https://gitee.com/hzy6
 *
 * *******************************************************
 */
using System;
using System.Net;

namespace HZY.Toolkit
{
    /// <summary>
    /// MVC 异常拦截 该对象 返回客户端
    /// </summary>
    [Serializable]
    public class MessageBox : Exception
    {
        /// <summary>
        /// 异常模型
        /// </summary>
        public ApiResult apiResult { set; get; }

        /// <summary>
        /// 成功消息 {status=1,msg="ok",Data=123}
        /// </summary>
        public MessageBox()
            : base("操作成功!")
        {
            this.apiResult = new ApiResult(StatusCodeEnum.成功, "操作成功!");
        }

        /// <summary>
        /// 失败并返回 {status=1,msg="ok",Data=123}
        /// </summary>
        /// <param name="Message"></param>
        public MessageBox(string Message)
            : base(Message)
        {
            this.apiResult = new ApiResult(StatusCodeEnum.警告, Message);
        }

        /// <summary>
        /// 自定义异常 {status=1,msg="ok",Data=123}
        /// </summary>
        /// <param name="StatusCode"></param>
        /// <param name="Message"></param>
        public MessageBox(StatusCodeEnum StatusCode, string Message)
            : base($"自定义消息!")
        {
            this.apiResult = new ApiResult(StatusCode, Message);
        }

        /// <summary>
        /// 自定义异常 {status=1,msg="ok",Data=123}
        /// </summary>
        /// <param name="StatusCode"></param>
        /// <param name="Data"></param>
        public MessageBox(StatusCodeEnum StatusCode, object Data)
            : base($"自定义消息!")
        {
            this.apiResult = new ApiResult(StatusCode, "", Data);
        }

        /// <summary>
        /// 自定义异常{status=1,msg="ok",Data=123}
        /// </summary>
        /// <param name="StatusCode"></param>
        /// <param name="Message"></param>
        /// <param name="Data"></param>
        public MessageBox(StatusCodeEnum StatusCode, string Message, object Data)
            : base($"自定义消息!")
        {
            this.apiResult = new ApiResult(StatusCode, Message, Data);
        }


        /// <summary>
        /// 自定义异常 {status=1,msg="ok",Data=123}
        /// </summary>
        /// <param name="StatusCode"></param>
        /// <param name="Message"></param>
        public MessageBox(int StatusCode, string Message)
            : base($"自定义消息!")
        {
            this.apiResult = new ApiResult(StatusCode, Message);
        }

        /// <summary>
        /// 自定义异常 {status=1,msg="ok",Data=123}
        /// </summary>
        /// <param name="StatusCode"></param>
        /// <param name="Data"></param>
        public MessageBox(int StatusCode, object Data)
            : base($"自定义消息!")
        {
            this.apiResult = new ApiResult(StatusCode, "", Data);
        }

        /// <summary>
        /// 自定义异常{status=1,msg="ok",Data=123}
        /// </summary>
        /// <param name="StatusCode"></param>
        /// <param name="Message"></param>
        /// <param name="Data"></param>
        public MessageBox(int StatusCode, string Message, object Data)
            : base($"自定义消息!")
        {
            this.apiResult = new ApiResult(StatusCode, Message, Data);
        }

        public MessageBox(string message, Exception innerException)
            : base(message, innerException)
        {

        }

        protected MessageBox(System.Runtime.Serialization.SerializationInfo serializationInfo, System.Runtime.Serialization.StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        {

        }

        /// <summary>
        /// 弹出警告
        /// </summary>
        /// <param name="message"></param>
        public static void Show(string message)
        {
            throw new MessageBox(StatusCodeEnum.警告, message, null);
        }

        /// <summary>
        /// 弹出警告
        /// </summary>
        /// <param name="data"></param>
        public static void Show(object data)
        {
            throw new MessageBox(StatusCodeEnum.警告, StatusCodeEnum.警告.ToString(), data);
        }

        /// <summary>
        /// 弹出警告
        /// </summary>
        /// <param name="message"></param>
        /// <param name="data"></param>
        public static void Show(string message, object data)
        {
            throw new MessageBox(StatusCodeEnum.警告, message, data);
        }


    }

    public enum StatusCodeEnum
    {
        程序异常 = -2,
        未授权 = -1,
        警告 = 0,
        成功 = 1,

    }

    public class ApiResult
    {
        public ApiResult(int status, string msg, object data = null)
        {
            this.status = status;
            this.msg = msg;
            this.data = data;
        }

        public ApiResult(StatusCodeEnum status, string msg, object data = null)
        {
            this.status = (int)status;
            this.msg = msg;
            this.data = data;
        }

        public int status { get; set; } = (int)StatusCodeEnum.警告;

        public string msg { get; set; }

        public object data { get; set; }

    }




}
