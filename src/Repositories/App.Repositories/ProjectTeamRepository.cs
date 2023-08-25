using System.Linq;
using System.Threading.Tasks;
using App.Domain.Entities;
using App.Infra.Implementation.Filter;
using App.Infra.Implementation.Filter.Extensions;
using App.Infra.Integration.Database;
using App.Infra.Integration.Database.Providers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace App.Repositories
{
    public class ProjectTeamRepository : RepositoryService<MySQLProvider, ProjectTeamEntity>
    {
        readonly ILogger<ProjectTeamRepository> _logger;

        public ProjectTeamRepository(ILogger<ProjectTeamRepository> logger)
        {
            _logger = logger;
        }

        public async Task<ProjectTeamEntity> GetAsync(int id)
            => await base.Where(x => x.Id.Equals(id)).FirstOrDefaultAsync();

        public async Task<Paginate<ProjectTeamEntity>> GetAsync(Filter filter)
        {
            var entity = Context.Set<ProjectTeamEntity>();
            var query = entity.FilterEqual<ProjectTeamEntity, string>("name", filter, (e, v) => e.Where(x => x.Name.Equals(v)))
                              .FilterLike("name", filter, (e, v) => e.Where(x => x.Name.ToLower().Contains(v)))                                                            
                              .Select(x => new ProjectTeamEntity()
                              {
                                  Id = x.Id,
                                  Name = x.Name,
                                  Icon = x.Icon,
                                  Description = x.Description,              
                                  Created = x.Created                                  
                              });

            var total = await query.CountAsync();
            var items = await query.AsNoTracking()
                                   .Skip(filter.Page)
                                   .Take(filter.Configuration.MaxPerPage)
                                   .OrderByDescending(x => x.Created)
                                   .ToListAsync();

            return new Paginate<ProjectTeamEntity>(filter, items, total);
        }
    }
}
