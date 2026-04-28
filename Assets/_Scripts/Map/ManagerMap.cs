using UnityEngine;
using UnityEngine.UI;
using GameInfo;

public class ManagerMap : MonoBehaviour
{
    [Header ("Scene References")]
    public ScrollRect MapScrollRect;
    public Transform MapHolder;
    public TMPro.TMP_Text MapName;
    [Space]
    public LocationScreen LocationScreen;
    public GameObject ButtonFinishQuest;

    public MapState MapState;

    private ManagerEvents managerEvents;
    private Map map;


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

    // Load a new map
    public void MapEnter(Map newMap) 
    {
        // destroy previous map
        if (map != null)
            Destroy(map.gameObject);

        // instantiate map
        map = Instantiate(newMap, MapHolder).GetComponent<Map>();
        map.Setup();

        // Set map movement scroll rect
        MapScrollRect.content = map.GetComponent<RectTransform>();
        // set map name
        MapName.text = map.Name;

        // add map events
        if (map.MapEvents != null)
            managerEvents.EventAddList(map.MapEvents.Events);

        // setup location buttons
        foreach (var locationButton in map.Locations)
            locationButton.Setup(this);

        // show map screen
        gameObject.SetActive(true);
        // hide finish quest button
        ButtonFinishQuest.SetActive(false);

        // refresh map at instantiation
        Refresh();

        // enter starting location
        if (map.InitialLocation != null)
        {
            LocationSetInfo(map.InitialLocation);
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
        Destroy(map.gameObject);
    }


    // refresh map (check location availability)
    public void Refresh() 
    {
        Debug.Log("Refresh Map");

        foreach (var location in map.Locations)
        {
            switch (location.LocationVisibility)
            {
                case SOLocation.LocationVisibilityType.Normal:
                    // activate location if any event is taking place there
                    location.gameObject.SetActive(managerEvents.GetLocationEvents(location.LocationInfo).Length > 0);
                    break;
                case SOLocation.LocationVisibilityType.ForceHide:
                    location.gameObject.SetActive(false);
                    break;
                case SOLocation.LocationVisibilityType.ForceShow:
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

    public void LocationChangeVisibility(SOLocation locationInfo, SOLocation.LocationVisibilityType newVisibility)
    {
        foreach (var locationButton in map.Locations)
        {
            if (locationButton.LocationInfo == locationInfo)
                locationButton.ChangeVisibility(newVisibility);
        }
    }
}

