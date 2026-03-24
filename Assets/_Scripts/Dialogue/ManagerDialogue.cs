using System;
using EasyTextEffects;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class ManagerDialogue : MonoBehaviour
{
    [Header ("Scene References")]
    public RectTransform TransformCharacterRight;
    public RectTransform TransformCharacterLeft;
    public RectTransform TransformExpression;
    public RectTransform TransformText;
    [Space]
    public DialogueExpression CharacterExpression;
    public Image CharacterPortraitRight;
    public Image CharacterPortraitLeft;
    public Image CharacterPortraitPlayer;
    [Space]
    public GameObject TextNextHint;
    public TMPro.TMP_Text DialogueText;
    public TextEffect DialogueTextEffects;
    public DialogueDecisionButton[] DecisionButtons;
    [Space]
    public DialogueSFXBeep SFXBeep;

    [Header("Variables")]
    public Vector2 OffsetCharacterRight;
    public Vector2 OffsetCharacterCentre;
    public Vector2 OffsetCharacterLeft;
    [Space]
    public Vector2 OffsetTextRight;
    public Vector2 OffsetTextLeft;
    public Vector2 OffsetTextCentre;

    // dialogue finished action
    public event Action OnDialogueFinished;
    public event Action<DecisionOption> OnDialogueDecision;

    private Dialogue dialogueSelected;
    private int dialogueIndex;

    public void Setup() 
    {
        // hide dialogue screen
        gameObject.SetActive(false);

        // setup dialogue options
        for (int i = 0; i < DecisionButtons.Length;i++)
            DecisionButtons[i].Setup((DecisionOption)i);
    }


    private void Update()
    {
        // activate next hint when dialogue text has stopped appearing
        TextNextHint.SetActive(DialogueTextEffects.QueryEffectStatuses(TextEffectType.Global, TextEffectEntry.TriggerWhen.Manual)[1].IsComplete);
    }


    public void DialogueStart(SODialogue dialogueReference) 
    {

        // show dialogue screen
        gameObject.SetActive(true);

        // set dialogue selected
        dialogueSelected = dialogueReference.GetDialogue();
        dialogueIndex = 0;

        // display first node
        RenderDialogueNode(dialogueSelected.Nodes[0]);
    }
    public void DialogueEnd() 
    {
        // hide dialogue
        gameObject.SetActive(false);

        // call action
        OnDialogueFinished?.Invoke();
    }

    public void DialogueNext() 
    {
        // skip animation and skip dialogue Next
        if (!DialogueTextEffects.QueryEffectStatuses(TextEffectType.Global, TextEffectEntry.TriggerWhen.Manual)[1].IsComplete)
        {
            DialogueTextEffects.StopAllEffects();
            SFXBeep.BeepingForceStop();
            return;
        }

        // add to dialogue index
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
    public void DialogueDecisionSetup(string[] options) 
    {
        // show dialogue
        gameObject.SetActive(true);

        // show buttons
        for (int i = 0; i < DecisionButtons.Length; i++)
            DecisionButtons[i].SetupDecisionButton(options[i]);
    }
    public void DialogueDecisionPick(DecisionOption option)
    {
        // hide buttons
        for (int i = 0; i < DecisionButtons.Length; i++)
            DecisionButtons[i].SetupDecisionButton("");

        // call action
        OnDialogueDecision?.Invoke(option);
    }

    private void RenderDialogueNode(DialogueNode node)
    {
        // activate or deactivate characters
        TransformCharacterRight.gameObject.SetActive(node.IndexCharacterRight > 0);
        TransformCharacterLeft.gameObject.SetActive(node.IndexCharacterLeft > 0);

        // set characters portrait sprites
        if (node.IndexCharacterRight > 0)
            CharacterPortraitRight.sprite = dialogueSelected.Characters[node.IndexCharacterRight].Portrait;
        if (node.IndexCharacterLeft > 0)
            CharacterPortraitLeft.sprite = dialogueSelected.Characters[node.IndexCharacterLeft].Portrait;

        // set character positions
        if (node.IndexCharacterRight > 0 && node.IndexCharacterLeft > 0)
        {
            // each to one side
            TransformCharacterRight.anchoredPosition = OffsetCharacterRight;
            TransformCharacterLeft.anchoredPosition = OffsetCharacterLeft;
        }
        else if (node.IndexCharacterRight > 0)
        {
            // right centre
            TransformCharacterRight.anchoredPosition = OffsetCharacterCentre;
        }
        else if (node.IndexCharacterLeft > 0)
        {
            // left centre
            TransformCharacterLeft.anchoredPosition = OffsetCharacterCentre;
        }

        // activate or deactivate expression
        TransformExpression.gameObject.SetActive(node.IndexCharacterFocus > 0);
        // set character expression
        if (node.IndexCharacterFocus > 0)
        {
            CharacterExpression.Expression.sprite = dialogueSelected.Characters[node.IndexCharacterFocus].Expressions[node.IndexExpression];
            // set emotion
            for (int i = 0; i < 2; i++)
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
            CharacterExpression.SetSideLeft(true);
        }
        else
        {
            // right character talking
            TransformText.anchoredPosition = OffsetTextLeft;
            CharacterExpression.SetSideLeft(false);
        }

        // set focus to nothing
        CharacterPortraitPlayer.color = Color.gray7;
        CharacterPortraitRight.color = Color.gray7;
        CharacterPortraitLeft.color = Color.gray7;

        // set focus to correct character
        if (node.IndexCharacterFocus == node.IndexCharacterRight)
        {
            //right
            CharacterPortraitRight.color = Color.white;
            TransformCharacterRight.SetAsLastSibling();
        }
        else if (node.IndexCharacterFocus == node.IndexCharacterLeft)
        {
            //left
            CharacterPortraitLeft.color = Color.white;
            TransformCharacterLeft.SetAsLastSibling();
        }
        else if (node.IndexCharacterFocus == 1)
        {
            //player
            CharacterPortraitPlayer.color = Color.white;
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


    public enum DecisionOption { OptionA, OptionB, OptionC }
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
    [Serializable]
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
        // variables for dialogue section
        public Character[] Characters;
        public DialogueNode[] Nodes;

        // constructors
        public Dialogue(List<Character> characters, List<DialogueNode> nodes)
        {
            Characters = characters.ToArray();
            Nodes = nodes.ToArray();
        }
    }
}
