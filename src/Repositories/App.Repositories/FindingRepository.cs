using App.Domain.Entities;
using Microsoft.Extensions.Logging;
using App.Infra.Integration.Database;
using App.Infra.Integration.Database.Providers;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;
using App.Domain.Models;
using App.Infra.Implementation.Filter;
using App.Infra.Implementation.Filter.Extensions;

namespace App.Repositories
{
    public class FindingRepository : RepositoryService<MySQLProvider, FindingEntity>
    {
        readonly ILogger<FindingRepository> _logger;

        public FindingRepository(ILogger<FindingRepository> logger)
        {
            _logger = logger;
        }
      
        bool SeverityIn(FindingEntity finding, string value)
        {
            return finding.Severity.Name.Contains(value);
        }

        public async Task<PaginateModel<FindingEntity>> GetWithFilterAsync(Filter filter)
        {
            var entity = Context.Set<FindingEntity>();            

            var query =  entity.FilterEqual<FindingEntity, float>(filter, "id", (e, v) => e.Where(x => x.Id == v));            

            var items = await query.Include(x => x.Severity)
                                   .Include(x => x.Status)
                                   .Include(x => x.Tool)
                                   .Include(x => x.Tool.Type)
                                   .Include(x => x.Project.Team)
                                   .Include(x => x.Project.Organization)
                                   .Select(x => new FindingEntity()
                                   {
                                       Id = x.Id,
                                       Title = x.Title,
                                       Created = x.Created,
                                       Severity = new() { Name = x.Severity.Name, Sla = x.Severity.Sla },
                                       Status = new() { Name = x.Status.Name },
                                       Tool = new()
                                       {
                                           Name = x.Tool.Name,
                                           Type = new() { Name = x.Tool.Type.Name }
                                       },
                                       Project = new()
                                       {
                                           Name = x.Project.Name,
                                           Organization = new() { Name = x.Project.Organization.Name },
                                           Team = new() { Name = x.Project.Team.Name }
                                       }
                                   })
                                   .AsNoTracking()
                                   .ToListAsync();

            return new PaginateModel<FindingEntity>(1, 12, items);                   
        }

        public async Task<List<FindingEntity>> GetWithFilterAsync()
          => await base.Where(x => x.Id != 0)
                       .Include(x => x.Severity)
                       .Include(x => x.Status)
                       .Include(x => x.Tool)
                       .Include(x => x.Tool.Type)
                       .Include(x => x.Project.Team)
                       .Include(x => x.Project.Organization)
                       .Select(x => new FindingEntity()
                       {
                           Id = x.Id,
                           Title = x.Title,
                           Created = x.Created,
                           Severity = new() { Name = x.Severity.Name, Sla = x.Severity.Sla },
                           Status = new() { Name = x.Status.Name },
                           Tool = new()
                           {
                               Name = x.Tool.Name,
                               Type = new() { Name = x.Tool.Type.Name }
                           },
                           Project = new()
                           {
                               Name = x.Project.Name,
                               Organization = new() { Name = x.Project.Organization.Name },
                               Team = new() { Name = x.Project.Team.Name }
                           }
                       })
                       .AsNoTracking()
                       .ToListAsync();

    }
}