using GameInfo;
using System.Collections.Generic;
using UnityEngine;

public class ManagerQuest : MonoBehaviour
{
    public ManagerEvents ManagerEvents;
    public ManagerMap ManagerMap;

    public SOQuest CurrentQuest { get; private set; }
    public Dictionary<SOEvent, EventState> QuestEvents { get; private set; }
    public MapState MapState { get; private set; }


    public void Setup() 
    {
        // setup event dictionary
        QuestEvents = new Dictionary<SOEvent, EventState>();

        ManagerMap.Setup();
    }


    public void QuestStart(SOQuest quest) 
    {
        CurrentQuest = quest;

        // Setup map state
        MapState = new MapState();

        // load map
        ManagerMap.MapEnter(quest.QuestMap);
        ManagerMap.Refresh(this);

        // add map events
        if (quest.ActiveEvents != null)
            EventAddList(quest.ActiveEvents.Events);

        // enter initial location
        if (quest.InitialLocation != null)
            LocationEnter(quest.InitialLocation);

        // trigger initial event
        if (quest.InitialEvent != null)
            EventStart(CurrentQuest.InitialEvent);

    }
    public void QuestFinish() 
    {
        CurrentQuest = null;

        // clear map state
        MapState = new MapState();

        // exit map
        ManagerMap.MapExit();
    }

    public void LoadQuest(SaveDataWorld worldSaveSave) 
    {
        // Add saved events
        foreach (var savedEvent in worldSaveSave.Events)
            QuestEvents.Add(CurrentQuest.EventsAll.GetEventReference(savedEvent.Id), savedEvent.State);

        // map state
        MapState = new MapState();

        // load map state flags
        MapState.Flags = new FlagList(worldSaveSave.Flags);
        // load current world location
        if (worldSaveSave.CurrentLocation != "")
            LocationEnter(CurrentQuest.AllLocations.GetLocationReference(worldSaveSave.CurrentLocation));

        // set visibility of locations to match saved data
        foreach (var locationSaveData in worldSaveSave.Locations)
        {
            foreach (var worldLocation in ManagerMap.Map.Locations)
            {
                if (worldLocation.LocationReference.Id == locationSaveData.Id)
                    worldLocation.ChangeVisibility(locationSaveData.Visibility);
            }
        }
    }



    #region QUEST Locations
    public void LocationEnter(SOLocation location)
    {
        MapState.WorldLocation = location;

        ManagerMap.LocationEnter(MapState.WorldLocation, GetLocationEvents());
    }
    public void LocationExit() 
    {
        MapState.WorldLocation = null;

        ManagerMap.LocationExit();
        ManagerMap.Refresh(this);
    }

    // called by location button to open a new location in the overworld
    public void ButtonLocation(SOLocation location)
    {
        LocationEnter(location);
    }
    #endregion

    #region QUEST Events
    public void EventAddList(SOEvent[] list)
    {
        foreach (var newEvent in list)
            EventAdd(newEvent);
    }
    public void EventAdd(SOEvent eventToAdd)
    {
        if (!QuestEvents.ContainsKey(eventToAdd))
            QuestEvents.Add(eventToAdd, new EventState(eventToAdd.CheckLocked(MapState.Flags)));

        ManagerMap.Refresh(this);
    }
    public void EventDeactivateList(SOEvent[] list)
    {
        foreach (var newEvent in list)
            EventDeactivate(newEvent);
    }
    public void EventDeactivate(SOEvent eventToRemove)
    {
        if (QuestEvents.ContainsKey(eventToRemove))
            QuestEvents[eventToRemove].IsActive = false;
    }

    public void EventsClear()
    {
        QuestEvents.Clear();
    }

    public void EventStart(SOEvent eventToStart) 
    {
        if (QuestEvents.ContainsKey(eventToStart))
            QuestEvents[eventToStart].IsComplete = !eventToStart.IsPersistent;

        ManagerEvents.EventStart(eventToStart);
    }
    public void EventFinish() 
    {
        ManagerEvents.EventFinish();

        // unlock events
        foreach (var eventToUnlock in QuestEvents)
        {
            if (eventToUnlock.Value.IsLocked)
                eventToUnlock.Value.IsLocked = eventToUnlock.Key.CheckLocked(MapState.Flags);
        }

        ManagerMap.LocationScreen.Refresh(MapState.WorldLocation, GetLocationEvents());
    }

    /// <summary>
    /// starts the first autoplay event in the location
    /// </summary>
    /// <param name="location"></param>
    /// <returns></returns>
    public void StartAutoplayEvent()
    {
        if (MapState.WorldLocation == null)
            return;

        foreach (var locationEvent in GetLocationEvents(MapState.WorldLocation))
        {
            if (locationEvent.IsAutoplay)
            {
                EventStart(locationEvent);
                break;
            }
        }
    }

    public EventState CheckEventState(SOEvent eventToCheck)
    {
        if (QuestEvents.ContainsKey(eventToCheck))
            return QuestEvents[eventToCheck];

        return null;
    }
    /// <summary>
    /// Get active events for a specific location
    /// </summary>
    /// <param name="location"></param>
    /// <returns></returns>
    public SOEvent[] GetLocationEvents()
    {
        List<SOEvent> locationEvents = new List<SOEvent>();
        foreach (var eventToCheck in QuestEvents)
        {
            if (eventToCheck.Value.IsAvailable && eventToCheck.Key.EventLocation == MapState.WorldLocation)
                locationEvents.Add(eventToCheck.Key);
        }

        return locationEvents.ToArray();
    }
    public SOEvent[] GetLocationEvents(SOLocation location)
    {
        List<SOEvent> locationEvents = new List<SOEvent>();
        foreach (var eventToCheck in QuestEvents)
        {
            if (eventToCheck.Value.IsAvailable && eventToCheck.Key.EventLocation == location)
                locationEvents.Add(eventToCheck.Key);
        }

        return locationEvents.ToArray();
    }
    #endregion
}
