using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace BankSystem
{
    public class Currency
    {
        [JsonProperty("name")]
        public string currencyName { get; set; }
        [JsonProperty("rate")]
        public double rate { get; set; }
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (!(obj is Currency))
                return false;
            Currency newCurrency = (Currency)obj;
            return currencyName == newCurrency.currencyName; //валюта одна если имя совпадает
        }
        public override int GetHashCode()
        {
            return currencyName.GetHashCode();
        }

    }

}
