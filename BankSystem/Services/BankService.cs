﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using BankSystem.Exceptions;

namespace BankSystem
{
    public class BankService
    {
        //основной словарь клиентов
        public Dictionary<Client, List<Account>> dictOfClients = new Dictionary<Client, List<Account>>();
        

        public static string MainPath = Path.Combine("TestFiles");
        public DirectoryInfo MainDirectoryInfo = new DirectoryInfo(MainPath);
        public static string ClientsfileName = "clients.txt";
        public Func<double, Currency, Currency, double> _transfer;

        public void RegisterTransfer(Func<double, Currency, Currency, double> transfer)
        {
            _transfer = transfer;
        }
        

        //обобщенный метод. работает только с экземплярами и наследниками Person
        public static Person FindPerson<T>(string PassportNumber, List<T> listOfPersons) where T: Person
        {
            Person person = new Person(){PassportNumber = PassportNumber};
            return listOfPersons.Find(x => x.Equals(person));
        }
        
        //поиск в файле
        public static Person FindPersonInFile<T>(string PassportNumber) where T: Person
        {
            Dictionary<Client, List<Account>> dictOfPersons = ReadClientsFromFile();
            List<Person> listOfPersons = null;
            //из словаря в список
            foreach (var item in dictOfPersons)
            {
                listOfPersons.Add(item.Key);
            }
            return FindPerson(PassportNumber, listOfPersons);
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
                else if (!dictOfClients.ContainsKey(client))
                {
                    List<Account> listOfAccounts = new List<Account>();
                    dictOfClients.Add(client, listOfAccounts);  //добавление в словарь
                    AddClientToFile(client, listOfAccounts);    //добавление в файл
                }
            }
            catch (WrongAgeException e)
            {
                Console.WriteLine(e.Message);
            }
        }
        
        //Добавление нового клиента в файл
        public void AddClientToFile(Client client, List<Account> listOfAccounts)
        {
            Dictionary<Client, List<Account>> dictOfClientsFromFile = ReadClientsFromFile();
            //если такого клиента нет в файле - добавляется
            if (!dictOfClientsFromFile.ContainsKey(client))
            {
                if (!MainDirectoryInfo.Exists)
                {
                    MainDirectoryInfo.Create();
                }
            
                using (FileStream fileStream = new FileStream($"{MainPath}\\{ClientsfileName}", FileMode.Append))
                {
                    string fieldSeparator = " ";    //разделитель полей
                    string accountSeparator = ",";  //разделитель информации о клиенте и счетов
                    string clientSeparator = "\n";  //разделитель клиентов
                    string clientString = "";
                
                    clientString += client.PassportNumber +  fieldSeparator +
                                    client.Name +  fieldSeparator +
                                    client.Age.ToString();
                    string accountString = "";
                    foreach (var account in listOfAccounts)
                    {
                        accountString += accountSeparator + 
                                         account.currency + fieldSeparator + 
                                         account.value.ToString();
                    }
                    clientString += accountString;
                    clientString += clientSeparator;
                
                    byte[] clientArray = System.Text.Encoding.Default.GetBytes(clientString);
                    fileStream.Write(clientArray,0,clientArray.Length);
                }
            }
            
        }
        
        //Добавление нового счета Account пользователю в словаре и файле
        public void AddClientAccount(Account account, Client client)
        {
            //если такого клиента нет в словаре - создаем нового клиента
            if (dictOfClients.ContainsKey(client) == false)
            {
                List<Account> listOfAccounts = new List<Account>();
                listOfAccounts.Add(account);
                dictOfClients.Add(client, listOfAccounts);  //добавление в словарь
                AddClientToFile(client, listOfAccounts);
            }
            //если искомый уже клиент есть, добавляется ещё один Account в listOfAccounts
            else
            {
                dictOfClients[client].Add(account);
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
            
            using (FileStream fileStream = new FileStream($"{MainPath}\\{ClientsfileName}", FileMode.Append))
            {
                string fieldSeparator = " ";    //разделитель полей
                string accountSeparator = ",";  //разделитель информации о клиенте и счетов
                string clientSeparator = "\n";  //разделитель клиентов
                string clientString = "";
                foreach (var item in dictOfClients)
                {
                    clientString += item.Key.PassportNumber +  fieldSeparator +
                                    item.Key.Name +  fieldSeparator +
                                    item.Key.Age.ToString();
                    string accountString = "";
                    foreach (var account in item.Value)
                    {
                        accountString += accountSeparator + 
                                         account.currency + fieldSeparator + 
                                         account.value.ToString();
                    }
                    clientString += accountString;
                    clientString += clientSeparator;
                }
                byte[] clientArray = System.Text.Encoding.Default.GetBytes(clientString);
                fileStream.Write(clientArray,0,clientArray.Length);
            }
            
        }
        //чтение из файла в словарь dictOfClientsFromFile
        public static Dictionary<Client, List<Account>> ReadClientsFromFile()
        {
            //словарь клиентов который будет прочитан из файла
            Dictionary<Client, List<Account>> dictOfClientsfromFile = new Dictionary<Client, List<Account>>();
            char fieldSeparator = ' ';      //разделитель полей
            char accountSeparator = ',';    //разделитель информации о клиенте и счетов
            char clientSeparator = '\n';    //разделитель клиентов

            using (FileStream fileStream = new FileStream($"{MainPath}\\{ClientsfileName}", FileMode.Open))
            {
                byte[] filearray = new byte[fileStream.Length];
                fileStream.Read(filearray, 0, filearray.Length);
                string fileString = System.Text.Encoding.Default.GetString(filearray);
                
                //парсинг клиентов и счетов из строки
                //строки с клиентами
                string[] arrayStringClients = fileString.Split(clientSeparator);
                foreach (var stringClient in arrayStringClients)
                {
                    if (stringClient != "")
                    {
                        //строки [0] - клиент, [1] и далее это счета
                        string[] arrayStringAccounts = stringClient.Split(accountSeparator);
                        string[] arrayclient = arrayStringAccounts[0].Split(fieldSeparator);
                        //парсинг информации о клиенте
                        string clientPassportnumber = arrayclient[0];
                        string clientName = arrayclient[1];
                        string clientAge = arrayclient[2];
                        Client client = new Client() {Name = clientName, Age = Int32.Parse(clientAge), PassportNumber = clientPassportnumber};
                        //парсинг счетов
                        List<Account> listOfAccounts = new List<Account>();
                        for (int i = 1; i < arrayStringAccounts.Length; i++)
                        {
                            string[] strAccount = arrayStringAccounts[i].Split(fieldSeparator);
                            string strCurrencyName = strAccount[0];
                            string strValue = strAccount[1];
                            Currency currency = null;
                            switch (strCurrencyName)
                            {
                                case "BankSystem.Dollar": currency = new Dollar() { CurrencyName = "Dollar", rate = 1 }; break;
                                case "BankSystem.Ruble": currency = new Ruble() { CurrencyName = "Ruble", rate = 77 }; break;
                                case "BankSystem.Leu": currency = new Leu() { CurrencyName = "Leu", rate = 12 }; break;
                                case "BankSystem.Hryvnia": currency = new Hryvnia() { CurrencyName = "Hryvnia", rate = 27 }; break;
                                default: break;
                            }
                            listOfAccounts.Add(new Account(){currency = currency, value = Double.Parse(strValue)});
                        }

                        if (!dictOfClientsfromFile.ContainsKey(client))
                        {
                            dictOfClientsfromFile.Add(client,listOfAccounts);
                            
                        }
                    }
                    
                    
                }
                return dictOfClientsfromFile;
            }
        }
    }
}