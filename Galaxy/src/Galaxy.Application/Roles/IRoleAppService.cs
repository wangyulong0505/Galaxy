using Abp.Application.Services;
using Galaxy.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Galaxy.Roles
{
    public interface IRoleAppService : IApplicationService
    {
        Task<List<Role>> GetRoles();

        Task PutRole(Role entity);

        Task PostRole(Role entity);

        Task DeleteRole(int Id);
    }
}
