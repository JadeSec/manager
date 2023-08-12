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
    public class UserRepository : RepositoryService<MySQLProvider, UserEntity>
    {
        readonly ILogger<UserRepository> _logger;

        public UserRepository(ILogger<UserRepository> logger)
        {
            _logger = logger;
        }

        public async Task<Paginate<UserEntity>> GetAsync(Filter filter)
        {
            var entity = Context.Set<UserEntity>();
            var query = entity.FilterEqual<UserEntity, string>("name", filter, (e, v) => e.Where(x => x.Name.Equals(v)))
                              .FilterEqual<UserEntity, string>("email", filter, (e, v) => e.Where(x => x.Email.Equals(v)))
                              .FilterEqual<UserEntity, string>("surname", filter, (e, v) => e.Where(x => x.Surname.Equals(v)))
                              .FilterEqual<UserEntity, string>("username", filter, (e, v) => e.Where(x => x.Username.Equals(v)))
                              .FilterLike("name", filter, (e, v) => e.Where(x => x.Name.ToLower().Contains(v)))                                                            
                              .FilterLike("email", filter, (e, v) => e.Where(x => x.Email.ToLower().Contains(v)))                                                            
                              .FilterLike("surname", filter, (e, v) => e.Where(x => x.Surname.ToLower().Contains(v)))                                                            
                              .FilterLike("username", filter, (e, v) => e.Where(x => x.Username.ToLower().Contains(v)))                                                            
                              .Select(x => new UserEntity()
                              {
                                  Id = x.Id,
                                  Name = x.Name,
                                  Email = x.Email,                                 
                                  Surname = x.Surname,
                                  Username = x.Username,
                                  Avatar = x.Avatar,
                                  Created = x.Created
                              });

            var total = await query.CountAsync();
            var items = await query.AsNoTracking()
                                   .Skip(filter.Page)
                                   .Take(filter.Configuration.MaxPerPage)
                                   .OrderByDescending(x => x.Created)
                                   .ToListAsync();

            return new Paginate<UserEntity>(filter, items, total);
        }
    }
}
