using GameInfo;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManagerEvents : MonoBehaviour
{
    [Header ("Scene References")]
    public Transform EventObjectParent;
    public GameObject EventScreen;
    public Image EventBackground;

    public Flags EventFlags;

    public event Action OnEnventFinish;

    // event lists
    private Dictionary<SOEvent, EventState> allEvents;

    private PlayMakerFSM eventFSMInstance;

    public void Setup() 
    {
        // setup dictionary events
        allEvents = new Dictionary<SOEvent, EventState>();

        // setup flags for the game
        EventFlags = new Flags();

        // check to unlock events when event finishes
        OnEnventFinish += EventUnlockCheck;
    }


    #region Event States
    public void EventAddList(SOEvent[] list) 
    {
        foreach (var newEvent in list)
            EventAdd(newEvent);
    }
    public void EventAdd(SOEvent eventToAdd) 
    {
        if (!allEvents.ContainsKey(eventToAdd))
            allEvents.Add(eventToAdd, new EventState(eventToAdd.CheckLocked(EventFlags)));
    }
    public void EventDeactivateList(SOEvent[] list)
    {
        foreach (var newEvent in list)
            EventDeactivate(newEvent);
    }
    public void EventDeactivate(SOEvent eventToRemove) 
    {
        if (allEvents.ContainsKey(eventToRemove))
            allEvents[eventToRemove].IsActive = false;
    }
    public void EventComplete(SOEvent eventToComplete) 
    {
        if (allEvents.ContainsKey(eventToComplete))
            allEvents[eventToComplete].IsComplete = true;
    }
    public void EventsClear() 
    {
        allEvents.Clear();
    }
    public void EventUnlockCheck() 
    {
        foreach (var eventToUnlock in allEvents)
        {
            if (eventToUnlock.Value.IsLocked)
                eventToUnlock.Value.IsLocked = eventToUnlock.Key.CheckLocked(EventFlags);
        }
    }
    #endregion

    public EventState CheckEventState(SOEvent eventToCheck) 
    {
        if (allEvents.ContainsKey (eventToCheck))
            return allEvents[eventToCheck];

        return null;
    }

    /// <summary>
    /// Get active events for a specific location
    /// </summary>
    /// <param name="location"></param>
    /// <returns></returns>
    public SOEvent[] GetLocationEvents(SOLocation location) 
    {
        List <SOEvent> locationEvents = new List <SOEvent>();
        foreach (var eventToCheck in allEvents)
        {
            if (eventToCheck.Value.IsAvailable && eventToCheck.Key.EventLocation == location)
                locationEvents.Add(eventToCheck.Key);
        }

        return locationEvents.ToArray();
    }


    #region EVENT START
    public void EventStart(SOEvent eventSelected)
    {
        // Player Setup
        ManagerGameElements.Instance.Player.EventStart();

        // complete event if is not persistent
        if (!eventSelected.IsPersistent)
            EventComplete(eventSelected);

        // execute event actions
        foreach (var action in eventSelected.EventActions)
            action.Execute(this);

        // show Location background
        EventScreen.SetActive(eventSelected.EventLocation != null);
        if (eventSelected.EventLocation != null)
            EventBackground.sprite = eventSelected.EventLocation.BackgroundSprite;
    }
    public void EventFinish()
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
    }
    #endregion


    public void InstantiateFSM(PlayMakerFSM fsm)
    {
        // instantiate event object
        eventFSMInstance = Instantiate(fsm, EventObjectParent);
        // start FSM
        eventFSMInstance.SendEvent("EVENT_START");
    }
}
