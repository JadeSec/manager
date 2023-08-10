using System.Linq;

namespace App.Infra.Implementation.Filter
{
    public class Configuration
    {
        public int MaxPerPage { get; set; }

        public int ValueLength { get; set; }

        public int ExpressionMax { get; set; }

        public Criteria[] Criteria { get; set; }  

        public bool IsValidValueLenght(string value)
          => value.Length <= ValueLength;

        public bool IsValidExpressionMax(int count)
          => count <= ExpressionMax;

        public bool IsValidCriteria(string attr)
          => Criteria.Any(x => x.Name.Equals(attr));        
    }
}