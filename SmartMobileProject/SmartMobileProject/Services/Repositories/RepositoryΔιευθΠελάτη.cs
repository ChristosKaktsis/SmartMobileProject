using DevExpress.Xpo;
using SmartMobileProject.Core;
using SmartMobileProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;


namespace SmartMobileProject.Services.Repositories
{
    class RepositoryΔιευθΠελάτη : IDataControl
    {
        AppShell app = (AppShell)Application.Current.MainPage;
        public async Task<bool> UpdateItemAsync()
        {
            using (var uow = XpoHelper.CreateUnitOfWork())
            {
                var allItemsList = await Task.Run(() =>
                {
                    return uow.Query<ΔιευθύνσειςΠελάτη>().Where(
                        x => x.Πελάτης.Πωλητής.SmartOid == app.πωλητής.SmartOid 
                        && x.CanUpload == true).ToList<ΔιευθύνσειςΠελάτη>();
                });
                if (allItemsList.Any())
                {
                    var jsonlist = await ConvertToJson(allItemsList);
                    var can = await XpoHelper.setSmartTable(jsonlist,"CustomerAddress");

                    if (can)
                    {
                        foreach (var item in allItemsList)
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
        Task<string> ConvertToJson(List<ΔιευθύνσειςΠελάτη> items)
        {
            var task = Task<string>.Run(() =>
            {
                string json = "{\"ΔιευθύνσειςΠελάτη\":[";
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
