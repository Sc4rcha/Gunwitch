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
    public ManagerEvents ManagerEvents;
    public ManagerQuests ManagerQuests;
    public ManagerMap ManagerMap;
    public ManagerDialogue ManagerDialogue;
    [Space]
    public ManagerPlayer Player;
    public InventoryItemReferences ItemReferences;

    private void Setup()
    {
        // setup item references
        ItemReferences.Setup();

        // setup dialogue
        ManagerDialogue.Setup();

        // setup event manager
        ManagerEvents.Setup();

        // setup map manager
        ManagerMap.Setup();

        // setup player
        Player.Info = StartingPlayer.GetPlayer();
        Player.Setup();

        // setup quest manager
        ManagerQuests.Setup();
        ManagerQuests.Show(true);
    }

    public void QuestStart(SOQuest quest) 
    {
        ManagerMap.MapOpen(quest.QuestMap);
        ManagerEvents.EventStart(quest.InitialEvent);

        ManagerQuests.Show(false);
    }
    public void QuestFinish() 
    {
        ManagerMap.MapClose();
        ManagerEvents.EventsClear();

        ManagerQuests.Show(true);
    }

    #region Combat
    public event Action<bool> OnCombatFinish;
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
    public void CombatEnd(bool isPlayerWin)
    {
        // unload combat scene
        StartCoroutine(ExitCombat());

        // send event for player winning
        OnCombatFinish?.Invoke(isPlayerWin);

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
