using UnityEngine;
using GameInfo;
using UnityEngine.UI;

public class InventorySlotButtonInfo : MonoBehaviour
{
    public TMPro.TMP_Text ItemName;
    public TMPro.TMP_Text ItemNumber;
    public Image ImageBack;

    public Button SlotButton { get; private set; }

    protected InventoryItem itemInfo;
    protected InventoryMenu inventoryMenu;

    public void Setup(InventoryMenu inventoryMenu) 
    {
        // setup inventory reference
        this.inventoryMenu = inventoryMenu;
        SlotButton = GetComponent<Button>();

        // setup button
        GetComponent<InventorySlotButton>().Setup(this);

        // hide button
        Show(false);
    }

    public void SetItem(InventoryItem itemInfo) 
    {
        // set intem information
        this.itemInfo = itemInfo;

        // setup button
        ItemName.text = this.itemInfo.Name;
        ItemNumber.gameObject.SetActive(itemInfo.Type == ItemType.INGREDIENT || itemInfo.Type == ItemType.BULLET);
        ItemNumber.text = itemInfo.Quantity.ToString();
        ImageBack.color = Color.white;

        // extra setup steps for bullets
        if (itemInfo is Bullet bullet)
        {
            ItemNumber.text = bullet.ManaCost.ToString();
            ImageBack.color = bullet.BulletColor;
        }
    }

    public void Show(bool isShow) 
    {
        gameObject.SetActive(isShow);
    }

    public virtual void ButtonEnter() 
    {

    }
    public virtual void ButtonExit() 
    {

    }
    public virtual void ButtonInteract() 
    {
        inventoryMenu.ItemSelect(itemInfo);
    }
}
