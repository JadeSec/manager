using App.Domain.Entities;
using App.Infra.Integration.Database;
using Microsoft.Extensions.Logging;
using App.Infra.Integration.Database.Providers;

namespace App.Repositories
{
    public class PocRepository : RepositoryService<MySQLProvider, object>
    {
        readonly ILogger<PocRepository> _logger;

        public PocRepository(ILogger<PocRepository> logger)
        {
            _logger = logger;
        }

        //public async Task<object> GetAsync(string email)
        //  => await base.Where(x => x.Email.Equals(email))
        //               .AsNoTracking()
        //               .FirstOrDefaultAsync();
    }
} 