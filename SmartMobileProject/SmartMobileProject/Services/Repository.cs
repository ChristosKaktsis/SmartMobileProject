using DevExpress.Xpo;
using SmartMobileProject.Core;
using SmartMobileProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartMobileProject.Services
{
    class Repository : IDataCommand<Appointment>
    {
        public async Task<bool> AddItemAsync(Appointment item)
        {
            try
            {
                using (var uow = XpoHelper.CreateUnitOfWork())
                {
                    uow.Save(item);
                    await uow.CommitChangesAsync();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteItemAsync(int id)
        {
            try
            {
                using (var uow = XpoHelper.CreateUnitOfWork())
                {
                    var itemToDelete = uow.GetObjectByKey<Appointment>(id);
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

        public Task<Appointment> GetItemAsync(int id)
        {
            using (var uow = XpoHelper.CreateUnitOfWork())
            {
                return uow.GetObjectByKeyAsync<Appointment>(id);
            }
        }

        public async Task<IEnumerable<Appointment>> GetItemsAsync(bool forceRefresh = false)
        {
            using (var uow = XpoHelper.CreateUnitOfWork())
            {
                return await uow.Query<Appointment>().OrderBy(i => i.Subject).ToListAsync();
            }
        }

        public async Task<bool> UpdateItemAsync(Appointment item)
        {
            try
            {
                using (var uow = XpoHelper.CreateUnitOfWork())
                {
                    var itemToUpdate = await uow.GetObjectByKeyAsync<Appointment>(item.Oid);
                    if (itemToUpdate == null)
                    {
                        return false;
                    }
                    itemToUpdate.AllDay = item.AllDay;
                    itemToUpdate.LabelId = item.LabelId;
                    itemToUpdate.StartTime = item.StartTime;
                    itemToUpdate.EndTime = item.EndTime;
                    itemToUpdate.Subject = item.Subject;
                    itemToUpdate.Type = item.Type;
                    uow.Save(itemToUpdate);
                    await uow.CommitChangesAsync();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
