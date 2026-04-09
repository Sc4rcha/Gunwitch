using System.Collections.Generic;
using GameInfo;
using UnityEngine;

public class CombatScreenWin : MonoBehaviour
{
    public TMPro.TMP_Text[] LootDisplay;

    private List<LootItem> lootTable;
    private List<InventoryItem> loot;

    private ManagerPlayer player;
    private int playerLuck;

    public void Setup() 
    {
        loot = new List<InventoryItem>();
        lootTable = new List<LootItem>();

        // get player info
        player = ManagerGameElements.Instance.Player;
        playerLuck = player.Info.Actor.Luck;

        // hide all loot display items
        foreach (var itemDisplay in LootDisplay)
            itemDisplay.gameObject.SetActive(false);
    }
    public void AddLootItem(LootItem item)
    {
        lootTable.Add(item);
    }

    public void ScreenShow()
    {
        LootCalculate();

        for (int i = 0; i < loot.Count; i++)
        {
            LootDisplay[i].gameObject.SetActive(true);
            LootDisplay[i].text = loot[i].Name;
        }

        gameObject.SetActive(true);
    }

    private void LootCalculate()
    {
        foreach (var lootItem in lootTable)
        {
            for (int i = 0; i < playerLuck; i++)
            {
                if (Random.Range(0, 100) < lootItem.Chances)
                {
                    loot.Add(lootItem.Loot.GetItem());
                    break;
                }
            }
        }

        // add loot to player
        foreach (var item in loot)
            player.Info.Inventory.AddItem(item);
    }
}
