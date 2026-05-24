using UnityEngine;

[CreateAssetMenu(fileName = "Item Spawn Agent", menuName = "Inventory/Consumables/Spawn Agent")]
public class SOInventoryItemAgent : SOInventoryItemConsum
{
    public CombatAgent AgentReference;

    public override void ItemEffect()
    {
        base.ItemEffect();

        ManagerGameElements.Instance.CombatReference.Encounter.SpawnEnemy(AgentReference, ManagerGameElements.Instance.CombatReference.ArenaBounds.center + Vector3.down);
    }
    
}
