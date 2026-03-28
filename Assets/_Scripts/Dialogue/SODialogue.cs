using System.Collections.Generic;
using UnityEngine;
using GameInfo;

[CreateAssetMenu(fileName = "Dialogue", menuName = "Dialogue/Dialogue")]
public class SODialogue : ScriptableObject
{
    public List<SODialogueCharacter> Characters;

    [HideInInspector]
    public List<DialogueNode> Nodes;


    public Dialogue GetDialogue() 
    {
        return new Dialogue(this);
    }
}
