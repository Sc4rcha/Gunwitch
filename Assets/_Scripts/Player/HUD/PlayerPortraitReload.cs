using System.Linq;
using UnityEngine;

public class PlayerPortraitReload : PlayerPortraitItemSelection
{

    public void ReloadStart() 
    {
        base.SelectionStart();

        // Get bullets count = bullets in inventory + 1 for the defalt bullet
        itemsCount = ManagerGameElements.Instance.Player.Actor.Inventory.Bullets.Count;

        // show default bullet in inventory
        // show consums in inventory
        for (int i = 0; i < itemsCount; i++)
        {
            Items[i].gameObject.SetActive(true);
            Items[i].SetItemInfo(ManagerGameElements.Instance.Player.Actor.Inventory.Bullets.ElementAt(i).Value);
        }

        // get angle step
        itemsAngleStep = 360f / itemsCount;
    }
}
