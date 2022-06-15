using DevExpress.Xpo;
using SmartMobileProject.Core;
using SmartMobileProject.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SmartMobileProject.Services.Repositories
{
    class RepositoryΠελάτες : IDataControl
    {
        AppShell app = (AppShell)Application.Current.MainPage;
        public async Task<bool> UpdateItemAsync()
        {
            bool result = false;
            using(var uow = XpoHelper.CreateUnitOfWork())
            {
                var PelatesItemsList = await Task.Run(() =>
                  {
                     return  uow.Query<Πελάτης>().Where(
                          x => x.Πωλητής.SmartOid == app.πωλητής.SmartOid && x.CanUpload == true).ToList<Πελάτης>();
                  });
               
                if (PelatesItemsList.Any())
                {
                    try
                    {
                        var jsonlist = await ConvertToJson(PelatesItemsList);
                        var can = await XpoHelper.setSmartTable(jsonlist, "Customers");
                        SendImageToCustomers(PelatesItemsList);
                        if (can)
                        {
                            foreach (var item in PelatesItemsList)
                                item.CanUpload = false;

                            await uow.CommitChangesAsync();
                            result = true;
                        }
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                }
            }
            return result;
        }
        //Task<string> ConvertToJson(List<Πελάτης> items)
        //{
        //    var task = Task<string>.Run(() =>
        //    {
        //        string json = "{\"Πελάτες\":[";
        //        foreach (var item in items)
        //        {
        //            json += item.ToJson() + ",";
        //        }
        //        json = json.Remove(json.Count() - 1);
        //        json += "]}";
        //        return json;
        //    });
        //    return task;
        //}
        Task<string> ConvertToJson(List<Πελάτης> items)
        {
            var task = Task<string>.Run(() =>
            {
                string json = "{\"Πελάτης\":[";
                foreach (var item in items)
                {
                    json += item.ToJson() + ",";
                }
                json = json.Remove(json.Count() - 1);
                json += "]}";
                return json;
            });
            return task;
        }
        void SendImageToCustomers(List<Πελάτης> items)
        {
            foreach (var item in items)
                SendImage(item);
        }
        private Stream StreamImage(string imageBytes)
        {
            if (string.IsNullOrWhiteSpace(imageBytes))
                return null;
            var bytearray = Convert.FromBase64String(imageBytes);
            Stream stream = new MemoryStream(bytearray);
            
            return stream;
        }
        async void SendImage(Πελάτης customer)
        {
            if (customer == null)
                return;
            try
            {
                var content = new MultipartFormDataContent();
                //content.Add(new StreamContent(await ImageFile.OpenReadAsync()), "file", ImageFile.FileName);
                content.Add(new StreamContent(StreamImage(customer.ImageBytes)), "file", customer.ImageName);
                //var response = await httpClient.PostAsync($"http://{ip}:{port}//mobile/Files/Upload", content);
                var client = GetClient();
                string ip = Preferences.Get("IP", "79.129.5.42");
                string port = Preferences.Get("Port2", "8882");
                var response = await client.PostAsync($"http://{ip}:{port}/api/UploadImage", content);
                var st = response.StatusCode.ToString();
                Console.WriteLine(st);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            HttpClient GetClient()
            {
                HttpClientHandler clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
                var httpClient = new HttpClient(clientHandler);
                string authval = $"{Preferences.Get("uname", "DemoAdmin")}:{ Preferences.Get("passwrd", "DemoPass")}";
                var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(authval);
                string conv = System.Convert.ToBase64String(plainTextBytes);
                httpClient.DefaultRequestHeaders.Add("Authorization", "Basic " + conv);
                return httpClient;
            }
        }
    }
}
