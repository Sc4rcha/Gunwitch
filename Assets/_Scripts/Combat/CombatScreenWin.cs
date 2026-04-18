using GameInfo;
using HutongGames.PlayMaker.Actions;
using System.Collections.Generic;
using UnityEngine;

public class CombatScreenWin : MonoBehaviour
{
    public TMPro.TMP_Text[] LootDisplay;

    private List<LootItem> lootTable;
    private Dictionary<SOInventoryItem,int> loot;

    private ManagerPlayer player;
    private int playerLuck;

    public void Setup() 
    {
        loot = new Dictionary<SOInventoryItem, int>();
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

        int i = 0;
        foreach (var lootSlot in loot)
        {
            LootDisplay[i].gameObject.SetActive(true);
            LootDisplay[i].text = lootSlot.Key.Name + " x" + lootSlot.Value;
            i++;
        }

        for (; i < LootDisplay.Length; i++)
        {
            LootDisplay[i].gameObject.SetActive(false);
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
                    if (loot.ContainsKey(lootItem.Loot))
                        loot[lootItem.Loot] += 1;
                    else
                        loot.Add(lootItem.Loot, 1);
                    break;
                }
            }
        }

        // add loot to player
        foreach (var lootSlot in loot)
        {
            for (int i = 0; i < lootSlot.Value; i++)
                player.Info.Inventory.AddItem(lootSlot.Key.GetItem());
        }
    }
}
