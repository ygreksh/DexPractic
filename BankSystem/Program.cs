using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BankSystem.Exceptions;
using Newtonsoft.Json;

namespace BankSystem
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            BankService bankService = new BankService();
            
            Dollar dollar = new Dollar() { currencyName = "USD" };
            Ruble ruble = new Ruble() { currencyName = "RUB" };
            Leu leu = new Leu() { currencyName = "MDL" };
            Hryvnia hryvnia = new Hryvnia() { currencyName = "UAH" };
            
            bankService.dictOfCurrency.Add(dollar.currencyName,dollar);
            bankService.dictOfCurrency.Add(ruble.currencyName,ruble);
            bankService.dictOfCurrency.Add(leu.currencyName,leu);
            bankService.dictOfCurrency.Add(hryvnia.currencyName,hryvnia);
            
            CurrencyService currencyService = new CurrencyService();
            CurrencyResponse currencyResponse;
            //получение информации о валюте 
            currencyResponse = await currencyService.GetExchangeRates();
            //Console.WriteLine(currencyResponse);
            //присвоение значений Rate для валюты в bankservice
            currencyService.AssigningCurrenryRates(currencyResponse, bankService.dictOfCurrency);
            
            //проверка: вывод значений Rate из словаря dictOfCurrency
            foreach (var item in bankService.dictOfCurrency)
            {
                Console.WriteLine($"{item.Value.currencyName} = {item.Value.rate}");
            }
        }
    }
}