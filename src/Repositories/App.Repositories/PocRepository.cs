using App.Domain.Entities;
using App.Infra.Integration.Database;
using Microsoft.Extensions.Logging;
using App.Infra.Integration.Database.Providers;

namespace App.Repositories
{
    public class CustomerRepository : RepositoryService<MySQLProvider, object>
    {
        readonly ILogger<CustomerRepository> _logger;

        public CustomerRepository(ILogger<CustomerRepository> logger)
        {
            _logger = logger;
        }

        //public async Task<object> GetAsync(string email)
        //  => await base.Where(x => x.Email.Equals(email))
        //               .AsNoTracking()
        //               .FirstOrDefaultAsync();
    }
} 