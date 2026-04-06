using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LoadDefaultInfo : MonoBehaviour, IPointerEnterHandler
{
    public Image ButtonSprite;
    public GameInfo.Bullet bullet;

    private Button button;

    public void Setup(GameInfo.Bullet bullet) 
    {
        this.bullet = bullet;

        // setup button color
        ButtonSprite.color = bullet.BulletColor;

        // get button reference
        button = GetComponent<Button>();
    }


    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        // if button is interactable select bullet
        if (button.interactable)
            PlayerHUDPortrait.Instance.ReloadFocus(bullet.Id);
    }
}
