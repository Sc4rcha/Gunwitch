using UnityEngine;
using UnityEngine.UI;
using System;

public class CombatScreenAttack : MonoBehaviour
{
    public Image AttackImage;
    public TMPro.TMP_Text AttackName;

    public float AttackTime;
    private float attackTimeCurrent;

    public event Action OnAttackAnimationEnd;

    public void Attack(Sprite attackSprite) 
    {
        AttackImage.sprite = attackSprite;

        attackTimeCurrent = 0;
        gameObject.SetActive(true);
    }

    private void Update()
    {
        attackTimeCurrent += Time.deltaTime;
        if (attackTimeCurrent > AttackTime)
        {
            OnAttackAnimationEnd?.Invoke();
            gameObject.SetActive(false);
        }
    }
}
