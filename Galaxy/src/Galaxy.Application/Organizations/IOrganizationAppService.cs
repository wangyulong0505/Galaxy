using Abp.Application.Services;
using Galaxy.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Galaxy.Organizations
{
    public interface IOrganizationAppService: IApplicationService
    {
        Task<List<Organization>> GetOrganizations();

        Task PutOrganization(Organization entity);

        Task PostOrganization(Organization entity);

        Task DeleteOrganization(int Id);

        Task<Organization> GetOrganization(int Id);
    }
}
