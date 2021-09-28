using DevExpress.Xpo;
using SmartMobileProject.Core;
using SmartMobileProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SmartMobileProject.Services.Repositories
{
    class RepositoryΠελάτες : IDataControl
    {
        AppShell app = (AppShell)Application.Current.MainPage;
        public async Task<bool> UpdateItemAsync()
        {
            using(var uow = XpoHelper.CreateUnitOfWork())
            {
                var PelatesItemsList = await Task.Run(() =>
                  {
                     return  uow.Query<Πελάτης>().Where(
                          x => x.Πωλητής.SmartOid == app.πωλητής.SmartOid && x.CanUpload == true).ToList<Πελάτης>();
                  });
               
                if (PelatesItemsList.Any())
                {
                    var jsonlist = await ConvertToJson(PelatesItemsList);
                    var can = await XpoHelper.setSmartTable(jsonlist,"Customers");

                    if (can)
                    {
                        foreach (var item in PelatesItemsList)
                            item.CanUpload = false;

                        await uow.CommitChangesAsync();
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return true;
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
    }
}
