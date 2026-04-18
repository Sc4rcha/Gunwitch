using HutongGames.PlayMaker.Actions;
using UnityEngine;

public class CombatAgentShake : MonoBehaviour
{
    public const float ShakeStrength = 0.3f;
    public const float ShakeScaleStrength = 0.05f;
    public const float ShakeStepTime = 0.1f;


    public AnimationCurve IntensityCurve;


    private Vector3 initialScale;
    private Vector3 initialPosition;
    private float shakeTime;
    private float shakeTimeCurrent;

    private int shakeSteps;
    private int shakeStepsCurrent;

    public void ShakeStart(int strength) 
    {
        // if already enabled shake finish
        if (enabled)
            ShakeFinish();

        // get initial state 
        initialPosition = transform.position;
        initialScale = transform.localScale;

        // setup shake variables
        shakeTime = ShakeStepTime * (strength);
        shakeSteps = strength;
        shakeStepsCurrent = 0;
        shakeTimeCurrent = 0;

        // enable component and start shake
        enabled = true;
    }
    public void ShakeFinish() 
    {
        // reset state to initial state
        transform.position = initialPosition;
        transform.localScale = initialScale;
    }

    private void Update()
    {
        shakeTimeCurrent += Time.deltaTime;

        // shake steps
        if (shakeTimeCurrent > (shakeTime / shakeSteps) * shakeStepsCurrent)
        {
            transform.localScale = initialScale - Vector3.one * IntensityCurve.Evaluate(shakeTimeCurrent / shakeTime) * ShakeScaleStrength;
            transform.position = initialPosition + (Vector3)Random.insideUnitCircle.normalized * IntensityCurve.Evaluate(shakeTimeCurrent / shakeTime) * ShakeStrength;
            shakeStepsCurrent++;
        }

        if (shakeTimeCurrent > shakeTime)
            enabled = false;
    }
}
