using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class CombatScreenAttack : MonoBehaviour
{
    public TMPro.TMP_Text AttackName;

    public event Action OnAnimationFinish;

    private WaitForSeconds preActWait;
    private WaitForSeconds postActWait;
    private ManagerCombat manager;


    private List<CombatAgent> enemiesSelected;


    public void Setup(ManagerCombat manager) 
    {
        this.manager = manager;


        enemiesSelected = new List<CombatAgent>();


        preActWait = new WaitForSeconds(0.5f);
        postActWait = new WaitForSeconds(1);

        gameObject.SetActive(false);
    }

    public void Attack(CombatAgent enemyActing, SOEnemySkill ability) 
    {
        enemiesSelected.Clear();

        AttackName.text = ability.Name;

        AttackName.gameObject.SetActive(false);
        gameObject.SetActive(true);

        StartCoroutine(AnimationBehaviour(enemyActing, ability));
    }
    public void Attack(CombatAgent enemyActing, List<CombatAgent> enemiesSelected, SOEnemySkill ability)
    {
        Attack(enemyActing, ability);

        this.enemiesSelected = enemiesSelected;

        // hide selectors secondary
        foreach (var selectSecondary in enemiesSelected)
            selectSecondary.SelectTarget(true);
    }


    private IEnumerator AnimationBehaviour(CombatAgent enemyActing, SOEnemySkill ability)
    {
        //SelectPrimary.gameObject.SetActive(true);

        // Select
        enemyActing.SelectActing(true);

        yield return preActWait;

        AttackName.gameObject.SetActive(true);

        enemyActing.TakeAction();

        // Do ability
        manager.EnemyAttack(enemyActing.Actor, ability);

        yield return postActWait;

        enemyActing.SelectActing(false);

        // send animation finish to actor
        OnAnimationFinish?.Invoke();

        // hide selectors secondary
        foreach (var selectSecondary in enemiesSelected)
            selectSecondary.SelectTarget(false);

        // hide screen
        gameObject.SetActive(false);
    }
}
