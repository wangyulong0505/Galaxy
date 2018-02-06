using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Galaxy.Entities
{
    public class Permission : Entity
    {
        /// <summary>
        /// 权限名称
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 权限关联菜单Id
        /// </summary>
        public virtual int MenuId { get; set; }

        /// <summary>
        /// 权限关联菜单名称
        /// </summary>
        public virtual string MenuName { get; set; }

        /// <summary>
        /// 权限状态
        /// </summary>
        public virtual int Status { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        public virtual DateTime CreateDate { get; set; }
    }
}
