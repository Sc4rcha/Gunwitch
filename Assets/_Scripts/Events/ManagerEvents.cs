using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ManagerEvents : MonoBehaviour
{
    public Transform EventObjectParent;
    public GameObject EventScreen;
    public Image EventBackground;

    public List<SOEvent> ActiveEvents { get; private set; }

    private PlayMakerFSM eventFSMInstance;

    public event Action OnEnventFinish;
    public event Action<bool> OnCombatFinish;

    public void Setup() 
    {
        // setup active events list
        ActiveEvents = new List<SOEvent>();
    }


    #region Add remove Events
    public void EventAddList(SOEvent[] list) 
    {
        foreach (var newEvent in list)
            ActiveEvents.Add(newEvent);
    }
    public void EventAdd(SOEvent eventToAdd) 
    {
        ActiveEvents.Add(eventToAdd);
    }
    public void EventRemove(SOEvent eventToRemove) 
    {
        ActiveEvents.Remove(eventToRemove);
    }
    #endregion

    /// <summary>
    /// Get active events for a specific location
    /// </summary>
    /// <param name="location"></param>
    /// <returns></returns>
    public SOEvent[] GetLocationEvents(SOLocation location) 
    {
        return ActiveEvents.Where(e => e.EventLocation == location).ToArray();
    }

    public void EventStart(SOEvent eventSelected)
    {
        // Player Setup
        ManagerGameElements.Instance.Player.EventStart();

        if (eventFSMInstance != null)
            Debug.LogError("Event already instantiated. There cannot be two events active at the same time!");

        if (eventSelected is SOEventFSM eventFSM) 
        {
            // instantiate event object
            eventFSMInstance = Instantiate(eventFSM.FSM, EventObjectParent).GetComponent<PlayMakerFSM>();
            // start FSM
            eventFSMInstance.SendEvent("EVENT_START");
        }
        else if (eventSelected is SOEventCombat eventCombat)
        {
            // start combat
            CombatStart(eventCombat.Encounter);
            OnCombatFinish += EventFinish;
        }

        // show event background
        EventScreen.SetActive(true);
        EventBackground.sprite = eventSelected.EventLocation.BackgroundSprite;
    }
    public void EventFinish(bool isEventPass)
    {
        // hide event background
        EventScreen.SetActive(false);

        // reset event FSM instance reference and destroy
        if (eventFSMInstance != null)
        {
            Destroy(eventFSMInstance);
            eventFSMInstance = null;
        }

        // send event end trigger
        OnEnventFinish?.Invoke();

        // Player Setup
        ManagerGameElements.Instance.Player.EventFinish();


        // game over screen if player fails event
        if (!isEventPass)
        {
        }
    }

    #region Combat
    private CombatEnounter encounterReference;
    public void CombatStart(CombatEnounter encounterReference) 
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
