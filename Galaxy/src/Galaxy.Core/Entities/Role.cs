using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Galaxy.Entities
{
    public class Role : Entity
    {
        /// <summary>
        /// 角色名
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public virtual int Status { get; set; }

        /// <summary>
        /// 代码/编码
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Remark { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public virtual string Sort { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        public virtual DateTime CreateDate { get; set; }

        /// <summary>
        /// 角色拥有权限Id, 一个角色可以有多个权限, 用逗号隔开 
        /// </summary>
        public virtual string PermissionIds { get; set; }
    }
}
