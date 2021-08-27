using Xunit;

namespace BankSystem.Test
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
    }
}