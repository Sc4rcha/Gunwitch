using System;
using System.Collections.Generic;
using System.Linq;
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

    public void EventStart(SOEvent eventSelected)
    {
        // Player Setup
        ManagerGameElements.Instance.Player.EventStart();

        if (eventFSMInstance != null)
            Debug.LogError("Event already instantiated. There cannot be two events active at the same time!");

        // start event depending on type
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
            ManagerGameElements.Instance.CombatLoad(eventCombat.Encounter);
            ManagerGameElements.Instance.OnCombatFinish += EventFinish;
        }


        // Add and Remove evets
        EventAddList(eventSelected.EventsAdd);
        EventRemoveList(eventSelected.EventsRemove);
        if (!eventSelected.IsPersistent)
            EventRemove(eventSelected);

        // show event background
        EventScreen.SetActive(true);
        EventBackground.sprite = eventSelected.EventLocation.BackgroundSprite;
    }
    public void EventFinish(bool isEventPass)
    {
        // clear combat finish event if the event was an SOCombatEvent
        ManagerGameElements.Instance.OnCombatFinish -= EventFinish;

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
}
