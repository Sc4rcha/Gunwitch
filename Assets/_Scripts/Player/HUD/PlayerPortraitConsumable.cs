using System.Linq;
using UnityEngine;

public class PlayerPortraitConsumable : PlayerPortraitItemSelection
{
    public override void SelectionStart()
    {
        base.SelectionStart();

        // Get consumables count = consumables  in inventory
        itemsCount = ManagerGameElements.Instance.Player.Info.Inventory.Consumables.Count;

        // show consums in inventory
        for (int i = 0; i < itemsCount; i++)
        {
            Items[i].gameObject.SetActive(true);
            Items[i].SetItemInfo(ManagerGameElements.Instance.Player.Info.Inventory.Consumables.ElementAt(i));
        }

        // get angle step
        itemsAngleStep = 360f / itemsCount;

        Animator.Play("Idle");
    }

}
