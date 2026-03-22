using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ManagerOverworld : MonoBehaviour
{
    [Header ("Scene References")]
    public ScrollRect MapScrollRect;
    public Transform MapHolder;
    public TMPro.TMP_Text MapName;
    [Space]
    public LocationScreen LocationScreen;


    private OverworldMap map;


    public void Setup() 
    {
        // setup Screen
        LocationScreen.Setup();
        LocationScreen.Exit();
    }

    // open a new overworld map
    public void OpenNewMap(OverworldMap newMap) 
    {
        // destroy previous map
        if (map != null)
            Destroy(map.gameObject);

        // instantiate map
        map = Instantiate(newMap, MapHolder).GetComponent<OverworldMap>();

        // Set map movement scroll rect
        MapScrollRect.content = map.GetComponent<RectTransform>();
        // set map name
        MapName.text = map.Name;

        // add map events
        if (map.MapEvents != null)
            Manager.Instance.ManagerEvents.EventAddList(map.MapEvents.Events);
    }

    // refresh map (check location availability)
    public void Refresh() 
    {
        // get list of active events
        SOEvent[] activeEvents = Manager.Instance.ManagerEvents.ActiveEvents.ToArray();

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

    #region Location
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
    }

    // called by location button to open a new location in the overworld
    public void ButtonLocation(SOLocation locationInfo) 
    {
        LocationSetInfo (locationInfo);
        LocationEnter();
    }
    #endregion
}
