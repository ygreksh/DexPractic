namespace BankSystem
{
    public class Employee : Person
    {
        public string Position { get; set; }    //должность
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

       
    }
}