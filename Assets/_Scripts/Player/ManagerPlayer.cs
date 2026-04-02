using UnityEngine;
using GameInfo;

public class ManagerPlayer : MonoBehaviour
{
    public static ManagerPlayer Instance;

    public PlayerInfo Info;

    public InventoryMenuOverworld InventoryMenu;
    public Crafting Crafting;
    public PlayerHUD HUD;

    public void Setup() 
    {
        // set singleton
        if (Instance == null)
            Instance = this;

        // setup elements
        InventoryMenu.Setup(Info);
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

    public void Damage(int value) 
    {
        Info.Actor.HealthChange(-value);
        HUD.Refresh(Info.Actor);
    }
    public void ManaUse(int value) 
    {
        Info.Actor.ManaChange(-value);
        HUD.Refresh(Info.Actor);
    }
    public void ManaRecover(int value)
    {
        Info.Actor.ManaChange(value);
        HUD.Refresh(Info.Actor);
    }
}

