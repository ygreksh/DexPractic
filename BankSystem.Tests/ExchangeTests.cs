using Xunit;

namespace BankSystem.Tests
{
    public class ExchangeTests
    {
        [Fact]
        public void CurrencyExchange_1d_to_77r()
        {
            //Arrange
            Exchange<Currency> exchange = new Exchange<Currency>();
            //Act
            Dollar dollar = new Dollar() { rate = 1 };
            Ruble ruble = new Ruble() { rate = 77 };
            double result = exchange.CurrencyExchange(1, dollar, ruble);
            //Assert
            Assert.Equal(77,result);
        }
    }
}