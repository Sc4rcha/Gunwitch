using GameInfo;
using System.Linq;

public class PlayerPortraitReload : PlayerPortraitItemSelection
{

    public void ReloadStart(Bullet defaultBullet) 
    {
        base.SelectionStart();

        // Get bullets count = bullets in inventory + 1 for the defalt bullet
        itemsCount = ManagerGameElements.Instance.Player.Info.Inventory.Bullets.Count + 1;

        // show default bullet in inventory
        Items[0].gameObject.SetActive(true);
        Items[0].SetItemInfo(defaultBullet);
        // show consums in inventory
        for (int i = 1; i < itemsCount; i++)
        {
            Items[i].gameObject.SetActive(true);
            Items[i].SetItemInfo(ManagerGameElements.Instance.Player.Info.Inventory.Bullets.ElementAt(i - 1).Value);
        }

        // get angle step
        itemsAngleStep = 360f / itemsCount;
    }
}
