using System;
using System.Collections.Generic;
using System.Text;

namespace HZY.Admin.Services.Sys
{
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using Microsoft.AspNetCore.Http;
    using HZY.EFCore.Repository;
    using HZY.Admin.Services.Core;
    using HZY.EFCore;
    using System.Linq;
    using System.IO;
    using Microsoft.AspNetCore.Hosting;
    using HZY.Toolkit;
    using HZY.Models.Sys;
    using HZY.EFCore.Base;

    public class CCTService : ServiceBase<Sys_Menu>
    {
        public CCTService(EFCoreContext _db)
            : base(_db)
        {

        }

        public Task<List<TABLE_NAME>> GetAllTableAsync()
          => db.GetAllTableAsync();

        public Task<List<TABLES_COLUMNS>> GetColsByTableNameAsync(string TableName)
            => db.GetColsByTableNameAsync(TableName);

        /// <summary>
        /// 获取表名和字段
        /// </summary>
        /// <returns></returns>
        public async Task<Dictionary<string, List<TABLES_COLUMNS>>> GetTableNameAndFields()
        {
            var _TableNames = await db.GetAllTableAsync();

            var dic = new Dictionary<string, List<TABLES_COLUMNS>>();
            var _TableAll = ModelCache.All();

            for (int i = 0; i < _TableNames.Count; i++)
            {
                var item = _TableNames[i];
                var _Cols = await db.GetColsByTableNameAsync(item.Name);

                List<ModelInfo> fieldInfoList = null;
                if (_TableAll.ContainsKey(item.Name))
                {
                    fieldInfoList = _TableAll[item.Name].ToList();
                }

                foreach (var _Col in _Cols)
                {
                    var _FieldDescribe = fieldInfoList?.FirstOrDefault(w => w.Name == _Col.ColName);
                    if (_FieldDescribe != null) _Col.ColRemark = _FieldDescribe.Remark;
                }
                dic[item.Name] = _Cols;
            }

            return dic;
        }

