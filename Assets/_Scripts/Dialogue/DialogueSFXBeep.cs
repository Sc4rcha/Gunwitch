using EasyTextEffects.Effects;
using UnityEngine;
using UnityEngine.Audio;

public class DialogueSFXBeep : MonoBehaviour
{
    public AudioSource AudioSource;
    public Effect_Color TypeWriter;
    [Space]
    public AudioResource NarratorBeep;

    private float timeToFinish;
    private float timeToFinishCurrent;

    private bool isPlaying;
    
    public void BeepingStart(AudioResource characterBeep, int textLength) 
    {
        // set character sound
        AudioSource.resource = characterBeep;

        // set timer variables
        timeToFinish = TypeWriter.timeBetweenChars * textLength;
        timeToFinishCurrent = 0;

        // set is plating to true
        isPlaying = true;
    }
    public void BeepingStart (int textLength)
    {
        // beep start with narrator beep
        BeepingStart(NarratorBeep, textLength);
    }

    public void BeepingForceStop() 
    {
        // set time to finish
        timeToFinishCurrent = timeToFinish;
    }


    private void Update()
    {
        // stop if not playing beeps!
        if (!isPlaying)
            return;

        // stop beeping
        timeToFinishCurrent += Time.deltaTime;
        if (timeToFinishCurrent > timeToFinish && !AudioSource.isPlaying)
        {
            AudioSource.Stop();
            isPlaying = false;
            return;
        }

        // do beep again if the beep has already finishing playing
        if (!AudioSource.isPlaying)
            AudioSource.Play();

    }
}
