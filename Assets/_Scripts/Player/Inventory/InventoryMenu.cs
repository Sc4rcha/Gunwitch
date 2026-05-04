using GameInfo;
using UnityEngine;

public class InventoryMenu : MonoBehaviour
{
    protected ActorPlayer player;
    protected InventorySlotButtonInfo[] inventorySlots;
    protected ItemType selectedSection;

    public virtual void Setup(ActorPlayer player) 
    {
        this.player = player;
    }

    #region BUTTONS
    public void ButtonOpenIngredients() => ShowSection(ItemType.INGREDIENT);
    public void ButtonOpenConsums() => ShowSection(ItemType.CONSUMABLE);
    public void ButtonOpenBullets() => ShowSection(ItemType.BULLET);
    public void ButtonOpenDrums() => ShowSection(ItemType.DRUM);
    public void ButtonOpenKeyItems() => ShowSection(ItemType.KEY);
    #endregion
    
    /// <summary>
    /// locks entire inventory menu
    /// </summary>
    /// <param name="isLock"></param>
    public virtual void Lock(bool isLock)
    {
        // lock/unlock inventory slots
        foreach (var slot in inventorySlots)
            slot.SlotButton.interactable = !isLock;
    }
    /// <summary>
    /// Lock section button for a given section
    /// </summary>
    /// <param name="section"></param>
    public virtual void LockSection(ItemType section, bool isLock) 
    {
        // lock/unlock inventory slots if section is selected section
        if (selectedSection == section)
        {
            foreach (var slot in inventorySlots)
                slot.SlotButton.interactable = !isLock;
        }
    }
    /// <summary>
    /// Lock inventory item buttons
    /// </summary>
    /// <param name="isLock"></param>
    public void LockItemButtons(bool isLock) 
    {
        foreach (var slot in inventorySlots)
            slot.SlotButton.interactable = !isLock;
    }

    public virtual void ShowSection(ItemType section)
    {
        selectedSection = section;
    }
    public void Refresh() 
    {
        ShowSection(selectedSection);
    }

    public virtual void ItemUse(InventoryItem item) { }
    public virtual void ItemSelect(InventoryItem item) { }
    public virtual void ItemDelesect() { }
}
