using UnityEngine;
using System;
using System.Collections;

public class CombatScreenAttack : MonoBehaviour
{
    public GameObject SelectPrimary;
    public GameObject[] SelectSecondary;
    public TMPro.TMP_Text AttackName;


    public event Action OnAnimationFinish;

    private WaitForSeconds preActWait;
    private WaitForSeconds postActWait;
    private ManagerCombat manager;


    public void Setup(ManagerCombat manager) 
    {
        this.manager = manager;

        preActWait = new WaitForSeconds(0.5f);
        postActWait = new WaitForSeconds(1);

        gameObject.SetActive(false);
    }

    public void Attack(CombatAgent enemyActing, SOEnemySkill ability) 
    {
        AttackName.text = ability.Name;
        SelectPrimary.transform.position = enemyActing.MarkerSelect.position;

        AttackName.gameObject.SetActive(false);
        SelectPrimary.gameObject.SetActive(false);
        gameObject.SetActive(true);

        StartCoroutine(AnimationBehaviour(enemyActing, ability));
    }
    public void Attack(CombatAgent enemyActing, CombatAgent[] enemiesSelected, SOEnemySkill ability)
    {
        Attack(enemyActing, ability);

        for (int i = 0; i < enemiesSelected.Length; i++)
        {
            SelectSecondary[i].SetActive(true);
            SelectSecondary[i].transform.position = enemiesSelected[i].MarkerSelect.position;
        }
    }


    private IEnumerator AnimationBehaviour(CombatAgent enemyActing, SOEnemySkill ability)
    {
        SelectPrimary.gameObject.SetActive(true);

        // Select
        enemyActing.Select(true);

        yield return preActWait;

        AttackName.gameObject.SetActive(true);

        enemyActing.TakeAction();

        // Do ability
        manager.EnemyAttack(enemyActing.Actor, ability);

        yield return postActWait;

        enemyActing.Select(false);
        SelectPrimary.gameObject.SetActive(false);

        // send animation finish to actor
        OnAnimationFinish?.Invoke();

        // hide selectors secondary
        foreach (var selectSecondary in SelectSecondary)
            selectSecondary.SetActive(false);

        // hide screen
        gameObject.SetActive(false);
    }
}
