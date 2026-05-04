using UnityEngine;
using GameInfo;

public class ManagerPlayer : MonoBehaviour
{
    public ActorPlayer Info;

    public InventoryMenuOverworld InventoryMenu;
    public Crafting Crafting;
    public PlayerHUD HUD;

    public void Setup() 
    {
        // setup elements
        InventoryMenu.Setup(Info);
        Crafting.Setup(this);
        HUD.Setup(this);
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

    public void Miss() 
    {
        HUD.Damage("MISS");
    }
    public void Damage(int value) 
    {
        Info.HealthChange(-value);
        HUD.Damage(value.ToString());
    }
    public void Heal (int value)
    {
        Info.HealthChange(value);
        HUD.Refresh();
    }
    public void ManaUse(int value) 
    {
        Info.ManaChange(-value);
        HUD.Refresh();
    }
    public void ManaRecover(int value)
    {
        Info.ManaChange(value);
        HUD.Refresh();
    }
}

