using Abp.AutoMapper;
using Galaxy.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Galaxy.Users.Dto
{
    [AutoMapFrom(typeof(User))]
    public class RegisterUserDto
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }
    }
}
