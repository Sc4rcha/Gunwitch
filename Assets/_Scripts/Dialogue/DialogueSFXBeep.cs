using EasyTextEffects.Effects;
using UnityEngine;
using UnityEngine.Audio;

public class DialogueSFXBeep : MonoBehaviour
{
    public AudioSource AudioSource;
    public Effect_Color TypeWriter;
    [Space]
    public AudioResource NarratorBeep;

    private float timeToFinishCurrent;
    private float timeToFinish;
    private float timeToNextBeepCurrent;
    private float timeToNextBeep;

    private bool isPlaying;
    
    public void BeepingStart(AudioResource characterBeep, int textLength) 
    {
        // set character sound
        AudioSource.resource = characterBeep;

        // set timer variables
        timeToNextBeep = TypeWriter.timeBetweenChars;
        timeToNextBeepCurrent = 0;
        timeToFinish = timeToNextBeep * textLength;
        timeToFinishCurrent = 0;

        // set is plating to true
        isPlaying = true;
    }
    public void BeepingStart (int textLength)
    {
        BeepingStart(NarratorBeep, textLength);
    }

    public void BeepingFinish() 
    {
        isPlaying = false;
        AudioSource.Stop();
    }


    private void Update()
    {
        // stop if not playing beeps!
        if (!isPlaying)
            return;

        timeToNextBeepCurrent += Time.deltaTime;
        if (timeToNextBeepCurrent > timeToNextBeep && !AudioSource.isPlaying)
        {
            timeToNextBeepCurrent = 0;
            AudioSource.Play();
        }

        timeToFinishCurrent += Time.deltaTime;
        if (timeToFinishCurrent > timeToFinish)
            BeepingFinish();
    }
}
