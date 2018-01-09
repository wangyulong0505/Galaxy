using Abp.Domain.Repositories;
using Galaxy.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Galaxy.IRepositories
{
    public interface IOrganizationRepository: IRepository<Organization>
    {
        Organization GetOrganizationById(int Id);
    }
}
