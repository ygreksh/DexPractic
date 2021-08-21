using System;

namespace BankSystem
{
    public class ExtractData
    {
        public static void GetData<T>(T obj)
        {
            var objType = obj.GetType();
            Console.WriteLine(objType.FullName);
            var objProperties = objType.GetProperties();
            var objMethods = objType.GetMethods();
            Console.WriteLine("Properties:");
            foreach (var property in objProperties)
            {
                Console.WriteLine($"    {property.PropertyType} {property.Name} = {property.GetValue(obj)}");
            }
            Console.WriteLine("Methods:");
            foreach (var method in objMethods)
            {
                Console.WriteLine($"    {method.MemberType} {method.ReturnType} {method.Name}()");
            }
        }
    }
}