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
    public ManagerEvents ManagerEvents;
    public ManagerQuests ManagerQuests;
    public ManagerMap ManagerMap;
    public ManagerDialogue ManagerDialogue;
    [Space]
    public ManagerPlayer Player;
    public InventoryItemReferences ItemReferences;
    [Space]
    public GameSaveLoad SaveLoad;

    public SOQuest CurrentQuest { get; private set; }


    private void Setup()
    {
        // setup item references
        ItemReferences.Setup();

        // setup dialogue
        ManagerDialogue.Setup();

        // setup event manager
        ManagerEvents.Setup(ManagerMap);
        // setup map manager
        ManagerMap.Setup(ManagerEvents);


        // setup player
        Player.Info = StartingPlayer.GetPlayer();
        Player.Setup();

        // setup quest manager
        ManagerQuests.Setup();
        ManagerQuests.Show(true);

        // show cursor
        Cursor.CursorShow(true);
    }


    public void QuestStart(SOQuest quest) 
    {
        CurrentQuest = quest;

        ManagerMap.QuestStart(CurrentQuest);
        ManagerEvents.QuestStart(CurrentQuest);

        ManagerEvents.EventStart(CurrentQuest.InitialEvent);

        ManagerQuests.Show(false);
    }
    public void QuestFinish() 
    {
        ManagerMap.MapExit();
        ManagerEvents.EventsClear();

        ManagerQuests.Show(true);
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
