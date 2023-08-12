using App.Domain.Entities;
using App.Infra.Implementation.Filter;
using App.Infra.Implementation.Filter.Extensions;
using App.Infra.Integration.Database;
using App.Infra.Integration.Database.Providers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace App.Repositories
{
    public class ProjectGroupRepository : RepositoryService<MySQLProvider, ProjectGroupEntity>
    {
        readonly ILogger<ProjectGroupRepository> _logger;

        public ProjectGroupRepository(ILogger<ProjectGroupRepository> logger)
        {
            _logger = logger;
        }

        public async Task<Paginate<ProjectGroupEntity>> GetAsync(Filter filter)
        {
            var entity = Context.Set<ProjectGroupEntity>();
            var query = entity.FilterEqual<ProjectGroupEntity, string>("name", filter, (e, v) => e.Where(x => x.Name.Equals(v)))
                              .FilterLike("name", filter, (e, v) => e.Where(x => x.Name.ToLower().Contains(v)))
                              .Select(x => new ProjectGroupEntity()
                              {
                                  Id = x.Id,
                                  Name = x.Name,
                                  Created = x.Created
                              });

            var total = await query.CountAsync();
            var items = await query.AsNoTracking()
                                   .Skip(filter.Page)
                                   .Take(filter.Configuration.MaxPerPage)
                                   .OrderByDescending(x => x.Created)
                                   .ToListAsync();

            return new Paginate<ProjectGroupEntity>(filter, items, total);
        }
    }
}
