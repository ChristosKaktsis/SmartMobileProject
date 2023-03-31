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
    class RepositoryΠαρΕισπράξεων : IDataControl
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
                        return uow.Query<ΠαραστατικάΕισπράξεων>().Where(
                        x => x.Πωλητής.SmartOid == App.Πωλητής.SmartOid &&
                        x.IsUploaded == false).ToList<ΠαραστατικάΕισπράξεων>();
                    });
                   
                    if (ParItemsList.Any())
                    {
                        var jsonlist = await ConvertToJson(ParItemsList);
                        var can = await XpoHelper.setSmartTable(jsonlist,"Income");

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
        Task<string> ConvertToJson(List<ΠαραστατικάΕισπράξεων> items)
        {
            var task = Task<string>.Run(() =>
            {
                string json = "{\"ΠαραστατικάΕισπράξεων\":[";
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
