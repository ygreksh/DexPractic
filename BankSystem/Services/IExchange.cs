namespace BankSystem
{
    public interface IExchange<T> where T: Currency
    {
        double CurrencyExchange(double sumfrom, T currencyFrom, T currencyTo);
    }
}