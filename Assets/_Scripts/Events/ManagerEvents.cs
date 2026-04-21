using GameInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UI;

public class ManagerEvents : MonoBehaviour
{
    public Transform EventObjectParent;
    public GameObject EventScreen;
    public Image EventBackground;

    public List<SOEvent> ActiveEvents { get; private set; }

    private PlayMakerFSM eventFSMInstance;

    public event Action OnEnventFinish;

    public void Setup() 
    {
        // setup active events list
        ActiveEvents = new List<SOEvent>();
    }


    #region Add remove Events
    public void EventAddList(SOEvent[] list) 
    {
        foreach (var newEvent in list)
            EventAdd(newEvent);
    }
    public void EventAdd(SOEvent eventToAdd) 
    {
        if (!ActiveEvents.Contains (eventToAdd))
            ActiveEvents.Add(eventToAdd);
    }
    public void EventRemoveList(SOEvent[] list)
    {
        foreach (var newEvent in list)
            EventRemove(newEvent);
    }
    public void EventRemove(SOEvent eventToRemove) 
    {
        if (ActiveEvents.Contains (eventToRemove))
            ActiveEvents.Remove(eventToRemove);
    }
    public void EventsClear() 
    {
        ActiveEvents.Clear();
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


    #region EVENT START
    public void EventStart(SOEvent eventSelected)
    {
        // Player Setup
        ManagerGameElements.Instance.Player.EventStart();

        // remove event from list if not persistent
        if (!eventSelected.IsPersistent)
            EventRemove(eventSelected);

        // execute event actions
        EventContext context = new EventContext(this);
        foreach (var action in eventSelected.EventActions)
            action.Execute(context);

        // show Location background
        EventScreen.SetActive(eventSelected.EventLocation != null);
        if (eventSelected.EventLocation != null)
            EventBackground.sprite = eventSelected.EventLocation.BackgroundSprite;
    }
    public void EventFSM(PlayMakerFSM fsm) 
    {
        // instantiate event object
        eventFSMInstance = Instantiate(fsm, EventObjectParent);
        // start FSM
        eventFSMInstance.SendEvent("EVENT_START");
    }
    /// <summary>
    /// Event finish for events you cannot FAIL
    /// </summary>
    public void EventFinish() 
    {
        EventFinish(true);
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
    #endregion
}
