using HutongGames.PlayMaker.Actions;
using UnityEngine;

public class CombatAgentShake : MonoBehaviour
{
    public const float ShakeStrength = 0.3f;
    public const float ShakeScaleStrength = 0.05f;
    public const float ShakeStepTime = 0.1f;

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
        initialPosition = transform.localPosition;
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
        transform.localPosition = initialPosition;
        transform.localScale = initialScale;

        enabled = false;
    }

    private void Update()
    {
        shakeTimeCurrent += Time.deltaTime;

        // shake steps
        if (shakeTimeCurrent > (shakeTime / shakeSteps) * shakeStepsCurrent)
        {
            transform.localScale = initialScale - Vector3.one * (1-(shakeTimeCurrent / shakeTime)) * ShakeScaleStrength;
            transform.localPosition = initialPosition + (Vector3)Random.insideUnitCircle.normalized * (1 - (shakeTimeCurrent / shakeTime)) * ShakeStrength;
            shakeStepsCurrent++;
        }

        if (shakeTimeCurrent > shakeTime)
            ShakeFinish();
    }
}
