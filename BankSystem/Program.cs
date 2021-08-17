using System;
using System.Collections.Generic;
using BankSystem.Exceptions;
using  Newtonsoft.Json;

namespace BankSystem
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            BankService bankService = new BankService();

            Client Ivanov = new Client() { Name = "Иванов", Age = 31, PassportNumber = "iv-11111111" };
            Client Petrov = new Client(){Name = "Петров", Age = 32, PassportNumber = "pe-22222222"};
            Client Sidorov = new Client(){Name = "Сидоров", Age = 33, PassportNumber = "si-33333333"};
            bankService.listOfClients.Add(Ivanov);
            bankService.listOfClients.Add(Petrov);
            bankService.listOfClients.Add(Sidorov);
            
            Dollar dollar = new Dollar() { CurrencyName = "dollar", rate = 1 };
            Ruble ruble = new Ruble() { CurrencyName = "ruble", rate = 77 };
            Leu leu = new Leu() { CurrencyName = "leu", rate = 18 };
            Hryvnia hryvnia = new Hryvnia() { CurrencyName = "hryvnia", rate = 27 };

            Account ivanovaccount1 = new Account() { currency = dollar, value = 10 };
            Account ivanovaccount2 = new Account() { currency = ruble, value = 1000 };
            Account petrovaccount1 = new Account() { currency = ruble, value = 2000 };
            Account petrovaccount2 = new Account() { currency = leu, value = 2000 };
            Account petrovaccount3 = new Account() { currency = hryvnia, value = 20000 };
            Account sidorovaccount1 = new Account() { currency = ruble, value = 3000 }; 
            
            bankService.AddClientAccount(ivanovaccount1, Ivanov);
            bankService.AddClientAccount(ivanovaccount2, Ivanov);
            bankService.AddClientAccount(petrovaccount1, Petrov);
            bankService.AddClientAccount(petrovaccount2, Petrov);
            bankService.AddClientAccount(petrovaccount3, Petrov);
            bankService.AddClientAccount(sidorovaccount1, Sidorov);
            
            //bankService.WriteClientsToFile();
            //bankService.WriteAccountsToFile();
            
            var listOfClientsFromFile = BankService.ReadClientsFromFile();
            var dictOfAccountsFromFile = BankService.ReadAccountsFromFile();
            
            foreach (var client in listOfClientsFromFile)
            {
                Console.WriteLine($"{client.PassportNumber} {client.Name} {client.Age}");
            }

            foreach (var item in dictOfAccountsFromFile)
            {
                Console.WriteLine($"{item.Key} :");
                foreach (var account in item.Value)
                {
                    Console.WriteLine($"            - {account.currency} {account.value}");
                }
            }

        }
        
        
    }
}