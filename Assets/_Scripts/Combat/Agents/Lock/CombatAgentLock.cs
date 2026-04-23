using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class CombatAgentLock : CombatAgent
{
    public const int CircleSteps = 12;

    [Header("Lock Variables")]
    public LockStage[] Stages;

    [Header ("Lock References")]
    public Transform LockObjective;
    public Transform LockCursor;
    public LockButtonCircle[] Circles;

    private int rotationStep => 360 / CircleSteps;

    private int positionObjective;
    private int positionCursor;

    private int stageIndex;

    // rotation animation variables
    private const float rotateTimeMax = 1;
    private float rotateTime;
    private float rotateTimeCurrent;
    private float lerpStartAngle;
    private float lerpTargetAngle;


    public override void Setup(ManagerCombat manager)
    {
        base.Setup(manager);

        // setup lock circles
        foreach (var circle in Circles)
            circle.Setup(manager, this);

        stageIndex = 0;
        SetStage(stageIndex);
    }

    private void SetStage(int index) 
    {
        // set position objective
        positionCursor = Stages[index].PositionCursor;
        positionObjective = Stages[index].PositionObjective;

        LockCursor.rotation = Quaternion.Euler(0, 0, positionCursor * rotationStep);
        LockObjective.rotation = Quaternion.Euler(0, 0, positionObjective * rotationStep);
        Rotate(0);
    }

    private void Update()
    {
        rotateTimeCurrent += Time.deltaTime;

        LockCursor.rotation = Quaternion.Euler(0, 0, Mathf.Lerp(lerpStartAngle, lerpTargetAngle, rotateTimeCurrent / rotateTime));

        if (!IsHurt)
            IsDoingStuff = rotateTimeCurrent < rotateTime;

        if (rotateTimeCurrent > rotateTime && !Actor.IsDead)
            CheckStageComplete();
    }


    public void Rotate(int amount) 
    {
        int previousPosition = positionCursor;

        positionCursor = (positionCursor + amount) % CircleSteps;
        if (positionCursor < 0) positionCursor += CircleSteps;

        rotateTime = rotateTimeMax * ((float)Mathf.Abs(amount) / CircleSteps);
        rotateTimeCurrent = 0;

        // derive angles ONLY from step indices
        lerpStartAngle = previousPosition * rotationStep;

        // force correct direction (no shortest path)
        lerpTargetAngle = lerpStartAngle + (amount * rotationStep);
    }
    private void CheckStageComplete() 
    {
        if (positionObjective == positionCursor)
        {
            stageIndex++;
            // kill if last stage complete
            if (stageIndex == Stages.Length)
                Damage(999, false);
            // go to next stage otherwise
            else
                SetStage(stageIndex);
        }
    }


    [System.Serializable]
    public struct LockStage 
    {
        [Range(0, 11)]
        public int PositionCursor;
        [Range(0, 11)]
        public int PositionObjective;
    }


#if UNITY_EDITOR
    public void OnDrawGizmosSelected()
    {
        for (int i = 0; i < Stages.Length; i++)
        {
            Handles.color = Color.magenta;
            Handles.DrawWireDisc(transform.position + (Vector3)GetPosition(Stages[i].PositionCursor, (i*0.2f) + 1.4f), Vector3.forward, 0.1f);
            Handles.color = Color.cyan;
            Handles.DrawWireDisc(transform.position + (Vector3)GetPosition(Stages[i].PositionObjective, (i * 0.2f) + 1.4f), Vector3.forward, 0.1f);
        }
    }

    public static Vector2 GetPosition(int step, float radius)
    {
        float theta = ((step * 360 / CircleSteps) + 90) * Mathf.Deg2Rad;

        float x = radius * Mathf.Cos(theta);
        float y = radius * Mathf.Sin(theta);

        return new Vector2(x, y);
    }
#endif
}
