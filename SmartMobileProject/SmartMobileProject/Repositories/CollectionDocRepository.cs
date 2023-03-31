using DevExpress.Xpo;
using SmartMobileProject.Core;
using SmartMobileProject.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartMobileProject.Repositories
{
    public class CollectionDocRepository : IDatabase<ΠαραστατικάΕισπράξεων>
    {
        public async Task<bool> AddItemAsync(ΠαραστατικάΕισπράξεων item)
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
                    var itemToDelete = uow.Query<ΠαραστατικάΕισπράξεων>()
                        .Where(i => i.SmartOid.ToString() == id).FirstOrDefault();
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
        private bool IsOrderLast(ΠαραστατικάΕισπράξεων order)
        {
            return order.Σειρά.Counter == order.OrderSeiraCounter();
        }
        public async Task<ΠαραστατικάΕισπράξεων> GetItemAsync(string id)
        {
            using (var uow = XpoHelper.CreateUnitOfWork())
            {
                return await uow.GetObjectByKeyAsync<ΠαραστατικάΕισπράξεων>(id);
            }
        }

        public async Task<IEnumerable<ΠαραστατικάΕισπράξεων>> GetItemsAsync()
        {
            using (var uow = XpoHelper.CreateUnitOfWork())
            {
                return await uow.Query<ΠαραστατικάΕισπράξεων>().OrderByDescending(i => i.Ημνία).ToListAsync();
            }
        }

        public Task<bool> UpdateItemAsync(ΠαραστατικάΕισπράξεων item)
        {
            throw new NotImplementedException();
        }
    }
}
