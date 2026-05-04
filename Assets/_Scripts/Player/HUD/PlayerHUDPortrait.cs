using GameInfo;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUDPortrait : MonoBehaviour
{
    public static PlayerHUDPortrait Instance;

    public PlayerPortraitAim PortraitAim;
    public PlayerPortraitReload PortraitReload;
    public PlayerPortraitConsumable PortraitConsumable;
    public GameObject PortraitShoot;
    [Space]
    public Image PlayerPortrait;
    public Animator PortraitAnimator;
    [Space]
    public CombatHitMessage HitNumber;

    [Header("Variables")]
    public Color FocusOffColor;


    public void Setup() 
    {
        Instance = this;

        PortraitAim.Setup();
    }

    #region Dialogue
    // focus player for dialogue
    public void SetFocusPlayer(bool isFocusOn)
    {
        if (isFocusOn)
            PlayerPortrait.color = Color.white;
        else
            PlayerPortrait.color = FocusOffColor;
    }
    #endregion


    public void PortraitShow(bool isShow) 
    {
        PlayerPortrait.gameObject.SetActive(isShow);
    }


    #region Combat AIM
    public void GunAimStart() 
    {
        PortraitAim.gameObject.SetActive(true);
        PortraitShow(false);
        PortraitShoot.SetActive(false);
    }
    public void GunAimFinish() 
    {
        PortraitAim.gameObject.SetActive(false);
        PortraitShow(true);
        PortraitShoot.SetActive(false);
    }
    public void GunAimOnOff(bool isAim)
    {
        if (isAim)
            GunAimStart();
        else
            GunAimFinish();
    }
    public void GunShoot() 
    {
        PortraitShow(false);
        PortraitAim.gameObject.SetActive(false);
        PortraitShoot.SetActive(true);
    }
    #endregion

    #region Combat Reload
    public void ReloadStart()
    {
        // show reload and hide normal player portrait
        PortraitReload.gameObject.SetActive(true);
        PortraitReload.ReloadStart();
        PortraitShow(false);
    }
    public void ReloadFinish() 
    {
        // hide reaload and show normal player portrait
        PortraitReload.gameObject.SetActive(false);
        PortraitShow(true);
    }
    public void ReloadFocus(string bulletId)
    {
        PortraitReload.ItemFocus(bulletId);
    }
    public void ReloadLoad(string bulletId)
    {
        PortraitReload.ItemUse(bulletId);
    }
    #endregion

    #region Consumable
    public void ConsumableStart()
    {
        // show consumable and hide normal player portrait
        PortraitConsumable.gameObject.SetActive(true);
        PortraitConsumable.SelectionStart();
        PortraitShow(false);
    }
    public void ConsumableFinish() 
    {
        // hide consums and show normal player portrait
        PortraitConsumable.gameObject.SetActive(false);
        PortraitShow(true);
    }
    public void ConsumsFocus(string consumId)
    {
        PortraitConsumable.ItemFocus(consumId);
    }
    public void ConsumsSelect(string consumId)
    {
        PortraitConsumable.ItemUse(consumId);
    }
    public void ConsumsDeselect() 
    {
        PortraitConsumable.SelectionStart();
    }
    #endregion
}
