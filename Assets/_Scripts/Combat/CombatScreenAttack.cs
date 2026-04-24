using UnityEngine;
using UnityEngine.UI;
using System;

public class CombatScreenAttack : MonoBehaviour
{
    public Image AttackImage;
    public TMPro.TMP_Text AttackName;

    public float AttackAnimationTime;
    private float attackTimeCurrent;

    public event Action OnAnimationFinish;

    public void Attack(Sprite attackSprite, SOAbility ability) 
    {
        AttackImage.sprite = attackSprite;
        AttackName.text = ability.Name;

        attackTimeCurrent = 0;

        gameObject.SetActive(true);
    }

    private void Update()
    {
        attackTimeCurrent += Time.deltaTime;
        if (attackTimeCurrent > AttackAnimationTime)
        {
            OnAnimationFinish?.Invoke();
            gameObject.SetActive(false);
        }
    }
}
