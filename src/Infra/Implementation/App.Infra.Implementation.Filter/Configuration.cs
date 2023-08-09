using System.Linq;

namespace App.Infra.Implementation.Filter
{
    public class Configuration
    {
        public int ValueLength { get; set; }

        public int ExpressionMax { get; set; }

        public Criteria[] Criteria { get; set; }  

        public bool isValidValueLenght(string value)
          => value.Length <= ValueLength;

        public bool isValidExpressionMax(int count)
          => count <= ExpressionMax;

        public bool isValidCriteria(string attr)
          => Criteria.Any(x => x.Name.Equals(attr));
    }
}