        /// <summary>
        /// 创建 Model 代码
        /// </summary>
        /// <param name="TableName"></param>
        /// <param name="Temp"></param>
        /// <returns></returns>
        public async Task<string> CreateModelCode(string TableName, string Temp)
        {
            var _Cols = await db.GetColsByTableNameAsync(TableName);

            var _TableAll = ModelCache.All();
            List<ModelInfo> fieldInfoList = null;
            if (_TableAll.ContainsKey(TableName))
            {
                fieldInfoList = _TableAll[TableName].ToList();
            }

            var _Code = Temp.ToString();
            var _ClassName = TableName;
            var _Fields = new StringBuilder();

            foreach (var item in _Cols)
            {
                var _Type = string.Empty;
                var _Key = item.ColIsKey;
                switch (item.ColType)
                {
                    case "uniqueidentifier":
                        _Type = _Key == 1 ? "Guid" : "Guid?";
                        break;
                    case "bit":
                    case "int":
                        _Type = _Key == 1 ? "int" : "int?";
                        break;
                    case "datetime":
                        _Type = "DateTime?";
                        break;
                    case "float":
                        _Type = "float?";
                        break;
                    case "money":
                        _Type = "double?";
                        break;
                    case "decimal":
                        _Type = "decimal?";
                        break;
                    default:
                        _Type = "string";
                        break;
                }

                if (_Key == 1)
                {
                    _Fields.Append($"\t[Key]\r\n");
                }
                else
                {
                    if (item.ColName.Contains("_CreateTime") && _Type == "DateTime?")
                    {
                        _Fields.Append($@"
        /// <summary>
        /// 创建时间
        /// </summary>
");
                        _Fields.Append("\t[DatabaseGenerated(DatabaseGeneratedOption.Computed)]\r\n");
                    }
                    else
                    {
                        if (fieldInfoList == null)
                        {
                            if (string.IsNullOrWhiteSpace(item.ColRemark))
                            {
                                _Fields.Append($@"
        /// <summary>
        /// 请设置 {item.ColName} 的显示名称 => 备注:{item.ColName}
        /// </summary>
");
                            }
                            else
                            {
                                _Fields.Append($@"
        /// <summary>
        /// {item.ColRemark} => 备注:{item.ColName}
        /// </summary>
");
                            }
                        }
                        else
                        {
                            var _FieldDescribe = fieldInfoList.FirstOrDefault(w => w.Name == item.ColName);
                            if (_FieldDescribe == null)
                            {
                                _Fields.Append($@"
        /// <summary>
        /// 请设置 {item.ColName} 的显示名称 => 备注:{item.ColName}
        /// </summary>
");
                            }
                            else
                            {
                                _Fields.Append($@"
        /// <summary>
        /// {_FieldDescribe.Remark} => 备注:{item.ColName}
        /// </summary>
");
                            }

                        }

                    }
                }
                if (_Key == 1 && (_Type == "Guid" || _Type == "Guid?"))
                {
                    _Fields.Append($"\tpublic {_Type} {item.ColName} {{ get; set; }} = Guid.Empty;\r\n");

                }
                else
                {
                    _Fields.Append($"\tpublic {_Type} {item.ColName} {{ get; set; }}\r\n");
                }
            }

            _Code = _Code.Replace("<#ClassName#>", _ClassName);
            _Code = _Code.Replace("<#TableName#>", TableName);
            _Code = _Code.Replace("<#Fields#>", _Fields.ToString());

            return _Code.ToString();
        }

        /// <summary>
        /// 生产 DbSet 代码
        /// </summary>
        /// <returns></returns>
        public async Task<string> CreateDbSetCode()
        {
            StringBuilder _StringBuilder = new StringBuilder();
            var _TableNames = await db.GetAllTableAsync();
            foreach (var item in _TableNames)
            {
                _StringBuilder.Append($@"public DbSet<{item.Name}> {item.Name}s {{ get; set; }}
");
            }
            return _StringBuilder.ToString();
        }

        /// <summary>
        /// 创建 Logic 代码
        /// </summary>
        /// <param name="TableName"></param>
        /// <param name="Temp"></param>
        /// <returns></returns>
        public async Task<string> CreateServiceCode(string TableName, string Temp)
        {
            var _Cols = await db.GetColsByTableNameAsync(TableName);

            var _Code = Temp.ToString();
            var _ClassName = TableName;
            var _KeyName = _Cols.Find(w => w.ColIsKey == 1);
            //
            var _NameCol = _Cols.Count > 2 ? _Cols[1] : null;
            var _Name = _NameCol == null ? "" : _NameCol.ColName;

            var _Select = _Cols.FindAll(w => w.ColIsKey == 0);

            var codeList = new List<string>();
            if (_Select != null && _Select.Count > 0)
            {
                foreach (var item in _Select.Select(w => w.ColName))
                {
                    codeList.Add($"w.{item}");
                }
                if (_KeyName != null) codeList.Add($"_ukid = w.{ _KeyName.ColName}");
            }

            _Code = _Code.Replace("<#Select#>", string.Join(",", codeList));
            _Code = _Code.Replace("<#ClassName#>", _ClassName);
            _Code = _Code.Replace("<#className#>", _ClassName.First().ToString().ToLower() + _ClassName.Substring(1));
            _Code = _Code.Replace("<#TableName#>", TableName);
            _Code = _Code.Replace("<#KeyName#>", _KeyName?.ColName);
            //_Code = _Code.Replace("<#QueryCode#>", _QueryCode.ToString());

            return _Code.ToString();
        }

        /// <summary>
        /// Services Register
        /// </summary>
        /// <returns></returns>
        public async Task<string> CreateServicesRegister()
        {
            StringBuilder _StringBuilder = new StringBuilder();
            var _TableNames = await db.GetAllTableAsync();
            foreach (var item in _TableNames)
            {
                _StringBuilder.Append($@"services.AddScoped<{item.Name}Service>();
");
            }
            return _StringBuilder.ToString();
        }

        /// <summary>
        /// 创建 Controllers 代码
        /// </summary>
        /// <param name="TableName"></param>
        /// <param name="Temp"></param>
        /// <returns></returns>
        public async Task<string> CreateControllersCode(string TableName, string Temp)
        {
            //var _Cols = await this.GetColsByTableName(TableName);
            var _Code = Temp.ToString();

            var _ClassName = TableName;

            _Code = _Code.Replace("<#ClassName#>", _ClassName);
            _Code = _Code.Replace("<#TableName#>", TableName);

            return await Task.FromResult(_Code.ToString());
        }

        /// <summary>
        /// 创建 Index.cshtml 代码
        /// </summary>
        /// <param name="TableName"></param>
        /// <param name="Temp"></param>
        /// <returns></returns>
        public async Task<string> CreateIndexCode(string TableName, string Temp)
        {
            var _Code = Temp.ToString();

            var _ClassName = TableName;

            _Code = _Code.Replace("<#ClassName#>", _ClassName);
            _Code = _Code.Replace("<#TableName#>", TableName);

            return await Task.FromResult(_Code.ToString());
        }

        /// <summary>
        /// 创建 Info.cshtml 代码
        /// </summary>
        /// <param name="Fields"></param>
        /// <param name="Temp"></param>
        /// <returns></returns>
        public async Task<string> CreateInfoCode(List<string> Fields, string Temp)
        {
            StringBuilder _Codes = new StringBuilder();
            await Task.Run(() =>
            {
                var _TableAll = ModelCache.All();

                foreach (var item in Fields)
                {
                    var _TableName = item.Split('/')[0];
                    var _FieldName = item.Split('/')[1];

                    List<ModelInfo> fieldInfos = null;
                    if (_TableAll.ContainsKey(_TableName))
                    {
                        fieldInfos = _TableAll[_TableName].ToList();
                    }

                    if (fieldInfos == null) continue;

                    var fieldInfo = fieldInfos.FirstOrDefault(w => w.Name == _FieldName);
                    if (fieldInfo == null) continue;

                    _Codes.Append(@$"
            <div class=""col-sm-12"">
                <h4 class=""example-title"">{fieldInfo.Remark}</h4>
                <input type=""text"" class=""form-control"" placeholder=""请输入 {fieldInfo.Remark}"" v-model=""form.vm.Model.{fieldInfo.Name}"" autocomplete=""off"" />
            </div>
");
                }
            });
            Temp = Temp.Replace("<#Form#>", _Codes.ToString());
            return Temp;
        }

        /// <summary>
        /// 创建文件
        /// </summary>
        /// <returns></returns>
        public async Task<string> CreateAllFiles(string webRootPath, string CodeType, string TableName, string Temp)
        {
            var path = $"{webRootPath}/Content/Codes/";

            if (CodeType == "Model")
            {
                var code = await this.CreateModelCode(TableName, Temp);
                path += $"{CodeType}";
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                await File.WriteAllTextAsync($"{path}/{TableName}.cs", code, Encoding.UTF8);
            }

            if (CodeType == "Service")
            {
                var code = await this.CreateServiceCode(TableName, Temp);
                path += $"{CodeType}";
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                await File.WriteAllTextAsync($"{path}/{TableName}Service.cs", code, Encoding.UTF8);
            }

            if (CodeType == "Controller")
            {
                var code = await this.CreateControllersCode(TableName, Temp);
                path += $"{CodeType}";
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                await File.WriteAllTextAsync($"{path}/{TableName}Controller.cs", code, Encoding.UTF8);
            }

            if (CodeType == "Index")
            {
                var code = await this.CreateIndexCode(TableName, Temp);
                path += $"Views";
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                path += $"/{TableName}";
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                await File.WriteAllTextAsync($"{path}/Index.cshtml", code, Encoding.UTF8);
            }

            if (CodeType == "Info")
            {
                //获取表下面的所有 字段
                var _Cols = await this.db.GetColsByTableNameAsync(TableName);
                var list = new List<string>();
                foreach (var _Col in _Cols)
                {
                    list.Add($"{TableName}/{_Col.ColName}");
                }
                var code = await this.CreateInfoCode(list, Temp);
                path += $"Views";
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                path += $"/{TableName}";
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                await File.WriteAllTextAsync($"{path}/Info.cshtml", code, Encoding.UTF8);
            }

            return path;
        }














    }
}
