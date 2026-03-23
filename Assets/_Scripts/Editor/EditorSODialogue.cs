using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SODialogue))]
public class EditorSODialogue : Editor
{
    private SODialogue dialogue;
    private SerializedObject dialogueObj;

    private SerializedProperty dialogueCharacters;
    private SerializedProperty dialogueNodes;

    private SerializedProperty indexcharacterRight;
    private SerializedProperty indexcharacterLeft;
    private SerializedProperty indexCharacter;
    private SerializedProperty indexExpression;
    private SerializedProperty indexEmotion;

    private SerializedProperty text;

    // character name dropdown variables
    private string[] characterNames;
    private int[] characterNamesInt;
    // character expression dropdown variables
    private string[] expressionNames;
    private int[] expressionNamesInt;
    // emotion dropdown variables
    private string[] emotionNames;
    private int[] emotionNamesInt;

    private GUIStyle textAreaStyle;
    private float textAreaHeight;
    private float textAreaWidth;

    // icons
    private GUIContent up;
    private GUIContent down;

    private void OnEnable()
    {
        // set dialogue reference
        dialogue = target as SODialogue;
        dialogueObj = new SerializedObject(target);

        // setup editor variables
        dialogueCharacters = dialogueObj.FindProperty("Characters");
        dialogueNodes = dialogueObj.FindProperty("Nodes");

        // setup speaker dropdown
        UpdateCharacterFocusDropdown();
        UpdateExpressionDropdown();
        UpdateEmotionDropdown();

        // setup text area size
        textAreaHeight = (EditorGUIUtility.singleLineHeight * 4) + (EditorGUIUtility.standardVerticalSpacing * 3);
        textAreaWidth = textAreaHeight * (880f / 180f);

        // setup buttons up and down
        up = EditorGUIUtility.IconContent("d_scrollup");
        down = EditorGUIUtility.IconContent("d_scrolldown");
    }


    public override void OnInspectorGUI()
    {
        if (textAreaStyle == null)
        {
            textAreaStyle = new GUIStyle(EditorStyles.textArea);
            textAreaStyle.padding = new RectOffset(10, 5, 10, 5);
        }

        base.OnInspectorGUI();

        // setup object for modifications
        dialogueObj.Update();

        // List of available characters
        EditorGUI.BeginChangeCheck();

        UpdateCharacterFocusDropdown();

        // GO THROUGH NODES AND RENDER THEM
        for (int i = 0; i < dialogueNodes.arraySize; i++)
        {
            // Add Node on top of this Node
            if (GUILayout.Button("+", GUILayout.Height(20)))
            {
                dialogueNodes.InsertArrayElementAtIndex(i);
                ClearSelection();
                break;
            }

            // NODE START
            EditorGUILayout.Space(5);
            EditorGUILayout.BeginHorizontal(GUILayout.Width(textAreaWidth + 40));

            // dialogue node render
            DialogueNode(i);

            // remove Node and end loop
            if (GUILayout.Button("x", GUILayout.Width(20)))
            {
                dialogueNodes.DeleteArrayElementAtIndex(i);
                ClearSelection();
                break;
            }

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space(5);
            // NODE END

        }

        //  Add Node at the BOTTOM of list
        if (GUILayout.Button("+", GUILayout.Height(20)))
        {
            dialogueNodes.InsertArrayElementAtIndex(dialogueNodes.arraySize);
            ClearSelection();
        }

        // save object changes
        dialogueObj.ApplyModifiedProperties();
    }


