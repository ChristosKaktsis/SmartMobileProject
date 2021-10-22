
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
                if (Preferences.Get("Active", false))
                    return;

                var Token =  Preferences.Get("token",null);
                var yesterday =  Preferences.Get("yesterday",null);
                if (Token == null)
                {
                    var startDate = DateTime.Today;
                    Preferences.Set("token", startDate.ToString());
                    Preferences.Set("yesterday", DateTime.Now.ToString());
                    Preferences.Set("DaysLeft", (startDate.AddDays(30) - DateTime.Now).Days);    
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
                        Preferences.Set("yesterday", DateTime.Now.ToString());
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
