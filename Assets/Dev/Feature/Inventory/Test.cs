using System.Collections;
using System.Collections.Generic;
using MyBox;
using UnityEngine;

public class Test : MonoBehaviour
{
    public Inventory TargetInventory;
    public CountableItemData ToAddItemData;

    [ButtonMethod]
    private void OnDebugToggleInventory()
    {
        if (Application.isPlaying == false) return;
        if (TargetInventory == null) return;
        
        if (TargetInventory.IsOpened())
        {
            TargetInventory.CloseInventory();
        }
        else
        {
            TargetInventory.OpenInventory();
        }
    }
    
    [ButtonMethod]
    private void OnAddItem()
    {
        if (Application.isPlaying == false) return;
        if (TargetInventory == null) return;
        
        TargetInventory.AddItem(new CountableItem(ToAddItemData, 1));
    }
}
