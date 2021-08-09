using System;
using System.Collections.Generic;

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
            Dictionary<Client, List<Account>> dictOfClients = new Dictionary<Client, List<Account>>();
            dictOfClients.Add(new Client(){Name = "Bulochkin", PassportNumber = "a-11111111"}, 
                                new List<Account>()     //содержимое списка счетов
                                {
                                    new Account(){currency = new Dollar(){CurrencyName = "Dollar",rate = 1}, value = 100},   //100 долларов
                                    new Account(){currency = new Ruble(){CurrencyName = "Ruble", rate = 77}, value = 1000},  //1000 рублей
                                    new Account(){currency = new Leu(){CurrencyName = "Leu", rate = 12}, value = 200}        //200 лей
                                });
            dictOfClients.Add(new Client(){Name = "Abrikosov", PassportNumber = "b-22222222"}, 
                new List<Account>()     //содержимое списка счетов
                {
                    new Account(){currency = new Dollar(){CurrencyName = "Dollar",rate = 1}, value = 50},   //50 долларов
                    new Account(){currency = new Ruble(){CurrencyName = "Ruble", rate = 77}, value = 500},  //500 рублей
                    new Account(){currency = new Leu(){CurrencyName = "Leu", rate = 12}, value = 100}       //000 лей
                });

            //тестовые счета
            Account account1 = new Account() 
                { currency = new Dollar() { CurrencyName = "Dollar", rate = 1 }, value = 20 };   //20 долларов
            Account account2 = new Account() 
                { currency = new Ruble() { CurrencyName = "Ruble", rate = 77 }, value = 20 };    //50 рублей
            Console.WriteLine("Было");
            Console.WriteLine($"{account1.value} {account1.currency}");
            Console.WriteLine($"{account2.value} {account2.currency}");
            
            //тест перевода денег между счетами
            BankSystem banksystem = new BankSystem();
            //переводит 20 долларов на рублевый счет
            banksystem.TransferMoney(20,account1,account2, MyTransferMoney);

            Console.WriteLine("Стало");
            Console.WriteLine($"{account1.value} {account1.currency}");
            Console.WriteLine($"{account2.value} {account2.currency}");

        }

        public static void MyTransferMoney(double Sum, Account fromAccount, Account toAccount)
        {
            fromAccount.value -= Sum;
            toAccount.value += new Exchange<Currency>().CurrencyExchange(Sum, fromAccount.currency, toAccount.currency);
        }
    }
}