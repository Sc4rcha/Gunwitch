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
        InventoryMenu.Setup(Info);
        Crafting.Setup(this);
        HUD.Setup();

        // add event finish method to event finish action
        ManagerGameElements.Instance.ManagerEvents.OnEnventFinish += EventFinish;
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
        HUD.Portrait.HitNumber.ShowNumber(value);

        Info.Actor.HealthChange(-value);
        HUD.Refresh(Info.Actor);
    }
    public void Heal (int value)
    {
        Info.Actor.HealthChange(value);
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

