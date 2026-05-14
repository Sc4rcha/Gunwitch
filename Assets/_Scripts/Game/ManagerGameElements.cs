using GameInfo;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManagerGameElements : MonoBehaviour
{
    private static ManagerGameElements _instance;
    public static ManagerGameElements Instance { get { return _instance; } }


    private void Awake()
    {
        // singletion initialization
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            Setup();
        }
    }


    public SOPlayerInitialState StartingPlayer;
    [Space]
    public ManagerCursor Cursor;
    public QuestSelector QuestSelector;
    [Space]
    public ManagerDialogue ManagerDialogue;
    public ManagerQuest ManagerQuest;
    [Space]
    public ManagerPlayer Player;
    public InventoryItemReferences ItemReferences;
    [Space]
    public GameSaveLoad SaveLoad;



    private void Setup()
    {
        // setup item references
        ItemReferences.Setup();

        // setup dialogue
        ManagerDialogue.Setup();

        // setup quest manager
        ManagerQuest.Setup();

        // setup player
        Player.Info = StartingPlayer.GetPlayer();
        Player.Setup();

        // setup quest manager
        QuestSelector.Setup();
        QuestSelector.Show(true);

        // show cursor
        Cursor.CursorShow(true);
    }


    public void QuestStart(SOQuest quest) 
    {
        ManagerQuest.QuestStart(quest);

        QuestSelector.Show(false);
    }
    public void QuestFinish() 
    {
        ManagerQuest.QuestFinish();

        QuestSelector.Show(true);
    }


    #region Combat
    public event Action<CombatEndType> OnCombatFinish;
    private CombatEncounter encounterReference;
    public void CombatLoad(CombatEncounter encounterReference)
    {
        // get combat start variables
        this.encounterReference = encounterReference;

        // load combat scene
        SceneManager.LoadScene("Combat", LoadSceneMode.Additive);
    }
    public void CombatRegister(ManagerCombat combat)
    {
        // start combat with stored variables
        combat.CombatStart(encounterReference);
    }
    public void CombatEnd(CombatEndType endType)
    {
        // unload combat scene
        StartCoroutine(ExitCombat());

        // send event for player winning
        OnCombatFinish?.Invoke(endType);

        // refresh PlayerHUD
    }
    private IEnumerator ExitCombat()
    {
        // wait for unload scene
        yield return SceneManager.UnloadSceneAsync("Combat");

        // wait for unload all assets unused by combat scene
        yield return Resources.UnloadUnusedAssets();
        // garbage colleciton
        GC.Collect();
    }
    #endregion
}
