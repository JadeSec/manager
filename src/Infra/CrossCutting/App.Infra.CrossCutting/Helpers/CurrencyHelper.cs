namespace App.Infra.CrossCutting.Helpers
{
    public static class CurrencyHelper
    {
        /// <summary>
        /// int cents = 15099;
        /// CentsToDollars(cents);
        /// 
        /// Output:
        ///   $150.99
        /// </summary>
        /// <param name="cents"></param>
        /// <returns></returns>
        public static decimal CentsToDollars(long cents)
            => (decimal)cents / 100;
    }
}
