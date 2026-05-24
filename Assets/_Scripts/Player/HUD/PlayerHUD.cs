using GameInfo;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    [Header("References")]
    public Image HealthBar;
    public Image ManaBar;
    public Button CraftingButton;
    public PlayerHUDPortrait Portrait;
    public PlayerPortraitShake Shake;

    private ManagerPlayer player;

    private Coroutine damageCoroutine;
    private WaitForSeconds damageMessageDelay;

    public void Setup(ManagerPlayer player)
    {
        this.player = player;

        Portrait.Setup();

        damageMessageDelay = new WaitForSeconds(0.75f);
        Shake.enabled = false;
    }

    // refresh player status
    public void Refresh()
    {
        
        HealthBar.fillAmount = (float)player.Actor.HealthCurrent / (float)player.Actor.Health;
        ManaBar.fillAmount = (float)player.Actor.ManaCurrent / (float)player.Actor.Mana;
    }

    public void Damage(string value) 
    {
        if (damageCoroutine != null)
            StopCoroutine(damageCoroutine);

        damageCoroutine = StartCoroutine(DamageAnimation(value));
    }
    private IEnumerator DamageAnimation(string value) 
    {
        yield return damageMessageDelay;

        if (value != "MISS")
        {
            Shake.ShakeStart(3);
            Portrait.CombatDamage();
        }

        Portrait.HitNumber.ShowNumber(value, 3, CombatHitMessage.MessageType.Damage);
        Refresh();


        damageCoroutine = null;
    }

    public void ShowHitMessage(string text)
    {
        Portrait.HitNumber.ShowNumber(text, 3, CombatHitMessage.MessageType.Text);
    }

    public void Heal(string value) 
    {
        Portrait.HitNumber.ShowNumber(value, 3, CombatHitMessage.MessageType.Heal);
        Refresh();
    }
}
