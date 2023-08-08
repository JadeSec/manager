namespace App.Infra.Implementation.Filter
{
    public class Configuration
    {
        public int ValueLengthAllowed { get; set; }

        public int ExpressionMaxAllowed { get; set; }

        public bool isValidValueLenght(string value)
          => value.Length <= ValueLengthAllowed;

        public bool isValidExpressionMax(int count)
          => count <= ExpressionMaxAllowed;
    }
}