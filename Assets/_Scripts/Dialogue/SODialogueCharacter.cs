using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "Character", menuName = "Dialogue/Character")]
public class SODialogueCharacter : ScriptableObject
{
    public string Name;
    [Space]
    public Sprite Portrait;
    [Space]
    public Sprite ExpressionNeutral;
    public Sprite ExpressionHappy;
    public Sprite ExpressionAngry;
    public Sprite ExpressionSad;
    public Sprite ExpressionSurprised;
    [Space]
    public AudioResource Beep;

    public GameInfo.Character GetCharacter() 
    {
        return new GameInfo.Character(this);
    }
}
