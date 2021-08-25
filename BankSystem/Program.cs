using System;
using System.Collections.Generic;
using BankSystem.Exceptions;
using Newtonsoft.Json;

namespace BankSystem
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            BankService bankService = new BankService();

            Client client1 = TestDataService.TestClientGenerate();
            Account account1 = TestDataService.TestAccountGenerate();
            
            ExtractData.GetData(client1,"client.txt");
            ExtractData.GetData(account1,"account1.txt");
        }
        
        
    }
}