using System;

namespace BankSystem.Exceptions
{
    //Исключение неправильного возраста 
    public class WrongAgeException : Exception
    {
        public WrongAgeException(string message) : base(message)
        {
            
        }
    }
}