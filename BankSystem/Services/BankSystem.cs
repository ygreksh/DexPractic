using System.Collections.Generic;

namespace BankSystem
{
    public class BankSystem
    {
        public static Person FindPersonByPassportNumber<T>(string PassportNumber, List<T> listOfPersons) where T: Person    //обобщенный метод. работает только с экземплярами и наследниками Person
        {
            return listOfPersons.Find(x => x.Equals(new Person() {PassportNumber = PassportNumber}));
        }
        
    }
}