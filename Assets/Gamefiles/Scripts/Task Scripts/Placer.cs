using UnityEngine;
using UnityEngine.Events;

public class Placer : MonoBehaviour
{
    public bool removeHeldItem;
    public ItemHolder.ItemType lookingForHeldItem;
    public UnityEvent onPlace;
    public void TryPlace()
    {
        if (!ItemHolder.IsHoldingItem(lookingForHeldItem)) return;
        onPlace?.Invoke();
        if(removeHeldItem) ItemHolder.Instance.StopHoldingCurrentItem();
    }
}
