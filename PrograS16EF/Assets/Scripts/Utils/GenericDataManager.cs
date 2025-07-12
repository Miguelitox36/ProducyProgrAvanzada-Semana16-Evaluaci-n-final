using System.Collections.Generic;
using System.Linq;

public class GenericDataManager<T> where T : Product
{
    private List<T> items = new List<T>();

    public void AddItem(T item)
    {
        items.Add(item);
    }

    public List<T> GetItemsByCategory(string category)
    {
        return items.Where(item => item.Category == category).ToList();
    }

    public T GetMostExpensive()
    {
        return items.OrderByDescending(item => item.Price).FirstOrDefault();
    }

    public int GetCount()
    {
        return items.Count;
    }

    public void ClearItems()
    {
        items.Clear();
    }
}