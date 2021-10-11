
using SmartMobileProject.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SmartMobileProject.Core
{
    public class TrialCheck
    {
        public static async void Check()
        {
            try
            {
                var Token = await SecureStorage.GetAsync("token");
                var yesterday = await SecureStorage.GetAsync("yesterday");
                if (Token == null)
                {
                    var startDate = DateTime.Today;
                    await SecureStorage.SetAsync("token", startDate.ToString());
                    await SecureStorage.SetAsync("yesterday", DateTime.Now.ToString());
                }
                else
                {  
                    var expire = DateTime.Parse(Token);
                    if (DateTime.Now < DateTime.Parse(yesterday))
                    {
                        await Application.Current.MainPage.DisplayAlert("Free Trial",
                     "Φαίνεται πως έχει αλλάξει η ημερομινία τηλεφώνου με τρόπο που επηρεάζει την δωρεαν δοκιμή.", "Εντάξει");
                        Preferences.Set("Lock", true);
                        return;
                    }
                    if(DateTime.Now > expire.AddDays(30))
                    {
                     //   await Application.Current.MainPage.DisplayAlert("Free Trial",
                     //"Έχει λήξει ή δωρεάν δοκιμή 30 ημερών", "Εντάξει");
                        Preferences.Set("Lock", true);
                    }
                    else
                    {
                     //   await Application.Current.MainPage.DisplayAlert("Free Trial",
                     //"Έχετε ακόμα "+ (expire.AddDays(30) - DateTime.Now).Days+" μέρες", "Εντάξει");
                        await SecureStorage.SetAsync("yesterday", DateTime.Now.ToString());
                        Preferences.Set("DaysLeft", (expire.AddDays(30) - DateTime.Now).Days);
                    } 
                }
            }
            catch (Exception ex)
            {
                // Possible that device doesn't support secure storage on device.
                Console.WriteLine(ex);
            }
        }

       
    }
}
