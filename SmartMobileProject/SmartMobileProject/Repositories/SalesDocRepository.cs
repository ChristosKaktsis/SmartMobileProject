using SmartMobileProject.Models;
using SmartMobileProject.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using SmartMobileProject.Core;
using System.Net.Http;
using System.Threading.Tasks;
using DevExpress.Xpo;
using System.Linq;
using System.Diagnostics;

namespace SmartMobileProject.Repositories
{
    internal class SalesDocRepository : IDatabase<ΠαραστατικάΠωλήσεων>
    {
        public async Task<bool> AddItemAsync(ΠαραστατικάΠωλήσεων item)
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
                Debug.WriteLine($"Coud not add {item}.");
                Debug.WriteLine($"Coud not add {ex}.");
                return false;
            }
        }
        public async Task<bool> DeleteItemAsync(string id)
        {
            try
            {
                using (var uow = XpoHelper.CreateUnitOfWork())
                {
                    var itemToDelete = uow.Query<ΠαραστατικάΠωλήσεων>()
                        .Where(i=>i.SmartOid.ToString() == id).FirstOrDefault();
                    if (itemToDelete == null) return false;
                    if (IsOrderLast(itemToDelete))
                        itemToDelete.Σειρά.Counter--;
                    uow.Delete(itemToDelete);
                    await uow.CommitChangesAsync();
                    return true;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return false;
            }
        }
        private bool IsOrderLast(ΠαραστατικάΠωλήσεων order)
        {
            return order.Σειρά.Counter == order.OrderSeiraCounter();
        }
        public async Task<ΠαραστατικάΠωλήσεων> GetItemAsync(string id)
        {
            using (var uow = XpoHelper.CreateUnitOfWork())
            {
                return await uow.GetObjectByKeyAsync<ΠαραστατικάΠωλήσεων>(id);
            }
        }

        public async Task<IEnumerable<ΠαραστατικάΠωλήσεων>> GetItemsAsync()
        {
            using (var uow = XpoHelper.CreateUnitOfWork())
            {
                return await uow.Query<ΠαραστατικάΠωλήσεων>().OrderByDescending(i => i.Ημνία).ToListAsync();
            }
        }

        public async Task<bool> UpdateItemAsync(ΠαραστατικάΠωλήσεων item)
        {
            try
            {
                using (var uow = XpoHelper.CreateUnitOfWork())
                {
                    var itemToUpdate = await uow.GetObjectByKeyAsync<ΠαραστατικάΠωλήσεων>(item.Oid);
                    if (itemToUpdate == null) return false;

                    //itemToUpdate.Ημνία = item.Ημνία;
                    //itemToUpdate.Σειρά = await uow.GetObjectByKeyAsync<ΣειρέςΠαραστατικώνΠωλήσεων>(item.Σειρά.Oid);
                    //itemToUpdate.Παραστατικό = item.Παραστατικό;
                    //itemToUpdate.IsUploaded = item.IsUploaded;
                    //itemToUpdate.Πελάτης = await uow.GetObjectByKeyAsync<Πελάτης>(item.Πελάτης.Oid);
                    //itemToUpdate.ΔιεύθυνσηΠαράδοσης = await uow.GetObjectByKeyAsync<Address>(item.ΔιεύθυνσηΠαράδοσης.Oid);
                    //itemToUpdate.Πωλητής = await uow.GetObjectByKeyAsync<Πωλητής>(item.Πελάτης.Πωλητής);
                    //itemToUpdate.ΤρόποςΠληρωμής = item.ΤρόποςΠληρωμής;
                    //itemToUpdate.ΤρόποςΑποστολής = item.ΤρόποςΑποστολής;
                    //itemToUpdate.Ποσότητα = item.Ποσότητα;
                    //itemToUpdate.ΚαθαρήΑξία = item.ΚαθαρήΑξία;
                    //itemToUpdate.ΑξίαΕκπτωσης = item.ΑξίαΕκπτωσης;
                    //itemToUpdate.Φπα = item.Φπα;
                    //itemToUpdate.ΑξίαΠαραστατικού = item.ΑξίαΠαραστατικού;
                    //itemToUpdate.ΗμνίαΔημ = item.ΗμνίαΔημ;
                    //itemToUpdate.Σχολια = item.Σχολια;

                    uow.Save(itemToUpdate);
                    await uow.CommitChangesAsync();
                    Debug.WriteLine($"{item} Updated.");
                    return true;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Could not update {item}.");
                Debug.WriteLine($"Could not update {e}.");
                return false;
            }
        }
    }
}
