using System;
using System.Collections.Generic;
using System.IO;
using BankSystem.Exceptions;
using Newtonsoft.Json;

namespace BankSystem
{
    public class BankService
    {
        public List<Client> listOfClients = new List<Client>();         //клиенты
        public List<Employee> ListOfEmployees = new List<Employee>();   //сотрудники
        public Dictionary<string, List<Account>> dictOfAccounts = new Dictionary<string, List<Account>>();  //словарь пасспорт - список счетов 
        

        public static string MainPath = Path.Combine("TestFiles");
        public DirectoryInfo MainDirectoryInfo = new DirectoryInfo(MainPath);
        public static string ClientsfileName = "clients.txt";       //хранит список клиентов
        public static string EmployeesfileName = "employees.txt";       //хранит список сотрудников
        public static string AccountsFileName = "accounts.txt";     //хранит словарь счетов у клиентов
        public Func<double, Currency, Currency, double> _transfer;

        public void RegisterTransfer(Func<double, Currency, Currency, double> transfer)
        {
            _transfer = transfer;
        }
        

        //обобщенный метод. работает только с экземплярами и наследниками Person
        public static T FindPerson<T>(string PassportNumber, List<T> listOfPersons) where T: Person
        {
            Person person = new Person(){PassportNumber = PassportNumber};
            return listOfPersons.Find(x => x.Equals(person));
        }
        
        //Добавление нового клиента в словарь
        public void AddClient(string name, int age, string passportnumber)
        {

            try
            {
                Client client = new Client() { Name = name, Age = age, PassportNumber = passportnumber };
                if (age < 18)
                {
                    throw new WrongAgeException("Недопустимый возраст клиента: возраст меньше 18!");
                }
                else if (!dictOfAccounts.ContainsKey(passportnumber))
                {
                    List<Account> listOfAccounts = new List<Account>();
                    listOfClients.Add(client);  //добавление в словарь
                    dictOfAccounts.Add(passportnumber,listOfAccounts);
                }
            }
            catch (WrongAgeException e)
            {
                Console.WriteLine(e.Message);
            }
        }
        
        
        //Добавление нового счета Account пользователю в словаре и файле
        public void AddClientAccount(Account account, Client client)
        {
            //если такого клиента нет в словаре - создаем нового клиента
            if (dictOfAccounts.ContainsKey(client.PassportNumber) == false)
            {
                List<Account> listOfAccounts = new List<Account>();
                listOfAccounts.Add(account);
                dictOfAccounts.Add(client.PassportNumber, listOfAccounts);  //добавление в словарь
            }
            //если искомый уже клиент есть, добавляется ещё один Account в listOfAccounts
            else
            {
                dictOfAccounts[client.PassportNumber].Add(account);
            }
        }
        //Переод денег между счетами без комиссии
        public void TransferMoney(double Sum, Account donorAccount, Account recipientAccount, 
                                    Func<double, Currency, Currency, double> transfermoney)
        {
            try
            {
                if (Sum > donorAccount.value)
                {
                    throw new NotEnoughMoneyException("Недостаточно денег на счету!");
                }
                else
                {
                    donorAccount.value -= Sum;
                    recipientAccount.value += transfermoney.Invoke(Sum, donorAccount.currency, recipientAccount.currency);
                }
            }
            catch (NotEnoughMoneyException e)
            {
                Console.WriteLine(e.Message);
            }
        }
        //Переод денег между счетами с комиссией
        public void TransferMoneyWithTax(double Sum, Account donorAccount, Account recipientAccount, 
                                            Func<double, Currency, Currency, double> transfermoney)
        {
            double tax = 1; //размер комиссии
            Dollar dollar = new Dollar() { CurrencyName = "Dollar", rate = 1 }; //валюта комиссии 
            try
            {
                if ((Sum + transfermoney.Invoke(tax, dollar, donorAccount.currency))  > donorAccount.value )
                {
                    throw new NotEnoughMoneyException("Недостаточно денег на счету!");
                }
                else
                {
                    donorAccount.value -= Sum;
                    recipientAccount.value += transfermoney.Invoke(Sum, donorAccount.currency, recipientAccount.currency);
                    //снимается комиссия со счета-донора
                    donorAccount.value -= transfermoney.Invoke(tax, dollar, donorAccount.currency);
                }
                
            }
            catch (NotEnoughMoneyException e)
            {
                Console.WriteLine(e.Message);
            }
        }
        
        //Запись словаря dictOfClients в текстовый файл
        public void WriteClientsToFile()
        {
            
            if (!MainDirectoryInfo.Exists)
            {
                MainDirectoryInfo.Create();
            }

            var jsonClients = JsonConvert.SerializeObject(listOfClients);
            byte[] arrayClients = System.Text.Encoding.Default.GetBytes(jsonClients);
            //запись списка клиентов в файл
            using (FileStream fileStream = new FileStream($"{MainPath}\\{ClientsfileName}", FileMode.Append))
            {
                fileStream.Write(arrayClients,0,arrayClients.Length);
            }
        }

        public void WriteAccountsToFile()
        {
            var jsonAccounts = JsonConvert.SerializeObject(dictOfAccounts);
            byte[] arrayAccounts = System.Text.Encoding.Default.GetBytes(jsonAccounts);
            using (FileStream fileStream = new FileStream($"{MainPath}\\{AccountsFileName}", FileMode.Append))
            {
                fileStream.Write(arrayAccounts,0,arrayAccounts.Length);
            }
        }


        public static List<Client> ReadClientsFromFile()
        {
            List<Client> clients = null;
            using (FileStream fileStream = new FileStream($"{MainPath}\\{ClientsfileName}", FileMode.Open))
            {
                byte[] filearray = new byte[fileStream.Length];
                fileStream.Read(filearray, 0, filearray.Length);
                string fileString = System.Text.Encoding.Default.GetString(filearray);
                clients = JsonConvert.DeserializeObject<List<Client>>(fileString);
                //парсинг клиентов и счетов из строки
                return clients;
            }
        }
        //чтение из файла в словарь dictOfAccountsFromFile
        public static Dictionary<string, List<Account>> ReadAccountsFromFile()
        {
            //словарь клиентов и счетов который будет прочитан из файла
            Dictionary<string, List<Account>> dictOfAccountsfromFile = null;
            
            using (FileStream fileStream = new FileStream($"{MainPath}\\{AccountsFileName}", FileMode.Open))
            {
                byte[] filearray = new byte[fileStream.Length];
                fileStream.Read(filearray, 0, filearray.Length);
                string fileString = System.Text.Encoding.Default.GetString(filearray);
                dictOfAccountsfromFile = JsonConvert.DeserializeObject<Dictionary<string, List<Account>>>(fileString);
                return dictOfAccountsfromFile;
            }
        }
    }
}