namespace BankSystem
{
    public class Exchange<T> : IExchange<T> where T : Currency
    {

        public double CurrencyExchange(double sumFrom, T currencyFrom, T currencyTo)
        {
            double sumTo = sumFrom / currencyFrom.rate * currencyTo.rate;
            return sumTo;
        }
    }
}