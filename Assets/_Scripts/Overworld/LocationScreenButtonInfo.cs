using UnityEngine;
using UnityEngine.UI;

public class LocationScreenButtonInfo : MonoBehaviour
{
    public TMPro.TMP_Text EventName;

    private LocationScreen screen;
    private SOEvent eventInfo;

    public void Setup(LocationScreen screen) 
    {
        this.screen = screen;
        Hide();
    }

    public void Show(SOEvent eventInfo) 
    {
        this.eventInfo = eventInfo;

        // set button name
        EventName.text = eventInfo.Name;

        // activate button
        gameObject.SetActive(true);
    }
    public void Hide() 
    {
        // deactivate button
        gameObject.SetActive(false);
    }

    public void ButtonInteract() 
    {
        // close location screen
        screen.Exit();

        // start event
        ManagerGameElements.Instance.ManagerEvents.EventStart(eventInfo);
    }
}
