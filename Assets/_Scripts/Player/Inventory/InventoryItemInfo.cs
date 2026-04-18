using UnityEngine;
using UnityEngine.UI;

public class InventoryItemInfo : MonoBehaviour
{
    public GameObject ItemInfo;
    [Space]
    public Image ItemImage;
    public TMPro.TMP_Text ItemDescription;

    public virtual void InfoShow(SOInventoryItem item) 
    {
        ItemInfo.SetActive(true);

        ItemDescription.text = item.Description;
        ItemImage.sprite = item.Sprite;
    }

    public virtual void InfoHide() 
    {
        ItemInfo.gameObject.SetActive(false);
    }
}
