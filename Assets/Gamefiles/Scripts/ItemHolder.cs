using System;
using AYellowpaper.SerializedCollections;
using DesignPatterns.CreationalPatterns;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class ItemHolder : Singleton<ItemHolder>
{
    public enum ItemType
    {
        None,
        StackOfPapers,
        PrintedPapers,
        CoffeeMug,
        FilledCoffeMug,
        TrashBag
    }

    [Serializable]
    public class ItemData
    {
        public ItemType itemType;
        public bool progressCompleted = false;
        public int currentItemProgress;
        public int maxItemProgress = 1;
        public GameObject go_itemObj;
        public UnityEvent onItemProgressComplete;
    }

    public bool hasAnItem => holdingItem != null;
    public SerializedDictionary<ItemType, ItemData> items = new();
    [NonSerialized] public ItemData holdingItem = null;

    protected override void Awake()
    {
        foreach (ItemType itemType in Enum.GetValues(typeof(ItemType)))
        {
            if(itemType == ItemType.None) continue;
            items[itemType].go_itemObj?.SetActive(false);
        }

        holdingItem = null;
    }

    public static bool IsHoldingItem(ItemType itemType)
    {
        if(Instance.holdingItem == null) return false;
         return (Instance.holdingItem.itemType == itemType); 
    }
    
    [Button]
    public void InitializeNotInitializedAudioClipTypes()
    {
        foreach (ItemType itemType in Enum.GetValues(typeof(ItemType)))
        {
            if(itemType == ItemType.None) continue;
            items.Add(itemType, new ItemData()
            {
                itemType = itemType,
            });
        }
    }

    public void PickupItem(int itemEnumIndx) => PickupItem((ItemType)itemEnumIndx);
    [Button]
    public void PickupItem(ItemType itemType)
    {
        if(itemType == ItemType.None) Debug.LogError("Cannot pickup NONE item");
        if (hasAnItem) return;
        if(itemType == ItemType.None) return;
        items[itemType].go_itemObj?.SetActive(true);
        holdingItem = items[itemType];
    }

    [Button]
    public void ProgressCurrentItem()
    {
        if (holdingItem.progressCompleted) return;
        holdingItem.currentItemProgress++;
        if (holdingItem.currentItemProgress >= holdingItem.maxItemProgress)
        {
            holdingItem.onItemProgressComplete?.Invoke();
            holdingItem.progressCompleted = true;
        }
    }

    public void StopHoldingCurrentItem()
    {
        holdingItem.go_itemObj?.SetActive(false);
        holdingItem = null;
    }

    [Button]
    public void ResetItem(ItemType itemType)
    {
        
        var item = items[itemType];
        item.go_itemObj?.SetActive(false);
        item.currentItemProgress = 0;
        item.progressCompleted = false;
    }


}
