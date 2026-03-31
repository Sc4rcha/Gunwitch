using GameInfo;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    [Header ("References")]
    public Image PlayerPortrait;
    public Image HealthBar;
    public Image ManaBar;

    [Header ("Variables")]
    public Color FocusOffColor;


    public void Setup()
    {
        // add player focus to dialogue focus event
        ManagerGameElements.Instance.ManagerDialogue.OnPlayerFocus += SetFocusPlayer;
    }

    private void OnDestroy()
    {
        // remove player focus from dialogue focus event
        ManagerGameElements.Instance.ManagerDialogue.OnPlayerFocus -= SetFocusPlayer;
    }

    public void Refresh(ActorStats stats) 
    {
        HealthBar.fillAmount = (float)stats.HealthCurrent / (float)stats.Health;
        ManaBar.fillAmount = (float)stats.ManaCurrent / (float)stats.Mana;
    }
    public void SetFocusPlayer(bool isFocusOn)
    {
        if (isFocusOn)
            PlayerPortrait.color = Color.white;
        else
            PlayerPortrait.color = FocusOffColor;
    }
}
