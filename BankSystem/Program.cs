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
            
            Dollar dollar = new Dollar() { CurrencyName = "USD" };
            Ruble ruble = new Ruble() { CurrencyName = "RUB" };
            Leu leu = new Leu() { CurrencyName = "MDL" };
            Hryvnia hryvnia = new Hryvnia() { CurrencyName = "UAH" };
            
            bankService.dictOfCurrency.Add(dollar.CurrencyName,dollar);
            bankService.dictOfCurrency.Add(ruble.CurrencyName,ruble);
            bankService.dictOfCurrency.Add(leu.CurrencyName,leu);
            bankService.dictOfCurrency.Add(hryvnia.CurrencyName,hryvnia);
            
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
                Console.WriteLine($"{item.Key} = {item.Value.rate}");
            }
        }
    }
}