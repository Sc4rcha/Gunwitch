using GameInfo;
using UnityEngine;
using UnityEngine.UI;

public class LocationScreen : MonoBehaviour
{
    public Image BackgroundImage;
    public GameObject ExitButton;
    public LocationScreenButtonInfo[] EventButtons;

    private ManagerMap managerMap;
    private ManagerEvents managerEvents;

    public void Setup(ManagerEvents managerEvent, ManagerMap managerMap) 
    {
        this.managerEvents = managerEvent;
        this.managerMap = managerMap;

        // setup event buttons
        foreach (var button in EventButtons)
            button.Setup(this);

        // Add event finish to manager events on event finish trigger
        managerEvents.OnEnventFinish += EventFinish;
    }

    public void Refresh()
    {
        // set screen background
        BackgroundImage.sprite = managerMap.MapState.WorldLocation.BackgroundSprite;

        // get all events in location
        SOEvent[] locationEvents = managerEvents.GetLocationEvents(managerMap.MapState.WorldLocation);

        // set location event buttons
        for (int i = 0; i < EventButtons.Length; i++)
        {
            // hide by default
            EventButtons[i].Hide();
            // show event button if event is available
            if (locationEvents.Length > i)
                EventButtons[i].Show(locationEvents[i]);
        }
    }


    #region Enter/Exit
    public void Enter() 
    {
        gameObject.SetActive(true);

        // start event with autoplay in location
        managerEvents.StartAutoplayEvent();

    }
    public void Exit() 
    {
        gameObject.SetActive(false);
    }
    #endregion

    public void EventStart(SOEvent eventInfo) 
    {
        // start events
        managerEvents.EventStart(eventInfo);
    }
    public void EventFinish()
    {
        // refresh location screen
        Refresh();
    }
}
