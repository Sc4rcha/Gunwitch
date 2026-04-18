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

    private Map map;


    public void Setup() 
    {
        // setup Screen
        LocationScreen.Setup(this);
        LocationScreen.Exit();

        // hide map screen
        gameObject.SetActive(false);
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
            ManagerGameElements.Instance.ManagerEvents.EventAddList(map.MapEvents.Events);

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
        // get list of active events
        SOEvent[] activeEvents = ManagerGameElements.Instance.ManagerEvents.ActiveEvents.ToArray();

        foreach (var location in map.Locations)
        {
            // Activate location if is persistent, deactivate if not
            location.gameObject.SetActive(location.LocationInfo.IsPersistent);

            foreach (var activeEvent in activeEvents)
            {
                // reactivate location if an event is taking place there
                if (location.LocationInfo == activeEvent.EventLocation)
                {
                    location.gameObject.SetActive(true);
                    break;
                }
            }
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

    // called by location button to open a new location in the overworld
    public void ButtonLocation(SOLocation locationInfo) 
    {
        LocationSetInfo (locationInfo);
        LocationEnter();
    }
    #endregion
}
