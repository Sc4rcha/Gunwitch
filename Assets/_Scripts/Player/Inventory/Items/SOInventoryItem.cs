using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Inventory/Item")]
public class SOInventoryItem : ScriptableObject
{
    public string Id;
    public string Name;
    public GameInfo.ItemType Type;
    [Space]
    [TextArea]
    public string Description;
    public Sprite Sprite;

    public GameInfo.InventoryItem GetItem() 
    {
        return new GameInfo.InventoryItem(this);
    }

    public virtual void ItemEffect()
    {
        Debug.Log("Item Effect " + Name);
    }
    public virtual bool IsItemUsable() 
    {
        return false;
    }
}
