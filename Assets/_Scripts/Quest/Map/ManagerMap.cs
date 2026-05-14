using GameInfo;
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

    public Map Map { get; private set; }

    public void Setup() 
    {
        // setup 
        LocationScreen.Setup();
        LocationScreen.Exit();

        // hide map screen
        gameObject.SetActive(false);
    }


    // Load a new map
    public void MapEnter(Map map) 
    {
        // destroy previous map
        if (Map != null)
            Destroy(Map.gameObject);

        // instantiate map
        Map = Instantiate(map, MapHolder).GetComponent<Map>();
        Map.Setup();

        // Set map movement scroll rect
        MapScrollRect.content = Map.GetComponent<RectTransform>();
        // set map name
        MapName.text = Map.Name;

        // setup location buttons
        foreach (var locationButton in Map.Locations)
            locationButton.Setup();

        // show map screen
        gameObject.SetActive(true);
        // hide finish quest button
        ButtonFinishQuest.SetActive(false);
    }
    public void MapExit() 
    {
        // hide map screen
        gameObject.SetActive(false);

        // destroy instanced map
        Destroy(Map.gameObject);
    }


    // refresh map (check location availability)
    public void Refresh(ManagerQuest questManager) 
    {
        foreach (var location in Map.Locations)
        {
            switch (location.Visibility)
            {
                case LocationVisibilityType.Normal:
                    // activate location if any event is taking place there
                    location.gameObject.SetActive(questManager.GetLocationEvents(location.LocationReference).Length > 0);
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
    // enter location screen
    public void LocationEnter(SOLocation location, SOEvent[] events) 
    {
        LocationScreen.Refresh(location, events);
        LocationScreen.Enter();
    }
    // exit location screen
    public void LocationExit()
    {
        LocationScreen.Exit();
    }
    // Lock location
    public void LocationLock(bool isLocked) 
    {
        LocationScreen.ExitButton.SetActive(!isLocked);
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

