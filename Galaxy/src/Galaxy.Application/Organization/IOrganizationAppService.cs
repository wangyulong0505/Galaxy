using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Galaxy.Organization
{
    public interface IOrganizationAppService: IApplicationService
    {
        List<Entities.Organization> GetOrganizationList();

        void PutOrganization(Entities.Organization entity);

        void PostOrganization(Entities.Organization entity);

        void DeleteOrganization(Entities.Organization entity);

        Entities.Organization GetOrganizationById(int Id);
    }
}
