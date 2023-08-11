using App.Domain.Entities;
using Microsoft.Extensions.Logging;
using App.Infra.Integration.Database;
using App.Infra.Integration.Database.Providers;
using App.Infra.Implementation.Filter.Extensions;
using App.Infra.Implementation.Filter;
using System.Threading.Tasks;
using System.Linq;
using System;
using Microsoft.EntityFrameworkCore;

namespace App.Repositories
{
    public class ProjectRepository : RepositoryService<MySQLProvider, ProjectEntity>
    {
        readonly ILogger<ProjectRepository> _logger;

        public ProjectRepository(ILogger<ProjectRepository> logger)
        {
            _logger = logger;
        }

        public async Task<Paginate<ProjectEntity>> GetAsync(Filter filter)
        {
            var entity = Context.Set<ProjectEntity>();
            var query = entity.FilterLike("name", filter, (e, v) => e.Where(x => x.Name.ToLower().Contains(v)))
                              .FilterEqual<ProjectEntity, int>("id", filter, (e, v) => e.Where(x => x.Id.Equals(v)))
                              .FilterEqual<ProjectEntity, string>("name", filter, (e, v) => e.Where(x => x.Name.Equals(v)))
                              .FilterEqual<ProjectEntity, string>("created", filter, (e, v) => e.Where(x => x.Created.Date.Equals(DateTime.Parse(v).Date)))
                              .FilterEqual<ProjectEntity, string>("team", filter, (e, v) => e.Where(x => x.Team.Name.Equals(v)))
                              .FilterEqual<ProjectEntity, string>("org", filter, (e, v) => e.Where(x => x.Organization.Name.Equals(v)))
                              .Include(x => x.Team)
                              .Include(x => x.Organization)
                              .Select(x => new ProjectEntity()
                              {
                                  Id = x.Id,
                                  Name = x.Name,
                                  Created = x.Created,                                  
                                  Team = new()
                                  {
                                      Name = x.Team.Name                                     
                                  },
                                  Organization = new()
                                  {
                                      Name = x.Organization.Name                                   
                                  }
                              });

            var total = await query.CountAsync();
            var items = await query.AsNoTracking()
                                   .Skip(filter.Page)
                                   .Take(filter.Configuration.MaxPerPage)
                                   .ToListAsync();

            return new Paginate<ProjectEntity>(filter, items, total);
        }
    }
}
