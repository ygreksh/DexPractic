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
            BankSystem banksystem = new BankSystem();
            //переводит 10 долларов на рублевый счет без комиссии
            banksystem.TransferMoney(10,account1,account2, MyTransferMoney);
            Console.WriteLine("Стало без комисии -10 долларов +770 рублей");
            Console.WriteLine($"{account1.value} {account1.currency}");
            Console.WriteLine($"{account2.value} {account2.currency}");
            //переводит 5 долларов на рублевый счет с комиссиеё
            banksystem.TransferMoney(5,account1,account2, MyTransferMoneyWithTax);
            Console.WriteLine("Стало -5 долларов +385 и -1 доллар комиссии");
            Console.WriteLine($"{account1.value} {account1.currency}");
            Console.WriteLine($"{account2.value} {account2.currency}");
            //переводит 77 рублей счет на долларовый счет с комиссиеё
            banksystem.TransferMoney(77,account2,account1, MyTransferMoneyWithTax);

            Console.WriteLine("Стало в итоге -77 рублей +1 доллар и -77 рублей комиссии");
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
            
                BankSystem.AddClientAccount(account3, client1, dictOfClients);  //Добавили счет с гривнами для Абрикосова
                BankSystem.AddClientAccount(account3, client2, dictOfClients);  //Добавили Яблокова с единственным счетом в гривнах
                BankSystem.AddClientAccount(account3, client3, dictOfClients);  //Добавили Картошкина который дожен вызвать исключение

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
        
        //Мой метод перевода денег между счетами без комиссии
        public static void MyTransferMoney(double Sum, Account donorAccount, Account recipientAccount)
        {
            try
            {
                if (donorAccount.value - Sum < 0)
                {
                    throw new NotEnoughMoneyException("Недостаточно денег на счету!");
                }

                donorAccount.value -= Sum;
                recipientAccount.value +=
                    new Exchange<Currency>().CurrencyExchange(Sum, donorAccount.currency, recipientAccount.currency);
            }
            catch (NotEnoughMoneyException e)
            {
                Console.WriteLine($"Ошибка: {e.Message}");
            }
        }
        //Мой метод перевода денег между счетами с комиссией
        public static void MyTransferMoneyWithTax(double Sum, Account donorAccount, Account recipientAccount)
        {
            try
            {
                double tax = 1;
                donorAccount.value -= Sum;
                recipientAccount.value += new Exchange<Currency>().CurrencyExchange(Sum, donorAccount.currency, recipientAccount.currency);
                //Комиссия tax = 1 доллар снимается со счета донора
                donorAccount.value -= new Exchange<Currency>().CurrencyExchange(tax, new Dollar(){CurrencyName = "Dollar", rate = 1}, donorAccount.currency);
                if (donorAccount.value < 0)
                {
                    throw new NotEnoughMoneyException("Недостаточно денег на счету");
                }
            }
            catch (NotEnoughMoneyException e)
            {
                Console.WriteLine($"Ошибка: {e.Message}");
                throw;
            }
        }
    }
}