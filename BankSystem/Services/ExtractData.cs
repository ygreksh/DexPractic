using System;
using System.IO;

namespace BankSystem
{
    public class ExtractData
    {
        public static void GetData<T>(T obj, string filename)
        {
            string objDataPath = Path.Combine("ObjData");
            var objType = obj.GetType();
            var objProperties = objType.GetProperties();
            var objMethods = objType.GetMethods();
            var directoryInfo = new DirectoryInfo(objDataPath);
            if (!directoryInfo.Exists)
            {
                directoryInfo.Create();
            }

            using (FileStream fs = new FileStream($"{objDataPath}\\{filename}",FileMode.Create))
            {
                string objdata = "";
                objdata += objType.FullName + "\n";
                foreach (var property in objProperties)
                {
                    objdata += property.Name + " = " + property.GetValue(obj) + "\n";
                }

                foreach (var method in objMethods)
                {
                    objdata += method.Name + "\n";
                }
                byte[] arr = System.Text.Encoding.Default.GetBytes(objdata);
                fs.Write(arr, 0, arr.Length);
            }
            
        }
    }
}