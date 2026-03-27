using UnityEngine;
using static GameInfo;

public class InventorySlotButtonInfo : MonoBehaviour
{
    public TMPro.TMP_Text ItemName;

    private InventoryItem itemInfo;
    private Inventory inventory;

    public void Setup(Inventory inventory) 
    {
        // setup inventory reference
        this.inventory = inventory;
        // hide button
        Show(false);
    }

    public void SetItem(InventoryItem itemInfo) 
    {
        // set intem information
        this.itemInfo = itemInfo;

        // setup button
        ItemName.text = this.itemInfo.Name;
    }

    public void Show(bool isShow) 
    {
        gameObject.SetActive(isShow);
    }

    public void ButtonInteract() 
    {
        inventory.ShowItem(itemInfo);
    }
}
