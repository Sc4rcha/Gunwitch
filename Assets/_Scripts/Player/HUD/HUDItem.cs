using UnityEngine;
using UnityEngine.UI;

public class HUDItem : MonoBehaviour
{
    public string ItemId;
    public RectTransform ItemRectTransform;
    public Image ItemImage;
    public Animator Animator;

    public virtual void SetItemInfo(GameInfo.InventoryItem item)
    {
        ItemId = item.Id;
        ItemImage.sprite = ManagerGameElements.Instance.ItemReferences.GetItemReference(item.Id).Sprite;

        Animator.Play("Idle");
    }

    public virtual void Use()
    {
        Animator.Play("Use");
    }
}
