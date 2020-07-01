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
using System.IO;
using System.Linq;

namespace HZY.Admin.Services.Core
{
    using HZY.EFCore.Base;
    using HZY.Toolkit;
    using NPOI.HSSF.UserModel;
    using NPOI.SS.UserModel;
    using Microsoft.AspNetCore.Http;
    using HZY.EFCore;
    using HZY.EFCore.Repository;
    using HZY.Toolkit.HzyNetCoreUtil.Attributes;
    using HZY.DapperCore.Dapper;

    [AppService(ServiceType.Scoped)]
    public class ServiceBase<T> : DefaultRepository<T>
        where T : class, new()
    {
        protected readonly EFCoreContext db;



        public ServiceBase( EFCoreContext _db)
            : base(_db)
        {
            this.db = _db;

           

        }

        #region 导出 Excel

        protected virtual byte[] ExportExcelByTableViewModel(TableViewModel tableViewModel)
        {
            HSSFWorkbook workbook = new HSSFWorkbook();
            ISheet sheet = workbook.CreateSheet();
            //数据
            var data = tableViewModel.DataSource;
            var cols = tableViewModel.Cols;
            //填充表头
            IRow dataRow = sheet.CreateRow(0);
            foreach (var item in cols)
            {
                if (!item.Show) continue;
                var index = cols.IndexOf(item);
                dataRow.CreateCell(index).SetCellValue(item.Title);
            }
            //填充内容
            for (int i = 0; i < data.Count; i++)
            {
                var item = data[i];
                dataRow = sheet.CreateRow(i + 1);
                foreach (var col in cols)
                {
                    if (!col.Show) continue;
                    var index = cols.IndexOf(col);
                    var value = item[col.DataIndex];
                    dataRow.CreateCell(index).SetCellValue(value == null ? "" : value.ToString());
                }
            }
            //填充byte
            using (MemoryStream ms = new MemoryStream())
            {
                workbook.Write(ms);
                return ms.ToArray();
            }
        }

        #endregion

        #region 上传文件

        /// <summary>
        /// 上传文件 辅助函数
        /// </summary>
        /// <param name="iFormFile"></param>
        /// <param name="webRootPath"></param>
        /// <param name="folder"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public string HandleUploadFile(IFormFile iFormFile, string webRootPath, string folder = "Files", params string[] format)
        {
            string ExtensionName = Path.GetExtension(iFormFile.FileName).ToLower().Trim();//获取后缀名

            if (format != null && format.Length > 0 && !format.Contains(ExtensionName.ToLower()))
            {
                throw new MessageBox("请上传后缀名为：" + string.Join("、", format) + " 格式的文件");
            }

            if (string.IsNullOrWhiteSpace(folder)) folder = "Files";

            var path = $"/AppUploadFile/{folder}";
            var fullPath = webRootPath + path;
            //webRootPath

            if (!Directory.Exists(fullPath))
            {
                Directory.CreateDirectory(fullPath);
            }

            path = $"{path}/{DateTime.Now.ToString("yyyyMMddHHmmss")}_{iFormFile.FileName}";
            fullPath = webRootPath + path;

            // 创建新文件
            using (FileStream fs = System.IO.File.Create(fullPath))
            {
                iFormFile.CopyTo(fs);
                // 清空缓冲区数据
                fs.Flush();
            }

            return path;
        }

        #endregion


    }
}