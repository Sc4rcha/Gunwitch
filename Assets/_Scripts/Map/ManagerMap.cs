using System.Linq;
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

    private ManagerEvents managerEvents;
    private Map map;


    public void Setup() 
    {
        // setup Screen
        LocationScreen.Setup(this);
        LocationScreen.Exit();

        // hide map screen
        gameObject.SetActive(false);

        // get event manager reference
        managerEvents = ManagerGameElements.Instance.ManagerEvents;
    }

    // Load a new map
    public void MapOpen(Map newMap) 
    {
        // destroy previous map
        if (map != null)
            Destroy(map.gameObject);

        // instantiate map
        map = Instantiate(newMap, MapHolder).GetComponent<Map>();

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
    public void MapClose() 
    {
        // hide map screen
        gameObject.SetActive(false);

        // destroy instanced map
        Destroy(map.gameObject);
    }


    // refresh map (check location availability)
    public void Refresh() 
    {
        foreach (var location in map.Locations)
        {
            // Activate location if is persistent, deactivate if not
            location.gameObject.SetActive(location.LocationInfo.IsPersistent);

            // activate location if any event is taking place there
            if (managerEvents.GetLocationEvents(location.LocationInfo).Length > 0)
                location.gameObject.SetActive(true);
        }
    }


    #region Locations
    // Set location screen Information
    public void LocationSetInfo(SOLocation locationInfo) 
    {
        LocationScreen.SetInfo(locationInfo);
    }
    // enter location screen
    public void LocationEnter() 
    {
        LocationScreen.Enter();
    }
    // exit location screen
    public void LocationExit()
    {
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
}
