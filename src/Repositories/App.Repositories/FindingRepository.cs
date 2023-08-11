using System;
using System.Linq;
using App.Domain.Entities;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using App.Infra.Integration.Database;
using App.Infra.Implementation.Filter;
using App.Infra.Integration.Database.Providers;
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

        public async Task<Paginate<FindingEntity>> GetAsync(Filter filter)
        {
            var entity = Context.Set<FindingEntity>();
            var query = entity.FilterLike("title", filter, (e, v) => e.Where(x => x.Title.ToLower().Contains(v)))
                              .FilterEqual<FindingEntity, long>("id", filter, (e, v) => e.Where(x => x.Id.Equals(v)))
                              .FilterEqual<FindingEntity, string>("title", filter, (e, v) => e.Where(x => x.Title.Equals(v)))
                              .FilterEqual<FindingEntity, string>("severity", filter, (e, v) => e.Where(x => x.Severity.Name.ToLower().Equals(v.ToLower())))
                              .FilterEqual<FindingEntity, string>("project", filter, (e, v) => e.Where(x => x.Project.Name.ToLower().Equals(v.ToLower())))
                              .FilterEqual<FindingEntity, string>("team", filter, (e, v) => e.Where(x => x.Project.Team.Name.ToLower().Equals(v.ToLower())))
                              .FilterEqual<FindingEntity, string>("org", filter, (e, v) => e.Where(x => x.Project.Organization.Name.ToLower().Equals(v.ToLower())))
                              .FilterEqual<FindingEntity, string>("status", filter, (e, v) => e.Where(x => x.Status.Name.ToLower().Equals(v.ToLower())))
                              .FilterEqual<FindingEntity, string>("created", filter, (e, v) => e.Where(x => x.Created.Date.Equals(DateTime.Parse(v).Date)))
                              .FilterEqual<FindingEntity, int>("sla", filter, (e, v) => e.Where(x => x.Severity.Sla.Equals(v)))
                              .FilterEqual<FindingEntity, string>("cwe", filter, (e, v) => e.Where(x => x.Cwe.ToLower().Equals(v.ToLower())))
                              .FilterEqual<FindingEntity, string>("cve", filter, (e, v) => e.Where(x => x.Cve.ToLower().Equals(v.ToLower())))
                              .FilterEqual<FindingEntity, string>("file_path", filter, (e, v) => e.Where(x => x.FilePath.ToLower().Equals(v.ToLower())))
                              .FilterEqual<FindingEntity, string>("sha1", filter, (e, v) => e.Where(x => x.Sha1.ToLower().Equals(v.ToLower())))
                              .FilterEqual<FindingEntity, string>("tool", filter, (e, v) => e.Where(x => x.Tool.Name.ToLower().Equals(v.ToLower())))
                              .FilterEqual<FindingEntity, string>("tool_type", filter, (e, v) => e.Where(x => x.Tool.Type.Name.ToLower().Equals(v.ToLower())))
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
                              });

            var total = await query.CountAsync();
            var items = await query.AsNoTracking()
                                   .Skip(filter.Page)
                                   .Take(filter.Configuration.MaxPerPage)
                                   .ToListAsync();

            return new Paginate<FindingEntity>(filter, items, total);
        }
    }
}