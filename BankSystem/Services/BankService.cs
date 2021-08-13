using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BankSystem.Exceptions;

namespace BankSystem
{
    public class BankService
    {
        public Dictionary<Client, List<Account>> dictOfClients = new Dictionary<Client, List<Account>>();

        //Делегат
        //public delegate double Transfer(double sum, Currency fromCurrency, Currency toCurrency);
        //Func
        public Func<double, Currency, Currency, double> _transfer;

        public void RegisterTransfer(Func<double, Currency, Currency, double> transfer)
        {
            _transfer = transfer;
        }
        

        //обобщенный метод. работает только с экземплярами и наследниками Person
        public static Person FindPersonByPassportNumber<T>(string PassportNumber, List<T> listOfPersons) where T: Person
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
                else if (!dictOfClients.ContainsKey(client))
                {
                    dictOfClients.Add(client, new List<Account>());
                    
                    string path = Path.Combine("TestFiles");
                    DirectoryInfo directoryInfo = new DirectoryInfo(path);

                    if (!directoryInfo.Exists)
                    {
                        directoryInfo.Create();
                    }
                    using (FileStream fileStream = new FileStream($"{path}\\clients.txt", FileMode.Append))
                    {
                        string clientSeparator = ";\n";
                        string fieldSeparator = " ";
                        string accountSeparator = ",";
                        string clientString = client.Name + fieldSeparator + 
                                              client.Age + fieldSeparator + 
                                              client.PassportNumber + clientSeparator;
                        byte[] textArray = System.Text.Encoding.Default.GetBytes(clientString);
                        fileStream.Write(textArray,0,textArray.Length);
                
                    }
                }
            }
            catch (WrongAgeException e)
            {
                Console.WriteLine(e.Message);
            }
        }
        
        //ДОбавление нового счета Account пользователю в словаре
        public void AddClientAccount(Account account, Client client)
        {
            string path = Path.Combine("TestFiles");
            DirectoryInfo directoryInfo = new DirectoryInfo(path);

            if (!directoryInfo.Exists)
            {
                directoryInfo.Create();
            }
            /*
            using (FileStream fileStream = new FileStream($"{path}\\clients.txt", FileMode.Append))
            {
                string sometext = "Некоторый текст";
                byte[] textArray = System.Text.Encoding.Default.GetBytes(sometext);
                fileStream.Write(textArray,0,textArray.Length);
                
            }
            */
            //если такого клиента нет в словаре - создаем нового клиента
            if (dictOfClients.ContainsKey(client) == false)
            {
                AddClient(client.Name, client.Age, client.Name);
                dictOfClients.Add(client, new List<Account>() { account });
            }
            //если искомый уже клиент есть, добавляется ещё один Accaunt в listOfAccounts
            else
            {
                List<Account> listOfAccounts;
                dictOfClients.TryGetValue(client, out listOfAccounts);
                Client foundclient = (Client)FindPersonByPassportNumber<Client>(client.PassportNumber, dictOfClients.Keys.ToList());
                listOfAccounts.Add(account);
                dictOfClients.Remove(foundclient);
                dictOfClients.Add(foundclient, listOfAccounts);
            }
        }
        //Переод денег между счетами без комиссии
        public void TransferMoney(double Sum, Account donorAccount, Account recipientAccount, Func<double, Currency, Currency, double> transfermoney)
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
        public void TransferMoneyWithTax(double Sum, Account donorAccount, Account recipientAccount, Func<double, Currency, Currency, double> transfermoney)
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
    }
}