using UnityEngine;
using UnityEngine.UI;

public class LocationScreen : MonoBehaviour
{
    public Image BackgroundImage;
    public LocationScreenButtonInfo[] EventButtons;

    private SOLocation locationInfo;

    public void Setup() 
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
    }
    public void Exit() 
    {
        gameObject.SetActive(false);
    }
    #endregion

    public void EventStart(SOEvent eventInfo) 
    {
        ManagerGameElements.Instance.ManagerEvents.OnEnventFinish += EventFinish;
        ManagerGameElements.Instance.ManagerEvents.EventStart(eventInfo);
    }
    public void EventFinish()
    {
        // called by event manager when event finishes
        ManagerGameElements.Instance.ManagerEvents.OnEnventFinish -= EventFinish;
        // refres location screen
        SetInfo(locationInfo);
    }

}
