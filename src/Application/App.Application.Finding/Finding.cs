using App.Domain.Entities;
using App.Infra.Bootstrap.Attributes;
using App.Infra.Implementation.Filter;
using App.Repositories;
using System.Threading.Tasks;

namespace App.Application.Finding
{
    [Transient]
    public class Finding
    {
        private readonly FindingRepository _findingRep;

        public Finding(FindingRepository findingRepository)
        {
            _findingRep = findingRepository;
        }

        public async Task<Paginate<FindingEntity>> GetAsync(string expression, int page)
        {
            var filter = new Filter(expression, configuration: new Configuration
            {
                ExpressionMax = 5,
                ValueLength = 255,
                Criteria = new Criteria[]
                {
                    new("id",new string[] { "=" }),
                    new("title", new string[] { "=", "%"}),
                    new("severity",new string[] { "=" }),
                    new("project",new string[] { "=" }),
                    new("team",new string[] { "=" }),
                    new("org",new string[] { "=" }),
                    new("created",new string[] { "=" }),
                    new("sla",new string[] { "=" }),
                    new("cwe",new string[] { "=" }),
                    new("cve",new string[] { "=" }),
                    new("file_path",new string[] { "=" }),
                    new("sha1",new string[] { "=" }),
                    new("tool",new string[] { "=" }),
                    new("tool_type",new string[] { "=" })
                }
            });

            return await _findingRep.GetAsync(filter, page);
        }
    }
}
