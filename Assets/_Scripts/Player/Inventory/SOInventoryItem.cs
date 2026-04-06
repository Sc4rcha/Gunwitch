using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Player/Item")]
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

    public void ItemEffect()
    {
        Debug.Log("Item Effect " + Name);
    }
}
