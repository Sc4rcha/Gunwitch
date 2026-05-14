using System;
using UnityEngine;
using UnityEngine.UI;

public class ManagerEvents : MonoBehaviour
{
    [Header ("Scene References")]
    public Transform EventObjectParent;
    public GameObject EventScreen;
    public Image EventBackground;

    private SOEvent eventSelected;
    private PlayMakerFSM eventFSMInstance;


    #region EVENT START
    public void EventStart(SOEvent eventSelected)
    {
        if (this.eventSelected != null)
            Debug.LogError("Event already running");

        // Player Start event
        ManagerGameElements.Instance.Player.EventStart();

        // set event selected reference
        this.eventSelected = eventSelected;

        // show Location background
        EventScreen.SetActive(eventSelected.EventLocation != null);
        if (eventSelected.EventLocation != null)
            EventBackground.sprite = eventSelected.EventLocation.BackgroundSprite;

        // start event behaviour if it has action
        if (eventSelected.EventAction != null)
            eventSelected.EventAction.Execute(ManagerGameElements.Instance.ManagerQuest);
        //otherwise end event (for events with no behaviour)
        else
            ManagerGameElements.Instance.ManagerQuest.EventFinish();
    }
    public void EventFinish()
    {
        // hide event background
        EventScreen.SetActive(false);

        // reset event FSM instance reference and destroy
        if (eventFSMInstance != null)
        {
            Destroy(eventFSMInstance.gameObject);
            eventFSMInstance = null;
        }

        // execute event actions
        foreach (var action in eventSelected.EventActions)
            action.Execute(ManagerGameElements.Instance.ManagerQuest);

        //clear event selected reference
        eventSelected = null;

        // Player Finish event
        ManagerGameElements.Instance.Player.EventFinish();
    }
    #endregion


    public void InstantiateFSM(PlayMakerFSM fsm)
    {
        // instantiate event object
        eventFSMInstance = Instantiate(fsm, EventObjectParent);
        // start FSM
        eventFSMInstance.SendEvent("EVENT_START");
    }
}
