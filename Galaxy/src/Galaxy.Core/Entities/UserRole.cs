using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Galaxy.Entities
{
    /// <summary>
    /// 用户角色表
    /// </summary>
    public class UserRole : Entity
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public virtual int UserId { get; set; }

        /// <summary>
        /// 角色Id
        /// </summary>
        public virtual int RoleId { get; set; }
    }
}
