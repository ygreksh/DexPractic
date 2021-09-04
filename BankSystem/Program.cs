using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BankSystem.Exceptions;
using Newtonsoft.Json;

namespace BankSystem
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            var clientlocker = new object();
            BankService bankService = new BankService();
            Client client;
            ThreadPool.QueueUserWorkItem(_ =>
            {
                for (int i = 0; i < 10; i++)
                {
                    client = TestDataService.TestClientGenerate();
                    bankService.AddClient(client.Name,client.Age,client.PassportNumber);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Добавлен новый клиент: {client.PassportNumber}, {client.Name}, {client.Age}");
                    Console.ResetColor();
                    Thread.Sleep(1000);
                }
            });
            ThreadPool.QueueUserWorkItem(_ =>
            {
                while (true)
                {
                    lock (clientlocker)
                    {
                        bankService.PrintClients();
                    }
                    Thread.Sleep(1000);
                }
                
            });
            Console.ReadLine();
        }
    }
}