using App.Domain.Entities;
using App.Infra.Bootstrap.Attributes;
using App.Infra.Implementation.Filter;
using App.Repositories;
using System.Threading.Tasks;

namespace App.Application.Project
{
    [Transient]
    public class Group
    {
        private readonly ProjectGroupRepository _projectGroupRep;

        public Group(ProjectGroupRepository projectGroupRepository)
        {
            _projectGroupRep = projectGroupRepository;
        }

        public async Task<Paginate<ProjectGroupEntity>> GetAsync(string expression, int page)
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

            return await _projectGroupRep.GetAsync(filter);
        }
    }
}