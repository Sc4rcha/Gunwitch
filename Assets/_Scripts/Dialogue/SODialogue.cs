using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue", menuName = "Dialogue/Dialogue")]
public class SODialogue : ScriptableObject
{
    public List<SODialogueCharacter> Characters;

    [HideInInspector]
    public List<ManagerDialogue.DialogueNode> Nodes;


    public ManagerDialogue.Dialogue GetDialogue() 
    {
        // create characters list and make first element player
        List<ManagerDialogue.Character> characters = new List<ManagerDialogue.Character>
        {
            null,
            Resources.Load<SODialogueCharacter>("Player").GetCharacter()
        };
        foreach (var character in Characters)
            characters.Add(character.GetCharacter());

        return new ManagerDialogue.Dialogue(characters, Nodes);
    }
}
