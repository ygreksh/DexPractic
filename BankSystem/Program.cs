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
            bankService.listOfClients.Add(new Client(){Name = "Иванов", Age = 31, PassportNumber = "iv-11111111"});
            bankService.listOfClients.Add(new Client(){Name = "Петров", Age = 32, PassportNumber = "pe-22222222"});
            bankService.listOfClients.Add(new Client(){Name = "Сидоров", Age = 33, PassportNumber = "si-33333333"});
            
            //bankService.WriteClientsToFile();
            var listOfClientsFromFile = new List<Client>();
            listOfClientsFromFile = BankService.ReadClientsFromFile();
            foreach (var client in listOfClientsFromFile)
            {
                Console.WriteLine($"{client.PassportNumber} {client.Name} {client.Age}");
            }

        }
        
        
    }
}