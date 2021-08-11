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
            Console.WriteLine("Было");
            Console.WriteLine($"{account1.value} {account1.currency}");
            Console.WriteLine($"{account2.value} {account2.currency}");
            
            //тест перевода денег между счетами
            BankService bankService = new BankService();
            //Устанавливаем через делегат способ обмена валюты
            BankService.Transfer transferMoney = new  Exchange<Currency>().CurrencyExchange;    
            bankService.RegisterTransfer(transferMoney);
            //собственно сам перевод денег со счета на счет  без комиссии
            bankService.TransferMoney(10, account1, account2, transferMoney);
            Console.WriteLine("Стало");
            Console.WriteLine($"{account1.value} {account1.currency}");
            Console.WriteLine($"{account2.value} {account2.currency}");
            //перевод денег со счета на счет с комиссией
            bankService.TransferMoneyWithTax(5, account1, account2, transferMoney);
            Console.WriteLine("Стало");
            Console.WriteLine($"{account1.value} {account1.currency}");
            Console.WriteLine($"{account2.value} {account2.currency}");
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

            try
            {
                Client client1 = new Client("Abrikosov", 22, "a-11111111");    //Такой клиент уже есть
                Client client2 = new Client("Yablokov", 33, "y-12121212");     //Новый клиент
                Client client3 = new Client("Kartoshkin", 13,"k-13131313");     //Новый клиент вызовет исключение WrongAgeException
            
                BankService.AddClientAccount(account3, client1, dictOfClients);  //Добавили счет с гривнами для Абрикосова
                BankService.AddClientAccount(account3, client2, dictOfClients);  //Добавили Яблокова с единственным счетом в гривнах
                BankService.AddClientAccount(account3, client3, dictOfClients);  //Добавили Картошкина который дожен вызвать исключение

            }
            catch (WrongAgeException e)
            {
                Console.WriteLine($"Ошибка: {e.Message}");
            }
            
            //Словарь клентов после добавления нового счета Hryvnia
            Console.WriteLine("Клиенты после добавления нового счета:");
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