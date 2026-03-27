using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Player/Item")]
public class SOInventoryItem : ScriptableObject
{
    public string Id;
    public string Name;
    public GameInfo.ItemType Type;

    public GameInfo.InventoryItem GetItem() 
    {
        return new GameInfo.InventoryItem(this);
    }
}
