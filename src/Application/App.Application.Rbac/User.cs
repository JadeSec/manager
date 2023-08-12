using App.Domain.Entities;
using System.Threading.Tasks;
using App.Infra.Bootstrap.Attributes;
using App.Infra.Implementation.Filter;
using App.Repositories;

namespace App.Application.Rbac
{
    [Transient]
    public class User
    {
        private readonly UserRepository _userRep;

        public User(UserRepository userRepository)
        {
            _userRep = userRepository;
        }

        public async Task<Paginate<UserEntity>> GetAsync(string expression, int page)
        {
            var filter = new Filter(expression, page, configuration: new Configuration
            {
                MaxPerPage = 10,
                ValueLength = 50,
                ExpressionMax = 1,
                Criteria = new Criteria[]
                {
                    new("name", new string[] { "=", "%"}),
                    new("email", new string[] { "=", "%"}),
                    new("surname", new string[] { "=", "%"}),
                    new("username", new string[] { "=", "%"})
                }
            });

            return await _userRep.GetAsync(filter);
        }
    }
}
