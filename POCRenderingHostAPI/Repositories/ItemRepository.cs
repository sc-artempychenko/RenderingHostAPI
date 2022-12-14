using Microsoft.EntityFrameworkCore;
using POCRenderingHostAPI.Data;
using POCRenderingHostAPI.Models;

namespace POCRenderingHostAPI.Repositories
{
    public class ItemRepository : IItemRepository
    {
        private readonly DataContext _dataContext;

        public ItemRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task AddItem(string id, string name)
        {
            await _dataContext.Items.AddAsync(new Item()
            {
                Id = id,
                Name = name
            });

            await _dataContext.SaveChangesAsync();
        }

        public async Task AddItem(Item item)
        {
            await _dataContext.Items.AddAsync(item);

            await _dataContext.SaveChangesAsync();
        }

        public List<Item?> GetItems()
        {
            var items = new List<Item?>();

            items.AddRange(_dataContext.Items);

            return items;
        }

        public async Task<Item?> GetItemById(string id)
        {
            return await _dataContext.Items.FirstOrDefaultAsync(item => item.Id == id);
        }

        public async Task<Item?> GetItemByName(string name)
        {
            return await _dataContext.Items.FirstOrDefaultAsync(item => item.Name == name);
        }

        public async Task RemoveItem(Item? itemToRemove)
        {
            if (itemToRemove != null)
            {
                _dataContext.Items.Remove(itemToRemove);
            }
            
            await _dataContext.SaveChangesAsync();
        }
    }
}
