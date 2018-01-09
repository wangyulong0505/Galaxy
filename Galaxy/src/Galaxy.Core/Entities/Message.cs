using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Galaxy.Entities
{
    public class Message : Entity
    {
        /// <summary>
        /// 消息标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// 发件人
        /// </summary>
        public string Sender { get; set; } 

        /// <summary>
        /// 发件人ID
        /// </summary>
        public int SenderId { get; set; }

        /// <summary>
        /// 抄送人员
        /// </summary>
        public string CarbonCopy { get; set; }

        /// <summary>
        /// 抄送人员ID
        /// </summary>
        public string CarbonCopyIds { get; set; }

        /// <summary>
        /// 收件人ID
        /// </summary>
        public string RecipientIds { get; set; }

        /// <summary>
        /// 收件人
        /// </summary>
        public string Recipients { get; set; }

        /// <summary>
        /// 消息状态：0草稿箱， 1发件箱， 2收件箱， 3回收站
        /// </summary>
        public int MessageStatus { get; set; }

        /// <summary>
        /// 消息类型： 0系统消息， 1邮件， 2短信， 3系统消息+邮件， 4系统消息+短信
        /// </summary>
        public int MessageType { get; set; }

        /// <summary>
        /// 消息标记： 0一般消息， 1重要消息
        /// </summary>
        public int Mark { get; set; }

        /// <summary>
        /// 是否已读
        /// </summary>
        public int IsRead { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
