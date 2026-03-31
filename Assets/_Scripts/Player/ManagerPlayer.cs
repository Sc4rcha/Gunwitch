using UnityEngine;
using GameInfo;

public class ManagerPlayer : MonoBehaviour
{
    public PlayerInfo Info;

    public InventoryMenuOverworld InventoryMenu;
    public Crafting Crafting;
    public PlayerHUD HUD;

    public void Setup() 
    {
        // setup elements
        InventoryMenu.Setup(Info.Inventory);
        Crafting.Setup(this);
        HUD.Setup();
    }

    public void EventStart() 
    {
        // lock inventory
        InventoryMenu.Open(false);
        InventoryMenu.Lock(true);
        // lock crafting
        Crafting.Lock(true);
    }
    public void EventFinish()
    {
        // unlock inventory
        InventoryMenu.Lock(false);
        // lock crafting
        Crafting.Lock(false);
    }

    public void CombatFinish(CombatActor playerCombatActor) 
    {
        // refresh player HUD to reflect combat end state
        Info.Stats = playerCombatActor.Stats;
        HUD.Refresh(Info.Stats);
    }
}

