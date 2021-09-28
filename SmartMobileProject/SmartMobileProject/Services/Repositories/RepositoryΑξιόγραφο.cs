﻿using DevExpress.Xpo;
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
    class RepositoryΑξιόγραφο : IDataControl
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
                        return uow.Query<Αξιόγραφα>().Where(
                        x => x.IsUploaded == false).ToList<Αξιόγραφα>();
                    });
                    if (allItemsList.Any())
                    {
                        var jsonlist = await ConvertToJson(allItemsList);
                        var can = await XpoHelper.setSmartTable(jsonlist,"Aksiografo");

                        if (can)
                        {
                            foreach (var item in allItemsList)
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
        Task<string> ConvertToJson(List<Αξιόγραφα> items)
        {
            var task = Task<string>.Run(() =>
            {
                string json = "{\"Αξιόγραφα\":[";
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