    // display a dialogue node
    private void DialogueNode(int index)
    {
        // get node properties
        indexcharacterRight = dialogueNodes.GetArrayElementAtIndex(index).FindPropertyRelative("IndexCharacterRight");
        indexcharacterLeft = dialogueNodes.GetArrayElementAtIndex(index).FindPropertyRelative("IndexCharacterLeft");
        indexCharacter = dialogueNodes.GetArrayElementAtIndex(index).FindPropertyRelative("IndexCharacterFocus");
        indexExpression = dialogueNodes.GetArrayElementAtIndex(index).FindPropertyRelative("IndexExpression");
        indexEmotion = dialogueNodes.GetArrayElementAtIndex(index).FindPropertyRelative("IndexEmotion");
        text = dialogueNodes.GetArrayElementAtIndex(index).FindPropertyRelative("Text");

        // Move Node UP AND DOWN
        EditorGUILayout.BeginVertical(GUILayout.Width(20));
        if (index > 0)
        {
            if (GUILayout.Button(up))
                dialogueNodes.MoveArrayElement(index, index - 1);
        }
        if (index < dialogueNodes.arraySize - 1)
        {
            if (GUILayout.Button(down))
                dialogueNodes.MoveArrayElement(index, index + 1);
        }
        EditorGUILayout.EndVertical();

        // Node Information
        EditorGUILayout.BeginVertical();

        // Set Characters on screen
        EditorGUILayout.BeginHorizontal();
        indexcharacterLeft.intValue = EditorGUILayout.IntPopup(indexcharacterLeft.intValue, characterNames, characterNamesInt, GUILayout.Height(20));
        indexcharacterRight.intValue = EditorGUILayout.IntPopup(indexcharacterRight.intValue, characterNames, characterNamesInt, GUILayout.Height(20));
        EditorGUILayout.EndHorizontal();

        // Set Character
        indexCharacter.intValue = EditorGUILayout.IntPopup(indexCharacter.intValue, characterNames, characterNamesInt, GUILayout.Height(20));

        // Set Expression if focus is not on None
        if (indexCharacter.intValue != 0)
        {
            EditorGUILayout.BeginHorizontal();
            indexExpression.intValue = EditorGUILayout.IntPopup(indexExpression.intValue, expressionNames, expressionNamesInt, GUILayout.Height(20));
            indexEmotion.intValue = EditorGUILayout.IntPopup(indexEmotion.intValue, emotionNames, emotionNamesInt, GUILayout.Height(20));
            EditorGUILayout.EndHorizontal();
        }

        // set text
        text.stringValue = EditorGUILayout.TextArea(text.stringValue, textAreaStyle, GUILayout.Width(textAreaWidth), GUILayout.Height(textAreaHeight));

        EditorGUILayout.EndVertical();
    }

    private void ClearSelection()
    {
        GUIUtility.keyboardControl = 0;
        GUIUtility.hotControl = 0;
    }


    private void UpdateCharacterFocusDropdown()
    {
        characterNames = new string[dialogueCharacters.arraySize + 2];
        characterNamesInt = new int[dialogueCharacters.arraySize + 2];

        characterNames[0] = "None";
        characterNamesInt[0] = 0;
        characterNames[1] = "Player";
        characterNamesInt[1] = 1;

        if (characterNames.Length > 1)
        {
            for (int i = 0; i < dialogueCharacters.arraySize; i++)
                characterNames[i + 2] = dialogue.Characters[i].CharacterName;
            for (int i = 2; i < characterNamesInt.Length; i++)
                characterNamesInt[i] = i;
        }
    }
    private void UpdateExpressionDropdown()
    {
        expressionNames = new string[5];
        expressionNamesInt = new int[5];

        // set index of dropdown menu
        for (int i = 0; i < expressionNamesInt.Length; i++)
            expressionNamesInt[i] = i;

        expressionNames[0] = "Neutral";
        expressionNames[1] = "Happy";
        expressionNames[2] = "Angry";
        expressionNames[3] = "Sad";
        expressionNames[4] = "Surprised";
    }

    private void UpdateEmotionDropdown()
    {
        emotionNames = new string[4];
        emotionNamesInt = new int[4];

        // set index of dropdown menu
        for (int i = 0; i < emotionNamesInt.Length; i++)
            emotionNamesInt[i] = i;

        emotionNames[0] = "None";
        emotionNames[1] = "SweatDrop";
        emotionNames[2] = "Veins";
        emotionNames[3] = "Dots";
    }
}
