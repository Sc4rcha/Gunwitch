using GameInfo;
using System;
using System.Collections.Generic;
using System.Linq;
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
    private List<SOEvent> activeEvents;
    private List<SOEvent> lockedEvents;

    private PlayMakerFSM eventFSMInstance;

    public void Setup() 
    {
        // setup active events list
        activeEvents = new List<SOEvent>();
        lockedEvents = new List<SOEvent>();

        // setup flags for the game
        EventFlags = new Flags();

        // check to unlock events when event finishes
        OnEnventFinish += CheckLockedEvents;
    }


    #region Add remove Events
    public void EventAddList(SOEvent[] list) 
    {
        foreach (var newEvent in list)
            EventAdd(newEvent);
    }
    public void EventAdd(SOEvent eventToAdd) 
    {
        if (eventToAdd.CheckUnlocked(EventFlags))
        {
            if (!activeEvents.Contains(eventToAdd))
                activeEvents.Add(eventToAdd);
        }
        else
        {
            if (!lockedEvents.Contains(eventToAdd))
                lockedEvents.Add(eventToAdd);
        }
    }
    public void EventRemoveList(SOEvent[] list)
    {
        foreach (var newEvent in list)
            EventRemove(newEvent);
    }
    public void EventRemove(SOEvent eventToRemove) 
    {
        if (activeEvents.Contains (eventToRemove))
            activeEvents.Remove(eventToRemove);
        if (lockedEvents.Contains(eventToRemove))
            lockedEvents.Remove(eventToRemove);
    }
    public void EventsClear() 
    {
        activeEvents.Clear();
    }
    #endregion


    /// <summary>
    /// Get active events for a specific location
    /// </summary>
    /// <param name="location"></param>
    /// <returns></returns>
    public SOEvent[] GetLocationEvents(SOLocation location) 
    {
        return activeEvents.Where(e => e.EventLocation == location).ToArray();
    }


    #region EVENT START
    public void EventStart(SOEvent eventSelected)
    {
        // Player Setup
        ManagerGameElements.Instance.Player.EventStart();

        // remove event from list if not persistent
        if (!eventSelected.IsPersistent)
            EventRemove(eventSelected);

        // execute event actions
        foreach (var action in eventSelected.EventActions)
            action.Execute(this);

        // show Location background
        EventScreen.SetActive(eventSelected.EventLocation != null);
        if (eventSelected.EventLocation != null)
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

        // game over screen if player fails event
        if (!isEventPass)
        {
        }
    }
    #endregion

    public void InstantiateFSM(PlayMakerFSM fsm)
    {
        // instantiate event object
        eventFSMInstance = Instantiate(fsm, EventObjectParent);
        // start FSM
        eventFSMInstance.SendEvent("EVENT_START");
    }

    public void CheckLockedEvents() 
    {
        SOEvent lockedEvent;
        for (int i = lockedEvents.Count - 1; i >= 0; i--)
        {
            lockedEvent = lockedEvents[i];
            // if event meets unlock conditions remove from locked events and add to active events
            if (lockedEvent.CheckUnlocked(EventFlags))
            {
                EventRemove(lockedEvent);
                EventAdd(lockedEvent);
            }
        }
    }
}
