using UnityEngine;
using UnityEngine.UI;

public class LocationScreen : MonoBehaviour
{
    public Image BackgroundImage;
    public GameObject ExitButton;
    public LocationScreenButtonInfo[] EventButtons;

    private SOLocation locationInfo;

    public void Setup(ManagerMap manager) 
    {
        // setup event buttons
        foreach (var button in EventButtons)
            button.Setup(this);
    }

    public void SetInfo(SOLocation locationInfo)
    {
        // set location Info
        this.locationInfo = locationInfo;

        // set screen background
        BackgroundImage.sprite = locationInfo.BackgroundSprite;

        // get all events in location
        SOEvent[] locationEvents = ManagerGameElements.Instance.ManagerEvents.GetLocationEvents(locationInfo);

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
        foreach (var locationEvent in ManagerGameElements.Instance.ManagerEvents.GetLocationEvents (locationInfo))
        {
            if (locationEvent.IsAutoplay)
            {
                EventStart(locationEvent);
                break;
            }
        }
    }
    public void Exit() 
    {
        gameObject.SetActive(false);
    }
    #endregion

    public void EventStart(SOEvent eventInfo) 
    {
        // Add event finish to managerevents on event finish trigger
        ManagerGameElements.Instance.ManagerEvents.OnEnventFinish += EventFinish;
        // start events
        ManagerGameElements.Instance.ManagerEvents.EventStart(eventInfo);
    }
    public void EventFinish()
    {
        // remove event finish to managerevents on event finish trigger
        ManagerGameElements.Instance.ManagerEvents.OnEnventFinish -= EventFinish;
        // refresh location screen
        SetInfo(locationInfo);
    }

}
