using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BankSystem
{
    public class CurrencyService
    {
        public async Task<CurrencyResponse> GetExchangeRate()
        {
            //https://openexchangerates.org
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
    }
}