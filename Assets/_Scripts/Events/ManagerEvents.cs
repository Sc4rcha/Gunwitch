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

    public event Action OnEnventFinish;

    // event lists
    public Dictionary<SOEvent, EventState> AddedEvents { get; private set; }


    private SOEvent eventSelected;
    private PlayMakerFSM eventFSMInstance;

    private ManagerMap map;

    public void Setup(ManagerMap map) 
    {
        this.map = map;

        // setup dictionary events
        AddedEvents = new Dictionary<SOEvent, EventState>();

        // check to unlock events when event finishes
        OnEnventFinish += EventUnlockCheck;
    }

    public void LoadSaveFile(SaveDataWorld worldSaveSave) 
    {
        // clear dictionary
        AddedEvents = new Dictionary<SOEvent, EventState>();

        // add saved events
        foreach (var savedEvent in worldSaveSave.Events)
            AddedEvents.Add(ManagerGameElements.Instance.CurrentQuest.EventsAll.GetEventReference(savedEvent.Id), savedEvent.State);
    }


    public void QuestStart(SOQuest quest) 
    {
        // add map events
        if (quest.ActiveEvents != null)
            EventAddList(quest.ActiveEvents.Events);
    }

    #region Event States
    public void EventAddList(SOEvent[] list) 
    {
        foreach (var newEvent in list)
            EventAdd(newEvent);
    }
    public void EventAdd(SOEvent eventToAdd) 
    {
        if (!AddedEvents.ContainsKey(eventToAdd))
            AddedEvents.Add(eventToAdd, new EventState(eventToAdd.CheckLocked(map.MapState.Flags)));

        ManagerGameElements.Instance.ManagerMap.Refresh();
    }
    public void EventDeactivateList(SOEvent[] list)
    {
        foreach (var newEvent in list)
            EventDeactivate(newEvent);
    }
    public void EventDeactivate(SOEvent eventToRemove) 
    {
        if (AddedEvents.ContainsKey(eventToRemove))
            AddedEvents[eventToRemove].IsActive = false;
    }
    public void EventComplete(SOEvent eventToComplete) 
    {
        if (AddedEvents.ContainsKey(eventToComplete))
            AddedEvents[eventToComplete].IsComplete = true;
    }
    public void EventsClear() 
    {
        AddedEvents.Clear();
    }
    public void EventUnlockCheck() 
    {
        foreach (var eventToUnlock in AddedEvents)
        {
            if (eventToUnlock.Value.IsLocked)
                eventToUnlock.Value.IsLocked = eventToUnlock.Key.CheckLocked(map.MapState.Flags);
        }
    }
    #endregion


    public EventState CheckEventState(SOEvent eventToCheck) 
    {
        if (AddedEvents.ContainsKey (eventToCheck))
            return AddedEvents[eventToCheck];

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
        foreach (var eventToCheck in AddedEvents)
        {
            if (eventToCheck.Value.IsAvailable && eventToCheck.Key.EventLocation == location)
                locationEvents.Add(eventToCheck.Key);
        }

        return locationEvents.ToArray();
    }


    #region EVENT START
    public void EventStart(SOEvent eventSelected)
    {
        if (this.eventSelected != null)
            Debug.LogError("Event already running");

        // Player Start event
        ManagerGameElements.Instance.Player.EventStart();

        // set event selected reference
        this.eventSelected = eventSelected;

        // complete event if is not persistent
        if (!eventSelected.IsPersistent)
            EventComplete(eventSelected);

        // show Location background
        EventScreen.SetActive(eventSelected.EventLocation != null);
        if (eventSelected.EventLocation != null)
            EventBackground.sprite = eventSelected.EventLocation.BackgroundSprite;

        // start event behaviour if it has one
        if (eventSelected.EventAction != null)
            eventSelected.EventAction.Execute(this);
        //otherwise end event (for events with no behaviour)
        else
            EventFinish();


    }
    public void EventFinish()
    {
        // hide event background
        EventScreen.SetActive(false);

        // reset event FSM instance reference and destroy
        if (eventFSMInstance != null)
        {
            Destroy(eventFSMInstance.gameObject);
            eventFSMInstance = null;
        }

        // execute event actions
        foreach (var action in eventSelected.EventActions)
            action.Execute(this);
        //clear event selected reference
        eventSelected = null;

        // send event end trigger
        OnEnventFinish?.Invoke();

        // Player Finish event
        ManagerGameElements.Instance.Player.EventFinish();

        // start autoplay event
        StartAutoplayEvent();
    }
    /// <summary>
    /// starts the first autoplay event in the location
    /// </summary>
    /// <param name="location"></param>
    /// <returns></returns>
    public void StartAutoplayEvent()
    {
        if (map.MapState.WorldLocation == null)
            return;

        foreach (var locationEvent in GetLocationEvents(map.MapState.WorldLocation))
        {
            if (locationEvent.IsAutoplay)
            {
                EventStart(locationEvent);
                break;
            }
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
}
