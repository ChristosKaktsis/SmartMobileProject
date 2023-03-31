using SmartMobileProject.Models;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using SmartMobileProject.Core;
using System.Threading.Tasks;
using DevExpress.Xpo;
using System.Linq;
using System.Diagnostics;

namespace SmartMobileProject.Repositories
{
    public class SaleSeriesRepository : IDatabase<ΣειρέςΠαραστατικώνΠωλήσεων>
    {
        public async Task<bool> AddItemAsync(ΣειρέςΠαραστατικώνΠωλήσεων item)
        {
            try
            {
                using (var uow = XpoHelper.CreateUnitOfWork())
                {
                    item.SmartOid = Guid.NewGuid();
                    await uow.SaveAsync(item);
                    await uow.CommitChangesAsync();
                    Debug.WriteLine($"{item} added.");
                    return true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{ex}");
                Debug.WriteLine($"Coud not add {item}.");
                return false;
            }
        }

        public Task<bool> DeleteItemAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<ΣειρέςΠαραστατικώνΠωλήσεων> GetItemAsync(string id)
        {
            using (var uow = XpoHelper.CreateUnitOfWork())
            {
                return await uow.GetObjectByKeyAsync<ΣειρέςΠαραστατικώνΠωλήσεων>(id);
            }
        }

        public async Task<IEnumerable<ΣειρέςΠαραστατικώνΠωλήσεων>> GetItemsAsync()
        {
            using (var uow = XpoHelper.CreateUnitOfWork())
            {
                return await uow.Query<ΣειρέςΠαραστατικώνΠωλήσεων>().OrderBy(i => i.Σειρά).ToListAsync();
            }
        }
        public async Task<IEnumerable<ΣειρέςΠαραστατικώνΠωλήσεων>> GetItemsAsync(string Pid)
        {
            using (var uow = XpoHelper.CreateUnitOfWork())
            {
                return await uow.Query<ΣειρέςΠαραστατικώνΠωλήσεων>()
                    .Where(s => s.IDΠωλητή.ToString() == Pid)
                    .OrderBy(i => i.Σειρά).ToListAsync();
            }
        }
        public async Task<bool> UpdateItemAsync(ΣειρέςΠαραστατικώνΠωλήσεων item)
        {
            try
            {
                using (var uow = XpoHelper.CreateUnitOfWork())
                {
                    var itemToUpdate = await uow.GetObjectByKeyAsync<ΣειρέςΠαραστατικώνΠωλήσεων>(item.Oid);
                    if (itemToUpdate == null) return false;

                    itemToUpdate.Σειρά = item.Σειρά;
                    itemToUpdate.Περιγραφή = item.Περιγραφή;
                    itemToUpdate.Λιανική = item.Λιανική;
                    itemToUpdate.ΠρόθεμαΑρίθμησης = item.ΠρόθεμαΑρίθμησης;
                    itemToUpdate.Counter = item.Counter;
                    itemToUpdate.ΚίνησηΣυναλασόμενου = item.ΚίνησηΣυναλασόμενου;
                    itemToUpdate.IDΠωλητή = item.IDΠωλητή;
                    itemToUpdate.ΣκοπόςΔιακίνησης = item.ΣκοπόςΔιακίνησης;
                    uow.Save(itemToUpdate);
                    await uow.CommitChangesAsync();
                    Debug.WriteLine($"{item} Updated.");
                    return true;
                }
            }
            catch (Exception)
            {
                Debug.WriteLine($"Could not add {item}.");
                return false;
            }
        }
    }
}
