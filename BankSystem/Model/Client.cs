using System;
using BankSystem.Exceptions;

namespace BankSystem
{
    public class Client : Person
    {
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public Client(string name, int age, string passportNumber) : base(name, age, passportNumber)
        {
            try
            {
                if (age < 18)
                {
                    throw new WrongAgeException("Возраст клиента должен быть больше или равен 18!");
                }
                else
                {
                    base.Name = name;
                    base.Age = age;
                    base.PassportNumber = passportNumber;
                }
            }
            catch (WrongAgeException e)
            {
                Console.WriteLine(e.Message);
            }
            
        }
        
    }
}