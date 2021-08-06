using System;

namespace BankSystem
{
    public class Person
    {
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public string PassportNumber { get; set; }

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