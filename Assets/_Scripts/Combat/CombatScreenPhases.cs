using UnityEngine;
using System;

public class CombatScreenPhases : MonoBehaviour
{
    public TMPro.TMP_Text PhaseText;

    public event Action OnPhaseMessageFinish;

    private float MessageTime;
    private float messageTimeCurrent;

    public void ShowPhase(string text, float messageTime) 
    {
        MessageTime = messageTime;
        messageTimeCurrent = 0;

        PhaseText.text = text;

        gameObject.SetActive(true);
    }

    private void Update()
    {
        messageTimeCurrent += Time.deltaTime;
        if (messageTimeCurrent > MessageTime)
        {
            gameObject.SetActive(false);
            OnPhaseMessageFinish?.Invoke();
        }
    }
}
