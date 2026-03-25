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

    private SOEvent eventSelected;
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


    public void EventStart(SOEvent eventToStart) 
    {
        if (eventSelected != null)
            Debug.LogError("Event already selected. There cannot be two events active at the same time!");

        eventSelected = eventToStart;

        // FSM event
        if (eventSelected is SOEventFSM eventFSM)
        {
            // instantiate event
            eventFSMInstance = Instantiate(eventFSM.EventFSM, EventObjectParent).GetComponent<PlayMakerFSM>();
            // start FSM
            eventFSMInstance.SendEvent("EVENT_START");
        }

        // show event background
        EventScreen.SetActive(true);
        EventBackground.sprite = eventToStart.EventLocation.BackgroundSprite;
    }
    public void EventFinish() 
    {
        // hide event background
        EventScreen.SetActive(false);

        // reset event selected reference
        eventSelected = null;

        // reset event FSM instance reference and destroy
        if (eventFSMInstance != null)
        {
            Destroy(eventFSMInstance);
            eventFSMInstance = null;
        }

        // send event end trigger
        OnEnventFinish?.Invoke();

        Debug.Log("Event End");
    }
}
