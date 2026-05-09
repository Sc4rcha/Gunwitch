using GameInfo;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManagerMap : MonoBehaviour
{
    [Header ("Scene References")]
    public ScrollRect MapScrollRect;
    public Transform MapHolder;
    public TMPro.TMP_Text MapName;
    [Space]
    public LocationScreen LocationScreen;
    public GameObject ButtonFinishQuest;

    public MapState MapState { get; private set; }
    public Map Map { get; private set; }


    private ManagerEvents managerEvents;


    public void Setup(ManagerEvents managerEvents) 
    {
        // Setup map state
        MapState = new MapState();

        // get event manager reference
        this.managerEvents = managerEvents;

        // setup 
        LocationScreen.Setup(managerEvents, this);
        LocationScreen.Exit();

        // hide map screen
        gameObject.SetActive(false);
    }

    public void LoadSaveData(SaveDataWorld worldSaveSave) 
    {
        // load map state flags
        MapState = new MapState(worldSaveSave.Flags);
        // load current world location
        MapState.WorldLocation = ManagerGameElements.Instance.CurrentQuest.AllLocations.GetLocationReference(worldSaveSave.CurrentLocation);
        LocationSetInfo(MapState.WorldLocation);
        LocationEnter();

        // set visibility of locations to match saved data
        foreach (var locationSaveData in worldSaveSave.Locations)
        {
            foreach (var worldLocation in Map.Locations)
            {
                if (worldLocation.LocationReference.Id == locationSaveData.Id)
                    worldLocation.ChangeVisibility(locationSaveData.Visibility);
            }       
        }
    }


    // Load a new map
    public void QuestStart(SOQuest quest) 
    {
        // destroy previous map
        if (Map != null)
            Destroy(Map.gameObject);

        // instantiate map
        Map = Instantiate(quest.QuestMap, MapHolder).GetComponent<Map>();
        Map.Setup();

        // Set map movement scroll rect
        MapScrollRect.content = Map.GetComponent<RectTransform>();
        // set map name
        MapName.text = Map.Name;

        // setup location buttons
        foreach (var locationButton in Map.Locations)
            locationButton.Setup(this);

        // show map screen
        gameObject.SetActive(true);
        // hide finish quest button
        ButtonFinishQuest.SetActive(false);

        // refresh map at instantiation
        Refresh();

        // enter starting location
        if (quest.InitialLocation != null)
        {
            LocationSetInfo(quest.InitialLocation);
            LocationEnter();
        }
    }
    public void MapExit() 
    {
        // Clear map state information on exit map
        MapState = new MapState();

        // hide map screen
        gameObject.SetActive(false);

        // destroy instanced map
        Destroy(Map.gameObject);
    }


    // refresh map (check location availability)
    public void Refresh() 
    {
        Debug.Log("Refresh Map");

        foreach (var location in Map.Locations)
        {
            switch (location.Visibility)
            {
                case LocationVisibilityType.Normal:
                    // activate location if any event is taking place there
                    location.gameObject.SetActive(managerEvents.GetLocationEvents(location.LocationReference).Length > 0);
                    break;
                case LocationVisibilityType.ForceHide:
                    location.gameObject.SetActive(false);
                    break;
                case LocationVisibilityType.ForceShow:
                    location.gameObject.SetActive(true);
                    break;
            }
        }
    }


    #region Location Screen
    // Set location screen Information
    public void LocationSetInfo(SOLocation locationInfo) 
    {
        MapState.WorldLocation = locationInfo;

        LocationScreen.Refresh();
    }
    // enter location screen
    public void LocationEnter() 
    {
        LocationScreen.Enter();
    }
    // exit location screen
    public void LocationExit()
    {
        MapState.WorldLocation = null;

        LocationScreen.Exit();

        Refresh();
    }
    public void LocationLock(bool isLocked) 
    {
        LocationScreen.ExitButton.SetActive(!isLocked);
    }
    // called by location button to open a new location in the overworld
    public void ButtonLocation(SOLocation locationInfo) 
    {
        LocationSetInfo (locationInfo);
        LocationEnter();
    }
    #endregion

    public void LocationChangeVisibility(SOLocation locationInfo, LocationVisibilityType newVisibility)
    {
        foreach (var locationButton in Map.Locations)
        {
            if (locationButton.LocationReference == locationInfo)
                locationButton.ChangeVisibility(newVisibility);
        }
    }
}

