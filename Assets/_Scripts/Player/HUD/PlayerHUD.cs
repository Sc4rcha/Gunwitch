using GameInfo;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    [Header("References")]
    public Image PlayerPortrait;
    public Image HealthBar;
    public Image ManaBar;
    public Button CraftingButton;
    [Space]
    public PlayerPortraitAim PortraitAim;
    public PlayerPortraitReload PortraitReload;
    public GameObject PortraitShoot;

    [Header("Variables")]
    public Color FocusOffColor;

    private enum PortraitState {Normal, Aim, Reload}
    private PortraitState portraitState;


    public void Setup()
    {
        // setup special portraits
        PortraitAim.Setup();

        PlayerPortrait.gameObject.SetActive(true);

        // hide special protraits
        PortraitAim.gameObject.SetActive(false);
        PortraitReload.gameObject.SetActive(false);
    }

    // refresh player status
    public void Refresh(Actor stats)
    {
        HealthBar.fillAmount = (float)stats.HealthCurrent / (float)stats.Health;
        ManaBar.fillAmount = (float)stats.ManaCurrent / (float)stats.Mana;
    }

    // focus player for dialogue
    public void SetFocusPlayer(bool isFocusOn)
    {
        if (isFocusOn)
            PlayerPortrait.color = Color.white;
        else
            PlayerPortrait.color = FocusOffColor;
    }


    #region Portrait actions
    public void Aim() 
    {
        ChangePortraitType(PortraitState.Aim);
    }
    public void Shoot(float shootTime) 
    {
        if (portraitState == PortraitState.Aim)
            StartCoroutine(ShootDelay(shootTime));
    }
    public void Reload(Bullet defaultBullet) 
    {
        ChangePortraitType(PortraitState.Reload);
        PortraitReload.ReloadStart(defaultBullet);
    }
    public void ReloadSelectBullet(Bullet bullet) 
    {
        PortraitReload.FocusBullet(bullet);
    }
    public void ReloadLoadBullet(Bullet bullet)
    {
        PortraitReload.LoadBullet(bullet);
    }
    #endregion


    private void ChangePortraitType (PortraitState newState)
    {
        switch (portraitState)
        {
            case PortraitState.Normal:
                PlayerPortrait.gameObject.SetActive(false);
                break;
            case PortraitState.Aim:
                PortraitAim.gameObject.SetActive(false);
                break;
            case PortraitState.Reload:
                PortraitReload.gameObject.SetActive(false);
                break;
        }

        switch (newState)
        {
            case PortraitState.Normal:
                PlayerPortrait.gameObject.SetActive(true);
                break;
            case PortraitState.Aim:
                PortraitAim.gameObject.SetActive(true);
                break;
            case PortraitState.Reload:
                PortraitReload.gameObject.SetActive(true);
                break;
        }

        portraitState = newState;
    }

    private IEnumerator ShootDelay(float delayTime) 
    {
        // hide aim and show shoot
        PortraitAim.gameObject.SetActive(false);
        PortraitShoot.SetActive(true);
        // wait gun cooldown
        yield return new WaitForSeconds(delayTime);
        // show aim hide shoot
        PortraitAim.gameObject.SetActive(true);
        PortraitShoot.SetActive(false);
    }
}
