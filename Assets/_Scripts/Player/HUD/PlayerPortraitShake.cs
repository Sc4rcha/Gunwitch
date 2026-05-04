using UnityEngine;

public class PlayerPortraitShake : MonoBehaviour
{
    public const float ShakeStrength = 10;
    public const float ShakeStepTime = 0.1f;

    private Vector3 initialPosition;
    private float shakeTime;
    private float shakeTimeCurrent;

    private int shakeSteps;
    private int shakeStepsCurrent;
    private int directionCurrent;

    public void ShakeStart(int strength)
    {
        // if already enabled shake finish
        if (enabled)
            ShakeFinish();

        // get initial state 
        initialPosition = transform.localPosition;

        // setup shake variables
        shakeTime = ShakeStepTime * (strength);
        shakeSteps = strength;
        shakeStepsCurrent = 0;
        shakeTimeCurrent = 0;
        directionCurrent = 1;

        // enable component and start shake
        enabled = true;
    }

    public void ShakeFinish()
    {
        // reset state to initial state
        transform.localPosition = initialPosition;

        enabled = false;
    }

    private void Update()
    {
        shakeTimeCurrent += Time.deltaTime;

        // shake steps
        if (shakeTimeCurrent > (shakeTime / shakeSteps) * shakeStepsCurrent)
        {
            transform.localPosition = initialPosition + Vector3.left * (1 - (shakeTimeCurrent / shakeTime)) * ShakeStrength * directionCurrent;
            directionCurrent *= -1;
            shakeStepsCurrent++;
        }

        if (shakeTimeCurrent > shakeTime)
            ShakeFinish();
    }
}
