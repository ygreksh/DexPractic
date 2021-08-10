using System;
using System.Collections.Generic;
using BankSystem.Exceptions;

namespace BankSystem
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            /*
            //список клиентов
                List<Client> listOfClients = new List<Client>();
                listOfClients.Add(new Client(){Name = "Abrikosov", PassportNumber = "a-11111111"});
                listOfClients.Add(new Client(){Name = "Bulochkin", PassportNumber = "b-22222222"});
                listOfClients.Add(new Client(){Name = "Vinogradov", PassportNumber = "v-33333333"});
                listOfClients.Add(new Client(){Name = "Gorohov", PassportNumber = "g-44444444"});
                listOfClients.Add(new Client(){Name = "Durmanov", PassportNumber = "d-55555555"});

                //список сотрудников
                List<Employee> listOfEmployee = new List<Employee>();
                listOfEmployee.Add(new Employee(){Name = "Acaciev", PassportNumber = "AC-66666666"});
                listOfEmployee.Add(new Employee(){Name = "Bukov", PassportNumber = "BU-77777777"});
                listOfEmployee.Add(new Employee(){Name = "Vyazov", PassportNumber = "VY-88888888"});
                listOfEmployee.Add(new Employee(){Name = "Gingkov", PassportNumber = "GI-99999999"});
                listOfEmployee.Add(new Employee(){Name = "Dubov", PassportNumber = "DU-00000000"});
            
                //поиск клиента из списка клиентов
                Person foundClient = BankSystem.FindPersonByPassportNumber("g-44444444", listOfClients);
                Console.WriteLine(foundClient.ToString());
            
                //поиск сотрудника из списка сотрудников тем же методом
                Person foundEmployee = BankSystem.FindPersonByPassportNumber("BU-77777777", listOfEmployee);
                Console.WriteLine(foundEmployee.ToString());

                //Проверка обмена валюты  из класса Exchange
                var dollar = new Dollar() {CurrencyName = "Dollar", rate = 1.00};
                var ruble = new Ruble() { CurrencyName = "Ruble", rate = 77.00 };
                double someMoneyDollars = 10.00;
                double someMoneyRuble = new Exchange<Currency>().CurrencyExchange(someMoneyDollars, dollar, ruble);
                Console.WriteLine(@"в долларах {0}, в рублях {1}", someMoneyDollars, someMoneyRuble);
                */
            //Словарь клиентов
            Dictionary<Client, List<Account>> dictOfClients = new Dictionary<Client, List<Account>>();
            dictOfClients.Add(new Client(){Name = "Bulochkin", PassportNumber = "b-22222222"}, 
                                new List<Account>()     //содержимое списка счетов
                                {
                                    new Account(){currency = new Dollar(){CurrencyName = "Dollar",rate = 1}, value = 100},   //100 долларов
                                    new Account(){currency = new Ruble(){CurrencyName = "Ruble", rate = 77}, value = 1000},  //1000 рублей
                                    new Account(){currency = new Leu(){CurrencyName = "Leu", rate = 12}, value = 200}        //200 лей
                                });
            dictOfClients.Add(new Client(){Name = "Abrikosov", PassportNumber = "a-11111111"}, 
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
                Client client1 = new Client() { Name = "Abrikosov", PassportNumber = "a-11111111" };    //Такой клиент уже есть
                Client client2 = new Client() { Name = "Yablokov", PassportNumber = "y-12121212" };     //Новый клиент
            
                BankSystem.AddClientAccount(account3, client1, dictOfClients);  //Добавили счет с гривнами для Абрикосова
                BankSystem.AddClientAccount(account3, client2, dictOfClients);  //Добавили Яблокова с единственным счетом в гривнах

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