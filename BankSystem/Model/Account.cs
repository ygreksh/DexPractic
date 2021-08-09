using System;
using System.Collections.Generic;
using System.Text;

namespace BankSystem
{
    public class Account
    {
        public Currency currency { get; set; }
        public double value { get; set; }

        public override int GetHashCode()
        {
            return currency.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (!(obj is Account))
                return false;
            Account newAccount = (Account)obj;
            return currency.Equals(newAccount.currency); //уникальность счетов в одной коллекции определяется уникальностью их валюты
        }
    }
}
