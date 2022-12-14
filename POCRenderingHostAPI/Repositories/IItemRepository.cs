using POCRenderingHostAPI.Models;

namespace POCRenderingHostAPI.Repositories;

public interface IItemRepository
{
    Task AddItem(string id, string name);
    Task AddItem(Item item);
    List<Item?> GetItems();
    Task<Item?> GetItemById(string id);
    Task<Item?> GetItemByName(string name);
    Task RemoveItem(Item? itemToRemove);
}