namespace BankSystem
{
    public interface IExchange<T> where T: Currency
    {
        T CurrencyExchange(T currencyFrom, T currencyTo);
    }
}