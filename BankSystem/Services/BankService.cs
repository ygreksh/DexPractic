using System;
using System.Collections.Generic;
using System.Linq;
using BankSystem.Exceptions;

namespace BankSystem
{
    public class BankService
    {
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
            Person person = new Person("", 30,PassportNumber);
            return listOfPersons.Find(x => x.Equals(person));
        }
        
        //ДОбавление нового счета Account пользователю в словаре
        public static void AddClientAccount(Account account, Client client, Dictionary<Client, List<Account>> dictOfClients)
        {
            //если такого клиента нет в словаре - создаем нового клиента
            if (dictOfClients.ContainsKey(client) == false)
            {
                dictOfClients.Add(client, new List<Account>(){account});
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