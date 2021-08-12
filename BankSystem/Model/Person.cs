using System;
using BankSystem.Exceptions;

namespace BankSystem
{
    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string PassportNumber { get; set; }

        public Person(string name, int age, string passportNumber)
        {
            
            {
                Name = name;
                Age = age;
                PassportNumber = passportNumber;    
            }
            
        }

        Person()
        {
            throw new NotImplementedException();
        }

        public override int GetHashCode()
        {
            return PassportNumber.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (!(obj is Person)) return false;
            Person person = (Person) obj;
            return PassportNumber == person.PassportNumber;
        }

        public override string ToString()
        {
            return PassportNumber + ", " + Name;
        }
    }
     
}