using Galaxy.IRepositories;
using System;
using System.Collections.Generic;
using System.Text;
using Galaxy.Entities;
using Abp.EntityFrameworkCore;
using System.Linq;
using System.Text.RegularExpressions;

namespace Galaxy.EntityFrameworkCore.Repositories
{
    public class UserRepository : GalaxyRepositoryBase<User>, IUserRepository
    {
        public UserRepository(IDbContextProvider<GalaxyDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

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
    }
}
