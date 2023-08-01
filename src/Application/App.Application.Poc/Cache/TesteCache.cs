using App.Infra.Integration.Redis.Attributes;
using App.Infra.Integration.Redis.Interfaces;

namespace App.Application.Poc.Cache
{
    [Redis(Name = "teste_poc", Database = 10, Expiry = 20)]
    public class TesteCache : ICache
    {        
        public int ProductId { get; set; }

        public TesteCache(int productId)
        {
            ProductId = productId;
        }
    }
}