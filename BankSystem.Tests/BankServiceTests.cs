using System;
using System.Threading;
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
        public void TransferMoney_Dollar5_To_Dollar10_Eq_Dollar15()
        {
            //Arrange
            Account donorAccount = new Account() { currency = new Dollar() { currencyName = "USD", rate = 1}, value = 5 };
            Account recipientAccount = new Account() { currency = new Dollar() { currencyName = "USD", rate = 1}, value = 10 };
            Func<double, Currency, Currency, double> transfermoney = new  Exchange<Currency>().CurrencyExchange;
            BankService bankService = new BankService();
            //Act
            bankService.TransferMoney(5,donorAccount,recipientAccount, transfermoney);
            //Assert
            Assert.Equal(0,donorAccount.value);
            Assert.Equal(15,recipientAccount.value);
        }
        [Fact]
        public void TransferMoney_Dollar1_To_Ruble10_Eq_Ruble87()
        {
            //Arrange
            Account donorAccount = new Account() { currency = new Dollar() { currencyName = "USD", rate = 1}, value = 10 };
            Account recipientAccount = new Account() { currency = new Ruble() { currencyName = "RUB", rate = 77}, value = 10 };
            Func<double, Currency, Currency, double> transfermoney = new  Exchange<Currency>().CurrencyExchange;
            BankService bankService = new BankService();
            //Act
            bankService.TransferMoney(1,donorAccount,recipientAccount, transfermoney);
            //Assert
            Assert.Equal(9,donorAccount.value);
            Assert.Equal(87,recipientAccount.value);
        }

        [Fact]
        public void TransferMoneyThreads_Dollar2_and_Dollar3_To_Dollar10_Eq_Dollar15()
        {
            //Arrange
            Account donorAccount1 = new Account() { currency = new Dollar() { currencyName = "USD", rate = 1}, value = 10 };
            Account donorAccount2 = new Account() { currency = new Dollar() { currencyName = "USD", rate = 1}, value = 10 };
            Account recipientAccount = new Account() { currency = new Ruble() { currencyName = "RUB", rate = 77}, value = 10 };
            Func<double, Currency, Currency, double> transfermoney = new  Exchange<Currency>().CurrencyExchange;
            BankService bankService = new BankService();
            //Act
            var locker = new object();
            ThreadPool.QueueUserWorkItem(_ =>
            {
                lock (locker)
                {
                    bankService.TransferMoney(2,donorAccount1,recipientAccount, transfermoney);
                }
            });
            ThreadPool.QueueUserWorkItem(_ =>
            {
                lock (locker)
                {
                    bankService.TransferMoney(3,donorAccount2,recipientAccount, transfermoney);
                }
            });
            //Thread.Sleep(100);
            Thread.CurrentThread.Join(100);
            //Assert
            Assert.Equal(8, donorAccount1.value);
            Assert.Equal(7, donorAccount2.value);
            Assert.Equal(395,recipientAccount.value);
        }
    }
}