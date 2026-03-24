using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ManagerEvents : MonoBehaviour
{
    public List<SOEvent> ActiveEvents { get; private set; }

    private SOEvent eventSelected;

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
    public void EventAdd(SOEvent eventToAdd) { }
    public void EventRemove(SOEvent eventToRemove) { }
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
        eventSelected = eventToStart;
    }
    public void EventEnd() 
    {
        Debug.Log("Event End");
    }
}
