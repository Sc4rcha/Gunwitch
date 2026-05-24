using UnityEngine;

public class CombatHitMessage : MonoBehaviour
{
    public TMPro.TMP_Text Text;
    public int SizeSmall;
    public int SizeMedium;
    public int SizeBig;
    [Space]
    public TMPro.TMP_FontAsset FontAssetText;
    public TMPro.TMP_FontAsset FontAssetDamage;
    public TMPro.TMP_FontAsset FontAssetHeal;

    public enum MessageType { Heal, Damage, Text}

    public bool IsAvailable { get; private set; } = true;

    private float messageTime = 0.75f;
    private float messageTimeCurrent;

    public void ShowNumber(string value, int size, MessageType type) 
    {
        Text.text = value.ToString();

        switch (type)
        {
            case MessageType.Heal:
                Text.font = FontAssetHeal;
                break;
            case MessageType.Damage:
                Text.font = FontAssetDamage;
                break;
            case MessageType.Text:
                Text.font = FontAssetText;
                break;
        }

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
