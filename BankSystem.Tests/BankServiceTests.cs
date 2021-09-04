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
            Account donorAccount = new Account() { currency = new Dollar() { currencyName = "USD" }, value = 5 };
            Account recipientAccount = new Account() { currency = new Dollar() { currencyName = "USD" }, value = 10 };
            //Func<double, Currency, Currency, double> transfermoney = (sum, fromAccount, toAccount) => {fromAccount.};
            BankService bankService = new BankService();
            //Act
            //bankService.TransferMoney(5,donorAccount,recipientAccount,Func<double, Currency, Currency, double> transfermoney);
            //Assert
        }
    }
}