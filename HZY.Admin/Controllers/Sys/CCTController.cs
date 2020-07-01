using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace HZY.Admin.Controllers.Sys
{
    using HZY.Admin.Dto.Sys;
    using Microsoft.AspNetCore.Hosting;
    using HZY.Toolkit;
    using System.Text;
    using System.IO;
    using HZY.Admin.Services.Sys;
    using System.Threading;

    /// <summary>
    /// 代码创建 工具
    /// </summary>
    public class CCTController : ApiBaseController<CCTService>
    {
        private string _WebRootPath { get; } = string.Empty;

        public CCTController(Sys_MenuService _menuservice, CCTService _service, IWebHostEnvironment IWebHostEnvironment)
            : base("4ce21a81-1cae-44d2-b29e-07058ff04b3e", _menuservice, _service)
        {
            this._WebRootPath = IWebHostEnvironment.WebRootPath;
        }

        #region 页面 Views

        [HttpGet(nameof(Index))]
        public IActionResult Index() => View();

        #endregion

        /// <summary>
        /// 获取所有的 表名 及对应的 字段
        /// </summary>
        /// <returns></returns>
        [HttpPost(nameof(GetTableNameAndFields))]
        public async Task<ApiResult> GetTableNameAndFields()
            => this.ResultOk(await this.service.GetTableNameAndFields());

        /// <summary>
        /// 获取 Model 代码
        /// </summary>
        /// <param name="TableName"></param>
        /// <returns></returns>
        [HttpPost(nameof(GetModelCode) + "/{TableName}")]
        public async Task<ApiResult> GetModelCode([FromRoute]string TableName)
        {
            var TempUrl = _WebRootPath + "/Content/CodeTemp/Model.txt";

            if (!System.IO.File.Exists(TempUrl))
                MessageBox.Show("模板文件不存在");

            return this.ResultOk(data: await this.service.CreateModelCode(TableName, await System.IO.File.ReadAllTextAsync(TempUrl, Encoding.UTF8)));
        }

        /// <summary>
        /// 获取 注册 数据库 表 代码
        /// </summary>
        /// <returns></returns>
        [HttpPost(nameof(GetDbSetCode))]
        public async Task<ApiResult> GetDbSetCode()
            => this.ResultOk(data: await this.service.CreateDbSetCode());

        /// <summary>
        /// 获取 Service 代码
        /// </summary>
        /// <param name="TableName"></param>
        /// <returns></returns>
        [HttpPost(nameof(GetServicesCode) + "/{TableName}")]
        public async Task<ApiResult> GetServicesCode([FromRoute]string TableName)
        {
            var TempUrl = _WebRootPath + "/Content/CodeTemp/Services.txt";

            if (!System.IO.File.Exists(TempUrl))
                MessageBox.Show("模板文件不存在");

            return this.ResultOk(data: await this.service.CreateServiceCode(TableName, await System.IO.File.ReadAllTextAsync(TempUrl, Encoding.UTF8)));
        }

        /// <summary>
        /// 获取 Service 注入 代码
        /// </summary>
        /// <returns></returns>
        [HttpPost(nameof(GetServicesRegister))]
        public async Task<ApiResult> GetServicesRegister()
            => this.ResultOk(data: await this.service.CreateServicesRegister());

        /// <summary>
        /// 获取 控制器 代码
        /// </summary>
        /// <param name="TableName"></param>
        /// <returns></returns>
        [HttpPost(nameof(GetControllersCode) + "/{TableName}")]
        public async Task<ApiResult> GetControllersCode([FromRoute]string TableName)
        {
            var TempUrl = _WebRootPath + "/Content/CodeTemp/Controllers.txt";

            if (!System.IO.File.Exists(TempUrl))
                MessageBox.Show("模板文件不存在");

            return this.ResultOk(data: await this.service.CreateControllersCode(TableName, await System.IO.File.ReadAllTextAsync(TempUrl, Encoding.UTF8)));
        }

        /// <summary>
        /// 获取 Index.cshtml 代码
        /// </summary>
        /// <param name="TableName"></param>
        /// <returns></returns>
        [HttpPost(nameof(GetIndexCode) + "/{TableName}")]
        public async Task<ApiResult> GetIndexCode([FromRoute]string TableName)
        {
            var TempUrl = _WebRootPath + "/Content/CodeTemp/Index.txt";

            if (!System.IO.File.Exists(TempUrl))
                MessageBox.Show("模板文件不存在");

            return this.ResultOk(data: await this.service.CreateIndexCode(TableName, await System.IO.File.ReadAllTextAsync(TempUrl, Encoding.UTF8)));
        }

        /// <summary>
        /// 获取 Info.cshtml 代码
        /// </summary>
        /// <param name="TableName"></param>
        /// <param name="Fields"></param>
        /// <returns></returns>
        [HttpPost(nameof(GetInfoCode) + "/{TableName}")]
        public async Task<ApiResult> GetInfoCode([FromRoute]string TableName, [FromBody]List<string> Fields)
        {
            var TempUrl = _WebRootPath + "/Content/CodeTemp/Info.txt";

            if (!System.IO.File.Exists(TempUrl))
                MessageBox.Show("模板文件不存在");

            if (Fields == null || Fields.Count == 0)
            {
                Fields = new List<string>();
                var _Cols = await this.service.GetColsByTableNameAsync(TableName);
                foreach (var _Col in _Cols)
                {
                    Fields.Add($"{TableName}/{_Col.ColName}");
                }
            }

            return this.ResultOk(data: await this.service.CreateInfoCode(Fields, await System.IO.File.ReadAllTextAsync(TempUrl, Encoding.UTF8)));
        }

        /// <summary>
        /// 下载当前代码
        /// </summary>
        /// <returns></returns>
        [HttpPost(nameof(Download))]
        public IActionResult Download([FromBody] CCTDownloadDto downloadDto)
        {
            var Suffix = string.Empty;
            var Name = downloadDto.TableName;

            if (downloadDto.CodeType == "Model") Suffix = ".cs";
            if (downloadDto.CodeType == "Service") Suffix = "Service.cs";
            if (downloadDto.CodeType == "Controller") Suffix = "Controller.cs";
            if (downloadDto.CodeType == "Index")
            {
                Name = "Index";
                Suffix = ".cshtml";
            }
            if (downloadDto.CodeType == "Info")
            {
                Name = "Info";
                Suffix = ".cshtml";
            }

            var _Bytes = Encoding.UTF8.GetBytes(downloadDto.Content);
            return File(_Bytes, Tools.GetFileContentType[".cs"], $"{Name}{Suffix}");
        }

        /// <summary>
        /// 下载所有 代码
        /// </summary>
        /// <param name="CodeType">代码类型</param>
        /// <returns></returns>
        [HttpPost(nameof(DownloadAll) + "/{CodeType}")]
        public async Task<IActionResult> DownloadAll([FromRoute]string CodeType)
        {
            var Suffix = string.Empty;
            var Temp = string.Empty;
            var TempUrl = string.Empty;

            if (CodeType == "Model")
            {
                TempUrl = _WebRootPath + "/Content/CodeTemp/Model.txt";
            }

            if (CodeType == "Service")
            {
                TempUrl = _WebRootPath + "/Content/CodeTemp/Services.txt";
            }

            if (CodeType == "Controller")
            {
                TempUrl = _WebRootPath + "/Content/CodeTemp/Controllers.txt";
            }

            var isViews = CodeType == "Index" || CodeType == "Info";

            if (Directory.Exists($"{_WebRootPath}/Content/ZipFile/"))
            {
                Directory.CreateDirectory($"{_WebRootPath}/Content/ZipFile/");
            }

            if (!string.IsNullOrWhiteSpace(TempUrl)) Temp = await System.IO.File.ReadAllTextAsync(TempUrl, Encoding.UTF8);

            var _TableNames = await this.service.GetAllTableAsync();
            foreach (var item in _TableNames)
            {
                if (isViews)
                {
                    TempUrl = _WebRootPath + "/Content/CodeTemp/Index.txt";
                    Temp = await System.IO.File.ReadAllTextAsync(TempUrl, Encoding.UTF8);
                    await this.service.CreateAllFiles(this._WebRootPath, "Index", item.Name, Temp);
                    TempUrl = _WebRootPath + "/Content/CodeTemp/Info.txt";
                    Temp = await System.IO.File.ReadAllTextAsync(TempUrl, Encoding.UTF8);
                    await this.service.CreateAllFiles(this._WebRootPath, "Info", item.Name, Temp);
                }
                else
                {
                    await this.service.CreateAllFiles(this._WebRootPath, CodeType, item.Name, Temp);
                }
            }

            var path = $"{_WebRootPath}/Content/Codes/{CodeType}";
            var pathZip = $"{_WebRootPath}/Content/ZipFile/{CodeType}.zip";
            if (isViews)
            {
                path = $"{_WebRootPath}/Content/Codes/Views";
                pathZip = $"{_WebRootPath}/Content/ZipFile/Views.zip";
            }

            //开始压缩
            Zip zip = new Zip(path, pathZip);
            var bytes = await System.IO.File.ReadAllBytesAsync(pathZip);

            //删除文件
            if (System.IO.File.Exists(pathZip)) System.IO.File.Delete(pathZip);
            if (Directory.Exists(path)) Directory.Delete(path, true);

            return File(bytes, Tools.GetFileContentType[".zip"], $"{(isViews ? "Views" : CodeType)}.zip");
        }




    }
}
