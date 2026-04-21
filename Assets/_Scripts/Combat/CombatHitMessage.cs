using UnityEngine;

public class CombatHitMessage : MonoBehaviour
{
    public TMPro.TMP_Text Text;

    public bool IsAvailable { get; private set; } = true;

    private float messageTime = 0.75f;
    private float messageTimeCurrent;

    public void ShowNumber(int value) 
    {
        Text.text = value.ToString();

        messageTimeCurrent = 0;

        IsAvailable = false;
        gameObject.SetActive(true);
    }
    public void ShowMiss() 
    {
        Text.text = "MISS";

        messageTimeCurrent = 0;

        IsAvailable = false;
        gameObject.SetActive(true);
    }

    private void Update()
    {
        messageTimeCurrent += Time.deltaTime;

        if (messageTime < messageTimeCurrent)
        {
            IsAvailable = true;
            gameObject.SetActive(false);
        }
    }
}
