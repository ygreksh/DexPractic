using System;
using System.Collections.Generic;

namespace BankSystem
{
    internal class Program
    {
        public static void Main(string[] args)
        {
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
        }
    }
}