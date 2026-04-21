using System.Collections;
using UnityEngine;

public class CombatEffects : MonoBehaviour
{

    private Coroutine hitStopCoroutine;

    public void HitStopStart()
    {
        if (hitStopCoroutine != null)
            StopCoroutine(hitStopCoroutine);

        hitStopCoroutine = StartCoroutine(HaitStop());
    }

    private IEnumerator HaitStop()
    {
        yield return new WaitForEndOfFrame();

        Time.timeScale = 0f;

        yield return new WaitForSecondsRealtime(0.1f);

        Time.timeScale = 1f;
        hitStopCoroutine = null;
    }

}
