using GameInfo;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPortraitItemSelection : MonoBehaviour
{
    [Header("References Scene")]
    public Transform ParentFront;
    public Transform ParentBack;
    public List<HUDItem> Items;
    public Animator Animator;
    [Header("Circle")]
    public RectTransform ItemCircleCentre;
    public float ItemCircleRadius = 200f;
    public float ItemCircleElipseStrength;

    // consum circle variables
    private float rotationSpeed = 5f;
    private float focusAngle = 300f;
    private float rotationOffset = 0f;
    private float targetOffset;

    // consum positioning
    protected int itemsCount;
    protected float itemsAngleStep;
    private float itemAngle;
    private float itemPosX;
    private float itemPosY;

    public virtual void SelectionStart() 
    {
        // hide all items
        foreach (var item in Items)
            item.gameObject.SetActive(false);
    }

    public virtual void ItemFocus(string itemId)
    {
        // get item angle to get focused
        float itemBaseAngle = Items.FindIndex(x => x.ItemId == itemId) * itemsAngleStep;

        // set target focus to item
        targetOffset = NormalizeAngle(focusAngle - itemBaseAngle);
    }
    public virtual void ItemUse(string itemId)
    {
        // get item index
        int itemIndex = Items.FindIndex(x => x.ItemId == itemId);

        // set circle angle to match selected item
        targetOffset = NormalizeAngle(focusAngle - itemIndex * itemsAngleStep);
        rotationOffset = targetOffset;

        // play use item animations
        Items[itemIndex].Use();
        Animator.Play("Use");
    }

    void Update()
    {
        // circle rotation animation
        rotationOffset = Mathf.LerpAngle(rotationOffset, targetOffset, Time.deltaTime * rotationSpeed);

        // update item positions
        Arrange();
    }



    void Arrange()
    {
        for (int i = 0; i < itemsCount; i++)
        {
            // get angle of item
            itemAngle = (i * itemsAngleStep + rotationOffset) * Mathf.Deg2Rad;

            // get position within circle for item angle
            itemPosX = Mathf.Cos(itemAngle) * ItemCircleRadius;
            itemPosY = Mathf.Sin(itemAngle) * ItemCircleRadius / ItemCircleElipseStrength;

            // place bullet in circle
            Items[i].ItemRectTransform.anchoredPosition = ItemCircleCentre.anchoredPosition + new Vector2(itemPosX, itemPosY);

            // Determine front/back depending on Y position
            if (itemPosY > 0)
            {
                // Behind character
                if (Items[i].ItemRectTransform.parent != ParentBack)
                    Items[i].ItemRectTransform.SetParent(ParentBack);

            }
            else
            {
                // In front of character
                if (Items[i].ItemRectTransform.parent != ParentFront)
                    Items[i].ItemRectTransform.SetParent(ParentFront);
            }
        }
    }


    /// <summary>
    /// returns an angle from 0 to 360
    /// </summary>
    /// <param name="angle"></param>
    /// <returns></returns>
    float NormalizeAngle(float angle)
    {
        angle %= 360f;
        if (angle < 0) angle += 360f;
        return angle;
    }
}
