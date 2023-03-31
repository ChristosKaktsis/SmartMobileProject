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
    class RepositoryΓραμμέςΠαρΕισπράξεων : IDataControl
    {
        AppShell app = (AppShell)Application.Current.MainPage;
        public async Task<bool> UpdateItemAsync()
        {
            try
            {
                using (var uow = XpoHelper.CreateUnitOfWork())
                {
                    var allItemsList = await Task.Run(() =>
                    {
                        return uow.Query<ΓραμμέςΠαραστατικώνΕισπράξεων>().Where(
                        x => x.ΠαραστατικάΕισπράξεων.Πωλητής.SmartOid == App.Πωλητής.SmartOid &&
                        x.IsUploaded == false).ToList<ΓραμμέςΠαραστατικώνΕισπράξεων>();
                    });
                    if (allItemsList.Any())
                    {
                        var jsonlist = await ConvertToJson(allItemsList);
                        //var can = await XpoHelper.setSmartTable(jsonlist);

                        //if (can)
                        //{
                        //    foreach (var item in allItemsList)
                        //        item.IsUploaded = true;

                        //    await uow.CommitChangesAsync();
                        //}
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
        Task<string> ConvertToJson(List<ΓραμμέςΠαραστατικώνΕισπράξεων> items)
        {
            var task = Task<string>.Run(() =>
            {
                string json = "{\"ΓραμμέςΠαραστατικώνΕισπράξεων\":[";
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
