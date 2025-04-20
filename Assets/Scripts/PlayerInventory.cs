using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory
{
    private static Dictionary<string, int> collectedItems = new Dictionary<string, int>();

    public static void AddItem(string itemName)
    {
        if (collectedItems.ContainsKey(itemName))
        {
            collectedItems[itemName]++;  // Increase count if already exists
        }
        else
        {
            collectedItems[itemName] = 1;  // Add new item
        }

        Debug.Log("Added: " + itemName + " | Quantity: " + collectedItems[itemName]);
    }

    public static bool HasItem(string itemName)
    {
        return collectedItems.ContainsKey(itemName) && collectedItems[itemName] > 0;
    }

    public static void RemoveItem(string itemName)
    {
        if (HasItem(itemName))
        {
            collectedItems[itemName]--;

            if (collectedItems[itemName] <= 0)
            {
                collectedItems.Remove(itemName);  // Remove if quantity is 0
            }

            Debug.Log("Removed: " + itemName);
        }
    }

    public Dictionary<string, int> GetInventoryItems()
    {
        return collectedItems;
    }
}
