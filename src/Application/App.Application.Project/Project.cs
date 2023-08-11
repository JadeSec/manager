using App.Repositories;
using App.Domain.Entities;
using System.Threading.Tasks;
using App.Infra.Bootstrap.Attributes;
using App.Infra.Implementation.Filter;

namespace App.Application.Project
{
    [Transient]
    public class Project
    {
        private readonly ProjectRepository _projectRep;

        public Project(ProjectRepository projectRepository)
        {
            _projectRep = projectRepository;
        }

        public async Task<Paginate<ProjectEntity>> GetAsync(string expression, int page)
        {
            var filter = new Filter(expression, page, configuration: new Configuration
            {
                MaxPerPage = 10,
                ValueLength = 255,
                ExpressionMax = 5,
                Criteria = new Criteria[]
                {
                    new("id",new string[] { "=" }),
                    new("name", new string[] { "=", "%"}),
                    new("team",new string[] { "=" }),
                    new("org",new string[] { "=" }),                    
                    new("provider",new string[] { "=" }),
                }
            });

            return await _projectRep.GetAsync(filter);
        }
    }
}