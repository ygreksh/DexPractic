using System;
using System.Collections.Generic;
using BankSystem.Exceptions;

namespace BankSystem
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            //Словарь клиентов
            Dictionary<Client, List<Account>> dictOfClients = new Dictionary<Client, List<Account>>();
            dictOfClients.Add(new Client("Bulochkin", 22, "b-22222222"), 
                                new List<Account>()     //содержимое списка счетов
                                {
                                    new Account(){currency = new Dollar(){CurrencyName = "Dollar",rate = 1}, value = 100},   //100 долларов
                                    new Account(){currency = new Ruble(){CurrencyName = "Ruble", rate = 77}, value = 1000},  //1000 рублей
                                    new Account(){currency = new Leu(){CurrencyName = "Leu", rate = 12}, value = 200}        //200 лей
                                });
            dictOfClients.Add(new Client("Abrikosov", 33,"a-11111111"), 
                new List<Account>()     //содержимое списка счетов
                {
                    new Account(){currency = new Dollar(){CurrencyName = "Dollar",rate = 1}, value = 50},   //50 долларов
                    new Account(){currency = new Ruble(){CurrencyName = "Ruble", rate = 77}, value = 500},  //500 рублей
                    new Account(){currency = new Leu(){CurrencyName = "Leu", rate = 12}, value = 100}       //000 лей
                });

            
            //тестовые счета для проверки перевода денег
            Account account1 = new Account() 
                { currency = new Dollar() { CurrencyName = "Dollar", rate = 1 }, value = 20 };   //20 долларов
            Account account2 = new Account() 
                { currency = new Ruble() { CurrencyName = "Ruble", rate = 77 }, value = 20 };    //50 рублей
            Account account3 = new Account()
                { currency = new Hryvnia() { CurrencyName = "Hryvnia", rate = 27 }, value = 27 };    //27 гривен
            
            //Словарь клентов до добавления нового счета Hryvnia
            Console.WriteLine("Клиенты до добавления нового счета:");
            foreach (var item in dictOfClients)
            {
                Console.WriteLine($"{item.Key}");
                foreach (var account in item.Value)
                {
                    Console.WriteLine($"    - {account.currency} - {account.value}");
                }
            }

            Client client1 = new Client("Abrikosov", 22, "a-11111111");    //Такой клиент уже есть
            Client client2 = new Client("Yablokov", 33, "y-12121212");     //Новый клиент
            //Client client3 = new Client("Kartoshkin", 13,"k-13131313");     //Новый клиент вызовет исключение WrongAgeException
            
            BankService.AddClientAccount(account3, client1, dictOfClients);  //Добавили счет с гривнами для Абрикосова
            BankService.AddClientAccount(account3, client2, dictOfClients);  //Добавили Яблокова с единственным счетом в гривнах
            
            //Словарь клентов после добавления нового счета Hryvnia
            Console.WriteLine("\nКлиенты после добавления нового счета:");
            foreach (var item in dictOfClients)
            {
                Console.WriteLine($"{item.Key}");
                foreach (var account in item.Value)
                {
                    Console.WriteLine($"    - {account.currency} - {account.value}");
                }
            }

        }
    }
}