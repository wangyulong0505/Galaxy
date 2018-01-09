using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Galaxy.Entities
{
    public class User : Entity
    {
        /// <summary>
        /// 登录用户名
        /// </summary>
        public virtual string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public virtual string Password { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 用户头像
        /// </summary>
        public virtual string Avatar { get; set; }

        /// <summary>
        /// 生日
        /// </summary>
        public virtual DateTime Birthday { get; set; }

        /// <summary>
        /// 部门ID
        /// </summary>
        public virtual int DepartmentId { get; set; }

        /// <summary>
        /// 座机
        /// </summary>
        public virtual string Telephone { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public virtual string Email { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public virtual int Gender { get; set; }

        /// <summary>
        /// 手机
        /// </summary>
        public virtual string Phone { get; set; }

        /// <summary>
        /// QQ
        /// </summary>
        public virtual string QQ { get; set; }

        /// <summary>
        /// 微信
        /// </summary>
        public virtual string WeChat { get; set; }

        /// <summary>
        /// 微博
        /// </summary>
        public virtual string Weibo { get; set; }

        /// <summary>
        /// Github
        /// </summary>
        public virtual string Github { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        public virtual DateTime CreateDate { get; set; }

        /// <summary>
        /// 状态： 0：正常； 1：禁用； 2：已删除
        /// </summary>
        public virtual int Status { get; set; }
    }
}
