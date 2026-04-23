using UnityEngine;


[CreateAssetMenu(fileName = "Item", menuName = "Inventory/Item Event")]
public class SOInventoryItemEvent : SOInventoryItem
{
    public SOEvent Event;

    private void OnValidate()
    {
        // force type to Consumable
        Type = GameInfo.ItemType.KEY;
    }

    public override void ItemEffect()
    {
        base.ItemEffect();

        AddEvent();

        if (ManagerGameElements.Instance.ManagerEvents.CheckEventState(Event) is GameInfo.EventState state)
        {
            if (!state.IsComplete)
                ManagerGameElements.Instance.ManagerEvents.EventStart(Event);
        }    
    }

    public override bool IsItemUsable()
    {
        AddEvent();

        return !ManagerGameElements.Instance.ManagerEvents.CheckEventState(Event).IsComplete;
    }

    private void AddEvent() 
    {
        ManagerGameElements.Instance.ManagerEvents.EventAdd(Event);
    }
}
