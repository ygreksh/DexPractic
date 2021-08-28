using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BankSystem
{
    public class CurrencyService
    {
        //получение значений rate из внешнего сервиса
        public async Task<CurrencyResponse> GetExchangeRates()
        {
            //Сервис https://openexchangerates.org
            //base currency - USD
            string _token = "6f4e6254cce34c899e27f556ba3bb5a3";
            
            HttpResponseMessage responseMessage;
            CurrencyResponse currencyResponse;
            using (HttpClient client = new HttpClient())
            {
                responseMessage = await client.GetAsync($"https://openexchangerates.org/api/latest.json?app_id={_token}");
                responseMessage.EnsureSuccessStatusCode();
                string serializedMessage = await responseMessage.Content.ReadAsStringAsync();
                currencyResponse = JsonConvert.DeserializeObject<CurrencyResponse>(serializedMessage);
            }
            return currencyResponse;
        }

        //присвоение значений Rate в словарь валюты
        public void AssigningCurrenryRates(CurrencyResponse currencyResponse, Dictionary<string, Currency> dictOfCurrency)
        {
            foreach (var item in currencyResponse.Rates)
            {
                if (dictOfCurrency.ContainsKey(item.Key))
                {
                    dictOfCurrency[item.Key].rate = item.Value;
                }
            }
        }
    }
}