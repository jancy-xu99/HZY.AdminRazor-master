using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HZY.Admin.Services.Act;
using HZY.Admin.Services.Sys;
using HZY.Models.Act;
using HZY.Toolkit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HZY.Admin.Controllers.Base
{
    using GYLib.Base.Utils;
    using HZY.DapperCore.Dapper;
    using HZY.Models.Act;
    using HZY.Toolkit.ToolKits;
    using Microsoft.AspNetCore.Server.Kestrel.Internal.System.Collections.Sequences;
    using Microsoft.Extensions.FileSystemGlobbing;
    using System.Text.RegularExpressions;

    public class ActivityController : ApiBaseController<ActivityService>
    {
        protected readonly IWebHostEnvironment webHostEnvironment;
        protected readonly string webRootPath;
        private readonly DapperClient dapper;
        public IActionResult Index()
        {
            return View();
        }

        //todo 
        //公司里要换 guid = CAA298AC - 104E-4D31-A303-08D7FBCAC280   DD9552F6-F439-47D8-6DE5-08D7FEE48E60
        /// <summary>
        ///  
        /// </summary>
        /// <param name="dapperFactory"></param>
        /// <param name="_menservice"></param>
        /// <param name="_service"></param>
        /// <param name="_webHostEnvironment"></param>
        public ActivityController(IDapperFactory dapperFactory, Sys_MenuService _menservice, ActivityService _service, IWebHostEnvironment _webHostEnvironment)
           : base("997BBD74-DD6D-41A9-A304-08D7FBCAC280", _menservice, _service)
        {
            this.webHostEnvironment = _webHostEnvironment;
            this.webRootPath = _webHostEnvironment.WebRootPath;
            //appsettings.json 和  Startup 下配置多个连接字符串
            //这样可以操作多个数据库
            dapper = dapperFactory.CreateClient("MSSQL1");
        }


        [HttpGet(nameof(ActIndex))]
        public IActionResult ActIndex() => View();

        [HttpGet("ActInfo/{Id?}")]
        public IActionResult ActInfo(int Id) => View((int)Id);


        [HttpGet("ActManage/{Id?}")]
        public IActionResult ActManage(int Id) => View((int)Id);




        [HttpPost("FindList/{Page}/{Rows}")]
        public async Task<ApiResult> FindListAsync(int Page, int Rows, [FromBody] Fct_Activity Search)
            => this.ResultOk(await this.service.FindListAsync(Page, Rows, Search));



        /// <summary>
        /// 保存数据
        /// </summary>
        /// <returns></returns>
        [HttpPost("Save")]//, Core.HZYAppCheckModel
        public async Task<ApiResult> SaveAsync([FromForm] Fct_Activity Model)
        {
            //Member
            var Files = new List<IFormFile>();
            //IFormFile Photo = null;
            if (Request.Form.Files.Count > 0)
            {
                Files = Request.Form.Files.Where(w => w.Name.Contains("Files")).ToList();
                //Photo = Request.Form.Files.FirstOrDefault(w => w.Name == "Photo");
            }


            return this.ResultOk(await dapper.InsertAsync(Model));// await this.service.SaveAsync(Model, this.webRootPath, Files));
        }

        /// <summary>
        /// 保存设计
        /// </summary>
        /// <returns></returns>
        [HttpPost("SaveDesign/{Id}")]//, Core.HZYAppCheckModel
        public async Task<ApiResult> SaveDesignAsync(int Id, [FromForm] Fct_Activity Model)
        {
            //Activity
            if (Model == null) return null;
            Model.Design_Introduce = SavaImgReplaceSrc(Model.Design_Introduce);
            var activity = new Fct_Activity() { ActId = Id, Design_Introduce = Model.Design_Introduce, ModifyTime = Model.ModifyTime };
            var Files = new List<IFormFile>();


            IFormFile Photo = null;
            if (Request.Form.Files.Count > 0)
            {
                Files = Request.Form.Files.Where(w => w.Name.Contains("Files")).ToList();
                Photo = Request.Form.Files.FirstOrDefault(w => w.Name == "Photo");
            }
            return this.ResultOk(await dapper.UpdateAsync<Fct_Activity>(activity));//, this.webRootPath, null));
        }

        public string SavaImgReplaceSrc(string content)
        {
            List<String> srcList = new List<String>();
            String img = "";
            Matcher m_image;
            String regEx_img = "<img[^>]*>";
            String regEx_backgroundimg = "url([^>]*)";
            string xmorign = "?x-oss-process=style/xmorient";
            content = content.Replace(xmorign, "");
            Regex rx = new Regex(regEx_img, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            Regex bgrx = new Regex(regEx_backgroundimg, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            MatchCollection mc = rx.Matches(content);
            MatchCollection bgmc = bgrx.Matches(content);
            if (mc.Count > 0)
            {
                foreach (Match image in mc)
                {
                    // 得到<img />数据
                    var m = new Regex("src\\s*=\\s*\"?(.*?)(\"|>|\\s+)", RegexOptions.Compiled).Match(image.Value);
                    if (m.Value.Contains("statics.xiumi.us"))
                    {
                        srcList.Add(m.Value);
                        var httpsrc = m.Value.Substring(m.Value.IndexOf("=\"") + 2, m.Value.LastIndexOf("\"") - m.Value.IndexOf("=\"") - 2);

                        var newvalue = WebClientHelper.DownLoadImages(httpsrc, this.webRootPath);
                        content = content.Replace(httpsrc, newvalue);
                    }
                }
            }
            if (bgmc.Count > 0)
            {
                foreach (Match image in bgmc)
                {
                    // 得到<img />数据
                    var m = new Regex("[a-zA-z]+://[^\\s]*[jpg|bmp|gif|ico|pcx|jpeg|tif|png|raw|tga]", RegexOptions.Compiled).Match(image.Value);
                    if (m.Value.Contains("statics.xiumi.us"))
                    {
                        srcList.Add(m.Value);
                        var httpsrc = m.Value.Substring(0, m.Value.LastIndexOf("&quot"));

                        var newvalue = WebClientHelper.DownLoadImages(httpsrc, this.webRootPath);
                        content = content.Replace(httpsrc, newvalue);
                    }
                }
            }



            //p_image = Pattern.compile(regEx_img, Pattern.CASE_INSENSITIVE);
            //m_image = p_image.matcher(content);
            //while (m_image.find())
            //{
            //    // 得到<img />数据
            //    img = m_image.group();
            //    // 匹配<img>中的src数据
            //    Matcher m = Pattern.compile("src\\s*=\\s*\"?(.*?)(\"|>|\\s+)").matcher(img);
            //    List<String> imgList = new ArrayList<String>();
            //    while (m.find())
            //    {
            //        imgList.add(m.group(1));
            //    }
            //    //包含秀米服务器的图片放入srcList做处理
            //    if (imgList.get(0).contains("statics.xiumi.us"))
            //    {
            //        srcList.add(imgList.get(0));
            //    }
            //}
            return content;
        }



        /// <summary>
        /// 根据Id 加载表单数据
        /// </summary>
        /// <param name="ActId"></param>
        /// <returns></returns>
        [HttpPost("LoadForm/{Id?}")]
        public async Task<ApiResult> LoadFormAsync(int Id)
            => this.ResultOk(await this.service.LoadFormAsync(Id));


    }
}