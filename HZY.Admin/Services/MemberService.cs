using System;
using System.Collections.Generic;
using System.Text;

namespace HZY.Admin.Services
{
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using Microsoft.AspNetCore.Http;
    using HZY.EFCore.Repository;
    using HZY.Models.Sys;
    using HZY.Toolkit;
    using System.Linq;
    using HZY.EFCore.Base;
    using HZY.Admin.Services.Core;
    using HZY.EFCore;
    using HZY.Models;
    using HZY.Admin.Dto;

    public class MemberService : ServiceBase<Member>
    {
        protected readonly DefaultRepository<Sys_User> userDb;

        public MemberService(EFCoreContext _db, DefaultRepository<Sys_User> _userDb)
            : base(_db)
        {
            this.userDb = _userDb;
        }

        #region CURD 基础

        /// <summary>
        /// 获取数据源
        /// </summary>
        /// <param name="Page"></param>
        /// <param name="Rows"></param>
        /// <param name="Search"></param>
        /// <returns></returns>
        public async Task<TableViewModel> FindListAsync(int Page, int Rows, Member Search)
        {
            var query = (
                from member in db.Members
                from user in db.Sys_Users.Where(w => w.User_ID == member.Member_UserID).DefaultIfEmpty()
                select new { member, user.User_Name }
                )
                .WhereIF(w => w.member.Member_Name.Contains(Search.Member_Name), !string.IsNullOrWhiteSpace(Search?.Member_Name))
                .OrderBy(w => w.member.Member_Num)
                .Select(w => new
                {
                    w.member.Member_Num,
                    w.member.Member_Photo,
                    w.member.Member_Name,
                    w.member.Member_Phone,
                    w.member.Member_Sex,
                    Member_Birthday = w.member.Member_Birthday.ToString("yyyy-MM-dd"),
                    操作人 = w.User_Name,
                    Member_CreateTime = w.member.Member_CreateTime.ToString("yyyy-MM-dd"),
                    _ukid = w.member.Member_ID
                })
                ;

            return await this.db.AsTableViewModelAsync(query, Page, Rows, typeof(Member));
        }

        /// <summary>
        /// 新增\修改
        /// </summary>
        /// <param name="model"></param>
        /// <param name="webRootPath"></param>
        /// <param name="Photo"></param>
        /// <param name="Files"></param>
        /// <returns></returns>
        public async Task<Guid> SaveAsync(Member model, string webRootPath, IFormFile Photo, List<IFormFile> Files)
        {
            if (Photo != null)
            {
                model.Member_Photo = this.HandleUploadFile(Photo, webRootPath, null, ".jpg", ".jpeg", ".png", "gif");
            }

            if (Files.Count > 0)
            {
                var path = new List<string>();
                foreach (var item in Files)
                {
                    path.Add(this.HandleUploadFile(item, webRootPath));
                }
                if (path.Count > 0) model.Member_FilePath = string.Join(",", path);
            }

            await this.InsertOrUpdateAsync(model);

            return model.Member_ID;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="Ids"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(List<Guid> Ids)
            => await this.DeleteAsync(w => Ids.Contains(w.Member_ID));

        /// <summary>
        /// 加载表单 数据
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<Dictionary<string, object>> LoadFormAsync(Guid Id)
        {
            var res = new Dictionary<string, object>();

            var Model = await this.FindByIdAsync(Id);
            var User = await userDb.FindByIdAsync(Model?.Member_UserID);

            res[nameof(Id)] = Id;
            res[nameof(Model)] = Model.ToNewByNull();
            res[nameof(User)] = User.ToNewByNull();

            return res;
        }


        #endregion





    }
}
