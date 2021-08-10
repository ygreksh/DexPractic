using System;

namespace BankSystem.Exceptions
{
    //Исключение недостаточного количества денег на счету
    public class NotEnoughMoneyException : Exception
    {
        public NotEnoughMoneyException(string message) : base(message)
        {
            
        }
    }
}