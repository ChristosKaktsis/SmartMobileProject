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
    internal class CustomerRepository : IDatabase<Πελάτης>
    {
        /*
          {
             "Oid":"cea01abc-f96e-4adb-b6c4-0130d9f641de",
             "Επωνυμία":"ΣΙΔΗΡΟΠΟΥΛΟΣ ΔΗΜΗΤΡΙΟΣ ΚΑΙ ΣΙΑ ΟΕ",
             "ΚατηγορίαΦΠΑ":0,
             "ΑΦΜ":"999261140",
             "Email":"info@exelixis-software.gr",
             "ΔΟΥ":"618f6486-9a96-4dfb-87ca-0246cbcd24dd",
             "ΚεντρικήΔιευθυνση":"e0102700-d67b-4ab9-8788-79e8f3709cab",
             "Πωλητής":null,
             "Κείμενο5":null,
             "ΔιακριτικόςΤίτλος":null
          }
        */
        
        public async Task<bool> AddItemAsync(Πελάτης item)
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
            catch (Exception)
            {
                Debug.WriteLine($"Coud not add {item}.");
                return false;
            }
        }
        public async Task<bool> UpdateItemAsync(Πελάτης item)
        {
            try
            {
                using (var uow = XpoHelper.CreateUnitOfWork())
                {
                    var itemToUpdate = await uow.GetObjectByKeyAsync<Πελάτης>(item.Oid);
                    if (itemToUpdate == null) return false;

                    //itemToUpdate.Κωδικός = item.Κωδικός;
                    //itemToUpdate.Επωνυμία = item.Επωνυμία;
                    //itemToUpdate.ΔιακριτικόςΤίτλος = item.ΔιακριτικόςΤίτλος;
                    //itemToUpdate.ΚατηγορίαΦΠΑ = item.ΚατηγορίαΦΠΑ;
                    //itemToUpdate.ΑΦΜ = item.ΑΦΜ;
                    //itemToUpdate.Email = item.Email;
                    //itemToUpdate.ΔΟΥ = item.ΔΟΥ;
                    //itemToUpdate.ΚεντρικήΔιευθυνση = item.ΚεντρικήΔιευθυνση;
                    //itemToUpdate.Πωλητής = item.Πωλητής;
                    //itemToUpdate.Κείμενο5 = item.Κείμενο5;
                    //itemToUpdate.CanUpload = item.CanUpload;
                    //itemToUpdate.ImageName = item.ImageName;
                    //itemToUpdate.ImageBytes = item.ImageBytes;
                    //itemToUpdate.Σημείωση1 = item.Σημείωση1;
                    //itemToUpdate.Σημείωση2 = item.Σημείωση2;
                    uow.Save(itemToUpdate);
                    await uow.CommitChangesAsync();
                    Debug.WriteLine($"{item} Updated.");
                    return true;
                }
            }
            catch (Exception)
            {
                Debug.WriteLine($"Could not update {item}.");
                return false;
            }
        }
        public async Task<bool> DeleteItemAsync(string id)
        {
            try
            {
                using (var uow = XpoHelper.CreateUnitOfWork())
                {
                    var itemToDelete = uow.GetObjectByKey<Πελάτης>(id);
                    if (itemToDelete != null)
                    {
                        uow.Delete(itemToDelete);
                        await uow.CommitChangesAsync();
                    }
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<Πελάτης> GetItemAsync(string id)
        {
            using (var uow = XpoHelper.CreateUnitOfWork())
            {
                return await uow.GetObjectByKeyAsync<Πελάτης>(id);
            }
        }
        public async Task<IEnumerable<Πελάτης>> GetItemsAsync()
        {
            using (var uow = XpoHelper.CreateUnitOfWork())
            {
                return await uow.Query<Πελάτης>().OrderBy(i => i.Επωνυμία).ToListAsync();
            }
        }
    }
}
