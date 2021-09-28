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
    class RepositoryΠαρΠωλήσεων : IDataControl
    {
        AppShell app = (AppShell)Application.Current.MainPage;
        public async Task<bool> UpdateItemAsync()
        {
            try
            {
                using (var uow = XpoHelper.CreateUnitOfWork())
                {
                    var ParItemsList = await Task.Run(() =>
                    {
                        return uow.Query<ΠαραστατικάΠωλήσεων>().Where(
                        x => x.Πωλητής.SmartOid == app.πωλητής.SmartOid &&
                        x.IsUploaded == false).ToList<ΠαραστατικάΠωλήσεων>();
                    });
                    
                    if (ParItemsList.Any())
                    {
                        var jsonlist = await ConvertToJson(ParItemsList);
                        var can = await XpoHelper.setSmartTable(jsonlist,"Sales");

                        if (can)
                        {
                            foreach (var item in ParItemsList)
                                item.IsUploaded = true;

                            await uow.CommitChangesAsync();
                        }
                        else
                        {
                            return false;
                        }
                    }
                    return true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
        Task<string> ConvertToJson(List<ΠαραστατικάΠωλήσεων> items)
        {
            var task = Task<string>.Run(() =>
            {
                string json = "{\"ΠαραστατικάΠωλήσεων\":[";
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
