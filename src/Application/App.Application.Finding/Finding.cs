using App.Domain.Entities;
using App.Domain.Models;
using App.Infra.Bootstrap.Attributes;
using App.Infra.Implementation.Filter;
using App.Repositories;
using System.Collections.Generic;
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

        public async Task<PaginateModel<FindingEntity>> GetAsync(string expression)
        {
            var filter = new Filter(expression, configuration: new Configuration
            {
                ExpressionMaxAllowed = 5,
                ValueLengthAllowed = 255
            });


            return await _findingRep.GetWithFilterAsync(filter);

            //return await _findingRep.GetWithFilterAsync();
        }
    }
}
