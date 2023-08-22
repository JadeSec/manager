using App.Domain.Entities;
using App.Infra.Bootstrap.Attributes;
using App.Infra.Implementation.Filter;
using App.Repositories;
using System.Threading.Tasks;

namespace App.Application.Project
{
    [Transient]
    public class Team
    {
        private readonly ProjectTeamRepository _projectTeamRep;

        public Team(ProjectTeamRepository projectTeamRepository)
        {
            _projectTeamRep = projectTeamRepository;
        }

        public async Task<bool> CreateAsync(ProjectTeamEntity entity)
            => await _projectTeamRep.CreateAsync(entity);
      
        public async Task<Paginate<ProjectTeamEntity>> GetAsync(string expression, int page)
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

            return await _projectTeamRep.GetAsync(filter);
        }
    }
}