using UnityEngine;
using UnityEngine.UI;
using System;

public class CombatScreenAttack : MonoBehaviour
{
    public Image AttackImage;
    public TMPro.TMP_Text AttackName;

    public float AttackAnimationTime;
    private float attackTimeCurrent;

    public event Action OnAnimationEnd;

    public void Attack(Sprite attackSprite) 
    {
        AttackImage.sprite = attackSprite;

        attackTimeCurrent = 0;

        gameObject.SetActive(true);
    }

    private void Update()
    {
        attackTimeCurrent += Time.deltaTime;
        if (attackTimeCurrent > AttackAnimationTime)
        {
            OnAnimationEnd?.Invoke();
            gameObject.SetActive(false);
        }
    }
}
