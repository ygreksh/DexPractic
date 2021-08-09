using System;
using System.Collections.Generic;

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

        public void TransferMoney(double Sum, Account fromAccount, Account toAccount, Transfer transfermoney)
        {
            transfermoney.Invoke(Sum, fromAccount, toAccount);
        }
    }
}