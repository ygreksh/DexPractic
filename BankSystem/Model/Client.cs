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
        }

        public Client()
        {
            throw new System.NotImplementedException();
        }
    }
}