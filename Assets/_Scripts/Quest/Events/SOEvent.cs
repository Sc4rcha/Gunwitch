using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Event", menuName = "Event/Event", order = 1)]
public class SOEvent : ScriptableObject
{
    public string Id;

    [ContextMenu("Generate ID")]
    private void GenerateID()
    {
        Id = Guid.NewGuid().ToString();
        Debug.Log("Generated ID: " + Id);
    }

    public string Name;
    public SOLocation EventLocation;
    [Tooltip ("Does this event stay after the player has finished?")]
    public bool IsPersistent;
    [Tooltip ("Does this event play the moment the player enters the location?")]
    public bool IsAutoplay;
    [Space]
    [Tooltip ("The action that has the event behaviour")]
    public SOEventAction EventAction;
    [Space]
    [Tooltip ("Array of flags to check to unlock event")]
    public SOEventFlag[] UnlockFlags;
    [Tooltip ("Actions called when the event is triggered")]
    public SOEventAction[] EventActions;

    public bool CheckLocked(GameInfo.FlagList flags) 
    {
        // check if any unlocked condition is not met and return true
        foreach (var unlockCondition in UnlockFlags)
            if (!unlockCondition.Check(flags))
                return true;

        // if all unlocked conitions are met return true
        return false;
    }
}
