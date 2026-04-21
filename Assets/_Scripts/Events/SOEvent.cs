using UnityEngine;

[CreateAssetMenu(fileName = "Event", menuName = "Event/Event", order = 0)]
public class SOEvent : ScriptableObject
{
    public string Name;
    public SOLocation EventLocation;
    [Tooltip ("Does this event stay after the player has finished?")]
    public bool IsPersistent;
    [Tooltip ("Does this event play the moment the player enters the location?")]
    public bool IsAutoplay;

    [Space]
    [Tooltip ("Array of flags to check to unlock event")]
    public SOEventFlag[] UnlockFlags;
    [Tooltip ("Actions called when the event is triggered")]
    public SOEventAction[] EventActions;

    public bool CheckUnlocked(GameInfo.Flags flags) 
    {
        // check if any unlocked condition is not met and return false
        foreach (var unlockCondition in UnlockFlags)
            if (!unlockCondition.Check(flags))
                return false;

        // otherwise return true
        return true;
    }
}
