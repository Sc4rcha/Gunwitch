using System;
using EasyTextEffects;
using UnityEngine;
using UnityEngine.UI;
using GameInfo;

public class ManagerDialogue : MonoBehaviour
{
    [Header("Scene References")]
    public RectTransform TransformCharacterRight;
    public RectTransform TransformCharacterLeft;
    public RectTransform TransformExpression;
    public RectTransform TransformText;
    [Space]
    public DialogueExpression CharacterExpression;
    public Image CharacterPortraitRight;
    public Image CharacterPortraitLeft;
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
    [Space]
    public Color FocusOffColor;

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
        for (int i = 0; i < DecisionButtons.Length; i++)
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
                CharacterExpression.Emotions[i].gameObject.SetActive(i + 1 == node.IndexEmotion);
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
        PlayerHUDPortrait.Instance.SetFocusPlayer(false);
        CharacterPortraitRight.color = FocusOffColor;
        CharacterPortraitLeft.color = FocusOffColor;

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
            PlayerHUDPortrait.Instance.SetFocusPlayer(true);
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
}
