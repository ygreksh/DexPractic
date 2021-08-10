﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace BankSystem
{
    public class BankSystem
    {
        //Делегат
        public delegate void Transfer(double sum, Account fromAccount, Account toAccount);
        
        //обобщенный метод. работает только с экземплярами и наследниками Person
        public static Person FindPersonByPassportNumber<T>(string PassportNumber, List<T> listOfPersons) where T: Person    
        {
            return listOfPersons.Find(x => x.Equals(new Person() {PassportNumber = PassportNumber}));
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
                listOfAccounts.Add(account);
                Client foundclient = (Client)FindPersonByPassportNumber<Client>(client.PassportNumber, dictOfClients.Keys.ToList());
                listOfAccounts.Add(account);
                dictOfClients.Add(foundclient, listOfAccounts);
    
            }
            
            /*
            foreach (var item in dictOfClients)
            {
                if (item.Key == foundclient)
                    item.Value = listOfAccounts;
            }
            */
        }

        public void TransferMoney(double Sum, Account fromAccount, Account toAccount, Transfer transfermoney)
        {
            transfermoney.Invoke(Sum, fromAccount, toAccount);
        }
    }
}