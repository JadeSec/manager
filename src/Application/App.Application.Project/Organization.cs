﻿using App.Domain.Entities;
using App.Infra.Bootstrap.Attributes;
using App.Infra.Implementation.Filter;
using App.Repositories;
using System.Threading.Tasks;

namespace App.Application.Project
{
    [Transient]
    public class Organization
    {
        private readonly ProjectOrganizationRepository _projectOrgRep;

        public Organization(ProjectOrganizationRepository projectOrganizationRepository)
        {
            _projectOrgRep = projectOrganizationRepository;
        }

        public async Task<Paginate<ProjectOrganizationEntity>> GetAsync(string expression, int page)
        {
            var filter = new Filter(expression, page, configuration: new Configuration
            {
                MaxPerPage = 10,
                ValueLength = 50,
                ExpressionMax = 1,
                Criteria = new Criteria[]
                {
                    new("name", new string[] { "=", "%"}),
                }
            });

            return await _projectOrgRep.GetAsync(filter);
        }
    }
}
