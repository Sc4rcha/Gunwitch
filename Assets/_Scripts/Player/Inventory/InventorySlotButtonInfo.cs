using UnityEngine;
using GameInfo;
using UnityEngine.UI;

[RequireComponent (typeof (Button))]
public class InventorySlotButtonInfo : MonoBehaviour
{
    public TMPro.TMP_Text ItemName;

    public Button SlotButton { get; private set; }

    private InventoryItem itemInfo;
    private InventoryMenu inventoryMenu;

    public void Setup(InventoryMenu inventoryMenu) 
    {
        // setup inventory reference
        this.inventoryMenu = inventoryMenu;
        SlotButton = GetComponent<Button>();

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
        inventoryMenu.ShowItem(itemInfo);
    }
}
