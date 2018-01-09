using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Galaxy.Entities
{
    public class Organization : Entity
    {
        /// <summary>
        /// 机构名称
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 机构编码
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// 机构类型： 一级部门， 二级部门， 三级部门
        /// </summary>
        public virtual int OrganizationType { get; set; }

        /// <summary>
        /// 层级编码
        /// </summary>
        public virtual string LevelCode { get; set; }

        /// <summary>
        /// 父节点Id
        /// </summary>
        public virtual int ParentNodeId { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public virtual int Status { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        public virtual DateTime CreateDate { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Remark { get; set; }
    }
}
