using UnityEngine;

public class CombatHitMessage : MonoBehaviour
{
    public TMPro.TMP_Text Text;
    public int SizeSmall;
    public int SizeMedium;
    public int SizeBig;

    public bool IsAvailable { get; private set; } = true;

    private float messageTime = 0.75f;
    private float messageTimeCurrent;

    public void ShowNumber(string value, int size) 
    {
        Text.text = value.ToString();

        // set size
        switch (size)
        {
            case 1:
                Text.fontSize = SizeSmall;
                break;
            case 2:
                Text.fontSize = SizeMedium;
                break;
            case 3:
                Text.fontSize = SizeBig;
                break;
            default:
                break;
        }

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
