using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "Character", menuName = "Dialogue/Character")]
public class SODialogueCharacter : ScriptableObject
{
    public string CharacterName;
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

    public ManagerDialogue.Character GetCharacter() 
    {
        ManagerDialogue.Character character = new ManagerDialogue.Character();

        character.Name = CharacterName;
        character.Portrait = Portrait;
        character.SetExpressions(
            ExpressionNeutral,
            ExpressionHappy,
            ExpressionAngry,
            ExpressionSad,
            ExpressionSurprised
            );
        character.Beep = Beep;

        return character;
    }
}
