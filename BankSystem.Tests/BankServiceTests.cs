using System;
using Xunit;

namespace BankSystem.Tests
{
    public class BankServiceTests
    {
        [Fact]
        public void AddClient_FindClient_Ivanov()
        {
            //Arrange
            Client client1 = new Client() { Name = "Ivanov", Age = 33, PassportNumber = "IV-11111111" };
            BankService bankService = new BankService();
            
            //Act
            bankService.AddClient("Ivanov", 33, "IV-11111111");
            Client client2 = BankService.FindPerson<Client>(client1.PassportNumber, bankService.listOfClients);
            //Assert
            Assert.NotNull(client2);
            Assert.Equal(client1,client2);
        }

        [Fact]
        public void TransferMoney_Dollar5_to_Dollar_10_Eq_Dollar15()
        {
            //Arrange
            Account donorAccount = new Account() { currency = new Dollar() { currencyName = "USD", rate = 1}, value = 5 };
            Account recipientAccount = new Account() { currency = new Dollar() { currencyName = "USD", rate = 1}, value = 10 };
            Func<double, Currency, Currency, double> transfermoney = new  Exchange<Currency>().CurrencyExchange;
            BankService bankService = new BankService();
            //Act
            bankService.TransferMoney(5,donorAccount,recipientAccount, transfermoney);
            //Assert
            Assert.Equal(donorAccount.value,0);
            Assert.Equal(recipientAccount.value,15);
        }
        [Fact]
        public void TransferMoney_Dollar1_to_Ruble_10_Eq_Ruble87()
        {
            //Arrange
            Account donorAccount = new Account() { currency = new Dollar() { currencyName = "USD", rate = 1}, value = 10 };
            Account recipientAccount = new Account() { currency = new Ruble() { currencyName = "RUB", rate = 77}, value = 10 };
            Func<double, Currency, Currency, double> transfermoney = new  Exchange<Currency>().CurrencyExchange;
            BankService bankService = new BankService();
            //Act
            bankService.TransferMoney(1,donorAccount,recipientAccount, transfermoney);
            //Assert
            Assert.Equal(donorAccount.value,9);
            Assert.Equal(recipientAccount.value,87);
        }
    }
}