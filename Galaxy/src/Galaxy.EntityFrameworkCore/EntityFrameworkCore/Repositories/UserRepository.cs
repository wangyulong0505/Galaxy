using Galaxy.IRepositories;
using System;
using System.Collections.Generic;
using System.Text;
using Galaxy.Entities;
using Abp.EntityFrameworkCore;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Galaxy.EntityFrameworkCore.Repositories
{
    public class UserRepository : GalaxyRepositoryBase<User>, IUserRepository
    {
        public UserRepository(IDbContextProvider<GalaxyDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        #region 登录验证

        /// <summary>
        /// 验证登录的状态 0：成功， 1：用户名或邮箱不存在， 2：密码错误， 3：用户名或邮箱为空
        /// </summary>
        /// <param name="usernameOrEmailAddress"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public int GetLoginStatus(string usernameOrEmailAddress, string password)
        {
            if (!string.IsNullOrEmpty(usernameOrEmailAddress))
            {
                Regex emailRegex = new Regex(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$");
                if (emailRegex.IsMatch(usernameOrEmailAddress))
                {
                    //按照邮箱的验证规则
                    User entity = GetAll().SingleOrDefault(q => q.Email == usernameOrEmailAddress);
                    if (entity == null)
                    {
                        return 1;
                    }
                    //密码Md5加密
                    string encryptPassword = TouchScreen.Core.CommonFuns.Encrypt.Md5(password).ToUpper();
                    if (encryptPassword == entity.Password)
                    {
                        return 0;
                    }
                }
                else
                {
                    //按照用户名的验证规则
                    User entity = GetAll().SingleOrDefault(q => q.UserName == usernameOrEmailAddress);
                    if (entity == null)
                    {
                        return 1;
                    }
                    //密码Md5加密
                    string encryptPassword = TouchScreen.Core.CommonFuns.Encrypt.Md5(password).ToUpper();
                    if (encryptPassword == entity.Password)
                    {
                        return 0;
                    }
                }
                return 2;
            }
            return 3;
        }

        #endregion

        public List<User> GetPagingUsers(int pageIndex, int pageSize, string strKey, out int pageCount, out int itemCount)
        {
            //strKey的格式类似于 name=wangshibang|username=admin
            IQueryable<User> allQueryable = GetQueryableByKeys(strKey);
            itemCount = allQueryable.ToList().Count;
            pageCount = itemCount % pageSize == 0 ? (itemCount / pageSize) : (itemCount / pageSize) + 1;

            //pageSize位-1时默认获取全部
            if (pageSize == -1)
            {
                return allQueryable.ToList();
            }

            return allQueryable.Skip(pageIndex - 1).Take(pageSize).ToList();
        }

        /// <summary>
        /// 获取所有用户列表
        /// </summary>
        /// <returns></returns>
        public List<User> GetUsers()
        {
            return GetAll().Where(q => q.Status == 0 && q.UserName != "admin").OrderBy(q=>q.Id).ToList();
        }

        /// <summary>
        /// 根据查询关键字获取所有集合
        /// </summary>
        /// <param name="strKey"></param>
        /// <returns></returns>
        public IQueryable<User> GetQueryableByKeys(string strKey)
        {
            if (!string.IsNullOrEmpty(strKey))
            {
                if (strKey.Split('|').Length > 1)
                {
                    string strName = strKey.Split('|')[0].Replace("name=", "");
                    string strUserName = strKey.Split('|')[1].Replace("username=", "");
                    return GetAll().Where(q => q.Status == 0 && q.UserName != "admin" && q.Name.Contains(strName) && q.UserName.Contains(strUserName)).OrderBy(q => q.Id);
                }
                else
                {
                    if (strKey.Contains("name"))
                    {
                        string key = strKey.Replace("name=", "");
                        return GetAll().Where(q => q.Status == 0 && q.UserName != "admin" && q.Name.Contains(key)).OrderBy(q => q.Id);
                    }
                    else
                    {
                        string key = strKey.Replace("username=", "");
                        return GetAll().Where(q => q.Status == 0 && q.UserName != "admin" && q.UserName.Contains(key)).OrderBy(q => q.Id);
                    }
                }
            }
            else
            {
                return GetAll().Where(q => q.Status == 0 && q.UserName != "admin").OrderBy(q => q.Id);
            }
        }
    }
}
