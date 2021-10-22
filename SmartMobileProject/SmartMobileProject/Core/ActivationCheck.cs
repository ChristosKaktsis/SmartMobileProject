using SmartMobileProject.Services;
using System;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace SmartMobileProject.Core
{
    class ActivationCheck
    {
        public static Task<bool> CheckActivationCode(string code, string ID)
        {
            return Task.Run( () =>
            {
                string decrypto = AesOperation.DecryptString("b14ca5898a4e4133bbce2ea2315a1916", code);
                Console.WriteLine("After Decrypt :" + decrypto); 
                if (string.IsNullOrEmpty(decrypto))
                    return false;
                char[] separators = new char[] { ' ' };
                string[] subs = decrypto.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                if (subs.Length != 3)
                    return false;
                //string Id =  Preferences.Get("ID",null);
                if (subs[0].Equals("OK") && subs[2].Equals(ID))
                {
                    Preferences.Set("Lock", false);
                    Preferences.Set("Active", true);
                    return true;
                }
                else
                {
                    return false;
                }
                
            });
        }
    }
}
