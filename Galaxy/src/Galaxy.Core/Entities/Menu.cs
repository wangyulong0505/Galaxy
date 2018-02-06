using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Galaxy.Entities
{
    public class Menu : Entity
    {
        /// <summary>
        /// 菜单名称
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 菜单链接
        /// </summary>
        public virtual string URL { get; set; }
        /// <summary>
        /// 父菜单Id
        /// </summary>
        public virtual int ParentNodeId { get; set; }

        /// <summary>
        /// 父菜单名称
        /// </summary>
        public virtual string ParentNodeName { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// 层级编码：用来同级排序
        /// </summary>
        public virtual string LevelCode { get; set; }

        /// <summary>
        /// 
        /// 菜单类型：目录，菜单，按钮
        /// </summary>
        public virtual int MenuType { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        public virtual string MenuIcon { get; set; }

        /// <summary>
        /// 状态： 0：正常， 1：禁用
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
