using System.Collections;
using UnityEditor;
using UnityEngine;

public class CombatAgentLock : CombatAgent
{
    public const int CircleSteps = 12;

    [Header("Lock Variables")]
    [Range(0, 11)]
    public int CursorPosition;
    [Range(0, 11)]
    public int[] Objectives;

    [Header("Lock References")]
    public Transform LockObjective;
    public Transform LockCursor;
    public LockButtonCircle[] Circles;
    public GameObject[] StageProgress;
    public ParticleSystem CircleBreakParticles;

    private int rotationStep => 360 / CircleSteps;

    private int positionObjective;
    private int positionCursor;

    private int stageIndex;

    // rotation animation variables
    private const float rotateTimeMax = 2;
    private float rotateTime;
    private float rotateTimeCurrent;

    private float lerpStartAngleTotal;
    private float lerpTargetAngleTotal;
    private float angleCurrent;


    public override void Setup(ManagerCombat manager)
    {
        base.Setup(manager);

        // setup lock circles
        foreach (var circle in Circles)
            circle.Setup(manager, this);

        // show progress circles
        for (int i = 0; i < StageProgress.Length; i++)
            StageProgress[i].SetActive(i < Objectives.Length);

        // set initial cursor position
        positionCursor = CursorPosition;

        // show FIRST step
        stageIndex = 0;
        SetStage(stageIndex);
    }

    private void Update()
    {
        // advance lerp time
        rotateTimeCurrent += Time.deltaTime;

        // move lerp and snap to steps
        angleCurrent = Mathf.Lerp(lerpStartAngleTotal, lerpTargetAngleTotal, rotateTimeCurrent / rotateTime);
        angleCurrent = Mathf.Round(angleCurrent / rotationStep) * rotationStep;

        // change cursor rotation
        LockCursor.rotation = Quaternion.Euler(0, 0, angleCurrent);

        // detect if stage is complete
        if (rotateTimeCurrent > rotateTime && !Actor.IsDead && positionObjective == positionCursor)
            CheckStageComplete();
    }


    public void Rotate(int amount)
    {
        int previousPosition = positionCursor;

        positionCursor = (positionCursor + amount) % CircleSteps;
        if (positionCursor < 0) positionCursor += CircleSteps;

        // get rotation time
        rotateTime = rotateTimeMax * ((float)Mathf.Abs(amount) / CircleSteps);
        rotateTimeCurrent = 0;

        // derive angles ONLY from step indices
        lerpStartAngleTotal = previousPosition * rotationStep;

        // force correct direction (no shortest path)
        lerpTargetAngleTotal = lerpStartAngleTotal + (amount * rotationStep);
    }

    private void SetStage(int index)
    {
        // set position objective
        positionObjective = Objectives[index];

        LockCursor.rotation = Quaternion.Euler(0, 0, positionCursor * rotationStep);
        LockObjective.rotation = Quaternion.Euler(0, 0, positionObjective * rotationStep);
        Rotate(0);

        StartCoroutine(StageChangeAnimation());
    }
    private void CheckStageComplete()
    {
        CircleBreakParticles.Play();
        StageProgress[stageIndex].SetActive(false);
        stageIndex++;
        // kill if last stage complete
        if (stageIndex == Objectives.Length)
            Damage(999, false);
        // go to next stage otherwise
        else
            SetStage(stageIndex);
    }

    private IEnumerator StageChangeAnimation() 
    {
        LockObjective.gameObject.SetActive(false);
        yield return new WaitForSeconds(1);
        LockObjective.gameObject.SetActive(true);
    }


#if UNITY_EDITOR
    protected override void DrawGizmos()
    {
        base.DrawGizmos();

        Handles.color = Color.magenta;
        Handles.DrawWireDisc(transform.position + (Vector3)GetPosition(CursorPosition, 0.2f + 1.4f), Vector3.forward, 0.1f);

        for (int i = 0; i < Objectives.Length; i++)
        {
            Handles.color = Color.cyan;
            Handles.DrawWireDisc(transform.position + (Vector3)GetPosition(Objectives[i], (i * 0.2f) + 1.4f), Vector3.forward, 0.1f);
        }
    }
    private Vector2 GetPosition(int step, float radius)
    {
        float theta = ((step * 360 / CircleSteps) + 90) * Mathf.Deg2Rad;

        float x = radius * Mathf.Cos(theta);
        float y = radius * Mathf.Sin(theta);

        return new Vector2(x, y);
    }
#endif
}
