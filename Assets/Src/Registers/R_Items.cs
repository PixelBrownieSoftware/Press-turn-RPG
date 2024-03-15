using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Registers/Items")]
public class R_Items : R_Default
{
    public Dictionary<S_Move, int> inventory = new Dictionary<S_Move, int>();
    private void OnDisable()
    {
        inventory.Clear();
    }

    public Tuple<S_Move, int> GetItem(S_Move item) {
        return new Tuple<S_Move, int>(item, inventory[item]);
    }

    public List<S_Move> GetItems() {
        List<S_Move> items = new List<S_Move>();
        foreach (var item in inventory) {
            items.Add(item.Key);
        }
        return items;
    }

    public void AddItem(S_Move item, int amount)
    {
        if (inventory.ContainsKey(item))
        {
            inventory[item]+= amount;
        }
        else
        {
            inventory.Add(item, amount);
        }
    }

    public void AddItem(S_Move item) {
        if (inventory.ContainsKey(item))
        {
            inventory[item]++;
        }
        else
        {
            inventory.Add(item, 1);
        }
    }

    public void RemoveItem(S_Move item)
    {
        if(inventory[item] > 0)
            inventory[item]--;
    }
}
