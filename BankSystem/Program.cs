using System;
using System.Collections.Generic;
using BankSystem.Exceptions;

namespace BankSystem
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            BankService bankService = new BankService();

            
            //Словарь клиентов
            bankService.dictOfClients.Add(new Client(){Name = "Bulochkin", Age = 22, PassportNumber = "b-22222222"}, 
                                new List<Account>()     //содержимое списка счетов
                                {
                                    new Account(){currency = new Dollar(){CurrencyName = "Dollar",rate = 1}, value = 100},   //100 долларов
                                    new Account(){currency = new Ruble(){CurrencyName = "Ruble", rate = 77}, value = 1000},  //1000 рублей
                                    new Account(){currency = new Leu(){CurrencyName = "Leu", rate = 12}, value = 200}        //200 лей
                                });
            bankService.dictOfClients.Add(new Client(){Name = "Abrikosov",Age = 33,PassportNumber = "a-11111111"}, 
                new List<Account>()     //содержимое списка счетов
                {
                    new Account(){currency = new Dollar(){CurrencyName = "Dollar",rate = 1}, value = 50},   //50 долларов
                    new Account(){currency = new Ruble(){CurrencyName = "Ruble", rate = 77}, value = 500},  //500 рублей
                    new Account(){currency = new Leu(){CurrencyName = "Leu", rate = 12}, value = 100}       //000 лей
                });
            //Добавление клиентов методом AddClient
            bankService.AddClient("Bulochkin",22,"b-22222222");     //такой клиент уже есть
            bankService.AddClient("Gorohov",99,"g-99999999");       //новый клиент     
            bankService.AddClient("Vinogradov",44,"v-33333333");    //новый клиент
            
            //тестовые счета
            Account account1 = new Account() 
                { currency = new Dollar() { CurrencyName = "Dollar", rate = 1 }, value = 20 };   //20 долларов
            Account account2 = new Account() 
                { currency = new Ruble() { CurrencyName = "Ruble", rate = 77 }, value = 20 };    //50 рублей
            Account account3 = new Account()
                { currency = new Hryvnia() { CurrencyName = "Hryvnia", rate = 27 }, value = 27 };    //27 гривен
            
            //отдельно клиенты
            Client client1 = new Client(){Name = "Abrikosov",Age = 33,PassportNumber = "a-11111111"};    //Такой клиент уже есть
            Client client2 = new Client(){Name = "Yablokov",Age = 24,PassportNumber = "y-12121212"};     //Новый клиент
            Client client3 = new Client(){Name = "Kartoshkin",Age = 66,PassportNumber = "k-13131313"};     //Новый клиент вызовет исключение WrongAgeException
            //добавление счетов клиентам
            bankService.AddClientAccount(account1, client1);  
            bankService.AddClientAccount(account2, client1);  
            bankService.AddClientAccount(account3, client1); 
            bankService.AddClientAccount(account1, client2);  
            bankService.AddClientAccount(account3, client3);  
            
            //bankService.WriteClientsToFile();     //запись в файл
            bankService.ReadClientsFromFile();      //чтение из файла
            
            Console.WriteLine("Вывод из словаря:");
            foreach (var item in bankService.dictOfClients)
            {
                Console.WriteLine($"{item.Key.PassportNumber} {item.Key.Name} {item.Key.Age}");
                foreach (var account in item.Value)
                {
                    Console.WriteLine($"    - {account.currency} - {account.value}");
                }
            }
            //проверка, вывод содержимого dictofClientsFromFile
            Console.WriteLine("-------------------------------------------------");
            Console.WriteLine("Вывод из файла:");
            foreach (var item in bankService.dictOfClientsfromFile)
            {
                Console.WriteLine($"{item.Key.PassportNumber} {item.Key.Name} {item.Key.Age}");
                foreach (var account in item.Value)
                {
                    Console.WriteLine($"    - {account.currency} - {account.value}");
                }
            }

        }
        
        
    }
}