using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Galaxy.Entities
{
    /// <summary>
    /// 角色权限表
    /// </summary>
    public class RolePermission : Entity
    {
        /// <summary>
        /// 角色Id
        /// </summary>
        public virtual int RoleId { get; set; }

        /// <summary>
        /// 权限Id，这里应该对应Menu表的MenuId,字符串用逗号分隔
        /// </summary>
        public virtual string PermissionIds { get; set; }
    }
}
