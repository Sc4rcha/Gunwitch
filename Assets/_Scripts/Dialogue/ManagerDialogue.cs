using EasyTextEffects;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class ManagerDialogue : MonoBehaviour
{
    public SODialogue dialogueTest;

    [Header ("Scene References")]
    public RectTransform TransformCharacterRight;
    public RectTransform TransformCharacterLeft;
    public RectTransform TransformExpression;
    public RectTransform TransformText;
    [Space]
    public DialogueExpression CharacterExpression;
    public Image CharacterPortraitRight;
    public Image CharacterPortraitLeft;
    [Space]
    public TMPro.TMP_Text DialogueText;
    public TextEffect DialogueTextEffects;
    [Space]
    public DialogueSFXBeep SFXBeep;

    [Header("Variables")]
    public Vector2 OffsetCharacterRight;
    public Vector2 OffsetCharacterCentre;
    [Space]
    public Vector2 OffsetTextRight;
    public Vector2 OffsetTextLeft;
    public Vector2 OffsetTextCentre;


    private Dialogue dialogueSelected;
    private int dialogueIndex;

    public void Setup() 
    {
        DialogueStart(dialogueTest);
    }


    public void DialogueStart(SODialogue dialogueReference) 
    {
        gameObject.SetActive(true);

        dialogueSelected = dialogueReference.GetDialogue();
        dialogueIndex = 0;

        // display first node
        RenderDialogueNode(dialogueSelected.Nodes[0]);
    }
    public void DialogueEnd() 
    {
        gameObject.SetActive(false);
    }

    public void DialogueNext() 
    {
        if (!DialogueTextEffects.QueryEffectStatuses(TextEffectType.Global, TextEffectEntry.TriggerWhen.Manual)[1].IsComplete)
        {
            DialogueTextEffects.StopAllEffects();
            SFXBeep.BeepingFinish();
            return;
        }

        dialogueIndex++;

        // dialogue end if no more nodes
        if (dialogueIndex == dialogueSelected.Nodes.Length)
        {
            DialogueEnd();
            return;
        }

        // render dialogue node
        RenderDialogueNode(dialogueSelected.Nodes[dialogueIndex]);
    }

    private void RenderDialogueNode(DialogueNode node) 
    {
        // activate or deactivate characters
        TransformCharacterRight.gameObject.SetActive(node.IndexCharacterRight > 0);
        TransformCharacterLeft.gameObject.SetActive(node.IndexCharacterLeft > 0);

        // set characters portraits
        if (node.IndexCharacterRight > 0)
            CharacterPortraitRight.sprite = dialogueSelected.Characters[node.IndexCharacterRight].Portrait;
        if (node.IndexCharacterLeft > 0)
            CharacterPortraitLeft.sprite = dialogueSelected.Characters[node.IndexCharacterLeft].Portrait;

        // set right character position
        if (node.IndexCharacterRight > 0 && node.IndexCharacterLeft > 0)
            TransformCharacterRight.anchoredPosition = OffsetCharacterRight;
        else
            TransformCharacterRight.anchoredPosition = OffsetCharacterCentre;

        // activate or deactivate expression
        TransformExpression.gameObject.SetActive(node.IndexCharacterFocus > 0);
        // set character expression
        if (node.IndexCharacterFocus > 0)
        {
            CharacterExpression.Expression.sprite = dialogueSelected.Characters[node.IndexCharacterFocus].Expressions[node.IndexExpression];
            // set emotion
            for (int i = 0; i < 3; i++)
                CharacterExpression.Emotions[i].gameObject.SetActive(i+1 == node.IndexEmotion);
        }

        // set expression and text position
        if (node.IndexCharacterFocus == 0)
        {
            // no character focus
            TransformText.anchoredPosition = OffsetTextCentre;
        }
        else if (node.IndexCharacterFocus == node.IndexCharacterLeft || node.IndexCharacterFocus == 1)
        {
            // Left character or player talking
            TransformText.anchoredPosition = OffsetTextRight;
            CharacterExpression.SetSide(-1);
        }
        else
        {
            // right character talking
            TransformText.anchoredPosition = OffsetTextLeft;
            CharacterExpression.SetSide(1);
        }

        // set focus to nothing
        CharacterPortraitRight.color = Color.gray8;
        CharacterPortraitLeft.color = Color.gray8;

        // set focus to correct character
        if (node.IndexCharacterFocus == node.IndexCharacterRight)
        {
            CharacterPortraitRight.color = Color.white;
            TransformCharacterRight.SetAsLastSibling();
        }
        else if (node.IndexCharacterFocus == node.IndexCharacterLeft)
        {
            CharacterPortraitLeft.color = Color.white;
            TransformCharacterLeft.SetAsLastSibling();
        }

        // set text
        DialogueText.text = node.Text;

        // start text effects
        DialogueTextEffects.Refresh();
        DialogueTextEffects.StartManualEffects();

        // start beep SFX
        if (node.IndexCharacterFocus != 0)
            SFXBeep.BeepingStart(dialogueSelected.Characters[node.IndexCharacterFocus].Beep, node.Text.Length);
        else
            SFXBeep.BeepingStart(node.Text.Length);
    }


    public class Character
    {
        public string Name;
        public Sprite Portrait;
        public Sprite[] Expressions;
        public AudioResource Beep;

        public void SetExpressions(Sprite neutral, Sprite happy, Sprite angry, Sprite sad, Sprite surprised) 
        {
            Expressions = new Sprite[5];
            Expressions[0] = neutral;
            Expressions[1] = happy;
            Expressions[2] = angry;
            Expressions[3] = sad;
            Expressions[4] = surprised;
        }
    }

    [System.Serializable]
    public class DialogueNode 
    {
        // 0:player | 1+:other characters
        public int IndexCharacterRight;
        public int IndexCharacterLeft;

        // 0:none | 1:player | 2:characterRight | 3:characterLeft
        public int IndexCharacterFocus;
        // 0:Neutral | 1:Happy | 2:Angry | 3:Sad | 4:Surprised
        public int IndexExpression;
        // 0:none | 1:SweatDrop | 2:Veins | 3:Dots
        public int IndexEmotion;

        public string Text;
    }

    public class Dialogue 
    {
        public Character[] Characters;
        public DialogueNode[] Nodes;

        public Dialogue(List <Character> characters, List <DialogueNode> nodes ) 
        {
            Characters = characters.ToArray();
            Nodes = nodes.ToArray();
        }
    }
}
