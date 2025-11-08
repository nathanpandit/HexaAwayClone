using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class InventoryHelper : Singleton<InventoryHelper>
{
    private Dictionary<InventoryType, List<Action<int>>> listeners = new Dictionary<InventoryType, List<Action<int>>>();

    private List<InventoryDataItem> _inventoryData;

    protected override void Awake()
    {
        base.Awake();
        // Subscribe to game events
        EventManager.Instance().HexFinished += OnHexFinished;
        EventManager.Instance().LevelWon += OnLevelWon;
    }

    void OnDestroy()
    {
        // Unsubscribe from events
        if (EventManager.Instance() != null)
        {
            EventManager.Instance().HexFinished -= OnHexFinished;
            EventManager.Instance().LevelWon -= OnLevelWon;
        }
    }

    private void OnHexFinished()
    {
        // Increase hex count when a hex finishes successfully
        AddItem(InventoryType.Hex, 1);
        Debug.Log($"Number of hexes: {GetQuantity(InventoryType.Hex)}");
    }

    private void OnLevelWon()
    {
        // Increase trophy count when a level is passed
        AddItem(InventoryType.Trophy, 1);
        Debug.Log($"Number of trophies: {GetQuantity(InventoryType.Trophy)}");
    }

    public List<InventoryDataItem> inventoryData
    {
        get
        {
            if (_inventoryData == null)
            {
                _inventoryData = Resources.Load<InventoryScriptableObjectScript>("Inventory").inventoryDataItems;
            }
            return _inventoryData;
        }
    }


    public int GetQuantity(InventoryType itemType)
    {
        InventoryDataItem item = inventoryData.FirstOrDefault(data => data.itemType == itemType);
        if (item != null)
        {
            return item.quantity;
        }
        Debug.Log("Item is null, returning 0");
        return 0;
    }

    public void SetQuantity(InventoryType itemType, int quantityToSet)
    {
        InventoryDataItem item = inventoryData.FirstOrDefault(data => data.itemType == itemType);
        if (item != null)
        {
            SetQuantity(item, quantityToSet);
        }
        else
        {
            InventoryDataItem newItem = new InventoryDataItem();
            newItem.itemType = itemType;
            newItem.quantity = quantityToSet;
            inventoryData.Add(newItem);
            listeners[itemType] = new List<Action<int>>();
        }
    }

    // use only when inventoryData is already pulled
    public void SetQuantity(InventoryDataItem item, int quantityToSet)
    {
        item.quantity = quantityToSet;
        // Trigger(item.itemType, item.quantity);
    }
    
    public void AddItem(InventoryType itemType, int quantityToAdd)
    {
        InventoryDataItem item = inventoryData.FirstOrDefault(data => data.itemType == itemType);
        
        if (item != null)
        {
            SetQuantity(item, item.quantity + quantityToAdd);
            Trigger(itemType, item.quantity);
        }
        else
        {
            InventoryDataItem newItem = new InventoryDataItem();
            newItem.itemType = itemType;
            newItem.quantity = quantityToAdd;
            inventoryData.Add(newItem);
            listeners[itemType] = new List<Action<int>>();
            Trigger(itemType, newItem.quantity);
        }
    }

    public void RemoveItem(InventoryType itemType, int quantityToRemove)
    {
        InventoryDataItem item = inventoryData.FirstOrDefault(data => data.itemType == itemType);
            
            if (item != null)
            {
                RemoveItem(item, quantityToRemove);
            }
            else
            {
                Debug.LogWarning("don't have this item");
            }
    }

    //only use when inventoryData is already pulled
    public void RemoveItem(InventoryDataItem item, int quantityToRemove)
    {
        item.quantity -= quantityToRemove;
        Trigger(item.itemType, item.quantity);
    }

    public void ResetOnLost(InventoryType itemType)
    {
        InventoryDataItem item = inventoryData.FirstOrDefault(item => item.itemType == itemType);

        if (item != null)
        {
            SetQuantity(item, item.quantity);
        }
    }

    public bool TrySpend(InventoryType itemType, int quantityToSpend)
    {
        InventoryDataItem item = inventoryData.FirstOrDefault(data => data.itemType == itemType);

        if (item != null)
        {
            if (item.quantity >= quantityToSpend)
            {
                RemoveItem(item, quantityToSpend);
                return true;
            }
        }
        Debug.Log("Not enough items");
        return false;
    }
    
    public void AddListener(InventoryType itemType, Action<int> listener)
    {
        if (!listeners.ContainsKey(itemType))
        {
            listeners[itemType] = new List<Action<int>>();
        }
        listeners[itemType].Add(listener);
    }
    
    public void RemoveListener(InventoryType itemType, Action<int> listener)
    {
        if (listeners.ContainsKey(itemType))
        {
            listeners[itemType].Remove(listener);
            if (listeners[itemType].Count == 0)
            {
                listeners.Remove(itemType);
            }
        }
    }

    public void Trigger(InventoryType itemType, int quantity)
    {
        // Use TryGetValue to safely check if the key exists before accessing
        if (listeners.TryGetValue(itemType, out var _listeners) && _listeners != null)
        {
            foreach(Action<int> listener in _listeners)
            {
                listener.Invoke(quantity);
            }
        }
    }

}