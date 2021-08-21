using System.Collections.Generic;
using Bogus;
namespace BankSystem
{
    public class TestDataService
    {
        public static Client TestClientGenerate()
        {
            var testClients = new Faker<Client>()
                .RuleFor(c => c.Name, f => f.Name.FullName())
                .RuleFor(c => c.Age, f => f.Random.Number(18, 100))
                .RuleFor(c => c.PassportNumber,
                    f => f.Random.String2(2, "ABCDEFGHIJKLMNOPQRSTUVWXYZ") + "-" +
                         f.Random.String2(8, "0123456789"));
            return testClients.Generate();
        }
        public static Employee TestEmployeeGenerate()
        {
            var testEmployees = new Faker<Employee>()
                .RuleFor(e => e.Name, f => f.Name.FullName())
                .RuleFor(e => e.Age, f => f.Random.Number(18, 100))
                .RuleFor(e => e.PassportNumber,
                    f => f.Random.String2(2, "ABCDEFGHIJKLMNOPQRSTUVWXYZ") + "-" +
                         f.Random.String2(8, "0123456789"))
                .RuleFor(e => e.Position, f => f.Name.JobTitle());
            return testEmployees.Generate();
        }
        public static Account TestAccountGenerate()
        {
            Dollar dollar = new Dollar() { CurrencyName = "dollar", rate = 1 };
            Ruble ruble = new Ruble() { CurrencyName = "ruble", rate = 77 };
            Leu leu = new Leu() { CurrencyName = "leu", rate = 18 };
            Hryvnia hryvnia = new Hryvnia() { CurrencyName = "hryvnia", rate = 27 }; 
            var testAccounts = new Faker<Account>()
                .RuleFor(a => a.value, f => f.Random.Double(0.0, 1000000))
                .RuleFor(a => a.currency, f => f.PickRandom<Currency>(new List<Currency>(){dollar,leu,ruble,hryvnia}));
                
            return testAccounts.Generate();
        }
    }
}