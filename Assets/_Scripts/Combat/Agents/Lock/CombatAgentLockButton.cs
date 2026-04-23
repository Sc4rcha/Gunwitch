using System.Collections.Generic;
using UnityEngine;

public class CombatAgentLockButton : CombatAgent
{
    [Header("Lock Button")]
    [Range (1,11)]
    public int Amount;
    public List <Transform> Steps;

    [HideInInspector]
    public LockButtonCircle Circle;

    public override void Setup(ManagerCombat manager)
    {
        base.Setup(manager);

        // set step rotations
        for (int i = 0; i < Steps.Count; i++)
            Steps[i].rotation = Quaternion.Euler(0, 0, i * 360 / Steps.Count);

        // show steps corresponding to button amount
        List<Transform> stepsPool = new List<Transform>(Steps);

        for (int i = 0; i < 12 - Amount; i++)
        {
            // get index of randomly selected step
            int index = Random.Range(0, stepsPool.Count);

            // Disable the selected step from the pool
            stepsPool[index].gameObject.SetActive(false);

            // remove step reference from pool
            stepsPool.RemoveAt(index);
        }
    }


    public override void Damage(int value, bool isCrit)
    {
        base.Damage(value, isCrit);

        // send lock rotation event
        Circle.ButtonHit(Amount);

        // stop movement while its hurt
        Circle.MovePause(hurtAnimationTime);
    }
}
