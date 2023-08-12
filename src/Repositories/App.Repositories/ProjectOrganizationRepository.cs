using App.Domain.Entities;
using App.Infra.Implementation.Filter;
using App.Infra.Implementation.Filter.Extensions;
using App.Infra.Integration.Database;
using App.Infra.Integration.Database.Providers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace App.Repositories
{
    public class ProjectOrganizationRepository : RepositoryService<MySQLProvider, ProjectOrganizationEntity>
    {
        readonly ILogger<ProjectOrganizationRepository> _logger;

        public ProjectOrganizationRepository(ILogger<ProjectOrganizationRepository> logger)
        {
            _logger = logger;
        }

        public async Task<Paginate<ProjectOrganizationEntity>> GetAsync(Filter filter)
        {
            var entity = Context.Set<ProjectOrganizationEntity>();
            var query = entity.FilterEqual<ProjectOrganizationEntity, string>("name", filter, (e, v) => e.Where(x => x.Name.Equals(v)))
                              .FilterLike("name", filter, (e, v) => e.Where(x => x.Name.ToLower().Contains(v)))                                                            
                              .Select(x => new ProjectOrganizationEntity()
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

            return new Paginate<ProjectOrganizationEntity>(filter, items, total);
        }
    }
}
