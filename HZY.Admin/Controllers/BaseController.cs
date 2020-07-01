/*
 * *******************************************************
 *
 * 作者：hzy
 *
 * 开源地址：https://gitee.com/hzy6
 *
 * *******************************************************
 */

//错误的请求结果 BadRequestResult 400
//冲突结果 ConflictResult 409
//没有内容结果 NoContentResult 204
//没有找到结果 NotFoundResult 404
//好的结果 OkResult 200
//未经授权的结果 UnauthorizedResult 401
//不可处理的实体结果 UnprocessableEntityResult 422
//不支持的媒体类型结果 UnsupportedMediaTypeResult 415
//内部服务器错误结果 InternalServerErrorResult 500

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HZY.Admin.Controllers
{
    using HZY.Toolkit;

    [Route("[controller]")]
    [ApiController]
    public class BaseController : Controller
    {
        [NonAction]
        public ApiResult Result(int status, string msg) => new ApiResult(status, msg);

        [NonAction]
        public ApiResult Result(int status, object data) => new ApiResult(status, "", data);

        [NonAction]
        public ApiResult Result(int status, string msg, object data) => new ApiResult(status, msg, data);

        [NonAction]
        public ApiResult Result(StatusCodeEnum status, string msg) => new ApiResult(status, msg);

        [NonAction]
        public ApiResult Result(StatusCodeEnum status, object data) => new ApiResult(status, "", data);

        [NonAction]
        public ApiResult Result(StatusCodeEnum status, string msg, object data) => new ApiResult(status, msg, data);

        [NonAction]
        public ApiResult ResultOk() => new ApiResult(StatusCodeEnum.成功, "success");

        [NonAction]
        public ApiResult ResultOk(string msg) => new ApiResult(StatusCodeEnum.成功, msg);

        [NonAction]
        public ApiResult ResultOk(object data) => new ApiResult(StatusCodeEnum.成功, "", data);

        [NonAction]
        public ApiResult ResultOk(string msg, object data) => new ApiResult(StatusCodeEnum.成功, msg, data);

        [NonAction]
        public ApiResult ResultError(string msg) => new ApiResult(StatusCodeEnum.警告, msg);

        [NonAction]
        public ApiResult ResultError(object data) => new ApiResult(StatusCodeEnum.警告, "", data);

        [NonAction]
        public ApiResult ResultError(string msg, object data) => new ApiResult(StatusCodeEnum.警告, msg, data);

    }
}