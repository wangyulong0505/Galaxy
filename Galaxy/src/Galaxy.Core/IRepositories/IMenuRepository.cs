using Abp.Domain.Repositories;
using Galaxy.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Galaxy.IRepositories
{
    public interface IMenuRepository: IRepository<Menu>
    {
        Task<List<Menu>> GetUserPermissions(int Id);
    }
}
