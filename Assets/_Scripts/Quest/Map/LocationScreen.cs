using GameInfo;
using UnityEngine;
using UnityEngine.UI;

public class LocationScreen : MonoBehaviour
{
    public Image BackgroundImage;
    public GameObject ExitButton;
    public LocationScreenButtonInfo[] EventButtons;

    public void Setup() 
    {
        // setup event buttons
        foreach (var button in EventButtons)
            button.Setup(this);
    }

    public void Refresh(SOLocation location, SOEvent[] locationEvents)
    {
        // set screen background
        BackgroundImage.sprite = location.BackgroundSprite;

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
        ManagerGameElements.Instance.ManagerQuest.StartAutoplayEvent();
    }
    public void Exit() 
    {
        gameObject.SetActive(false);
    }
    #endregion

    public void EventStart(SOEvent eventInfo) 
    {
        // start events
        ManagerGameElements.Instance.ManagerQuest.EventStart(eventInfo);
    }

}
