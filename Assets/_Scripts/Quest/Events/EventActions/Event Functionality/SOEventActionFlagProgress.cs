using GameInfo;
using UnityEngine;

[CreateAssetMenu(fileName = "Flag Progress", menuName = "Event/Actions Functionality/Flag Progress", order = 33)]
public class SOEventActionFlagProgress : SOEventAction
{
    public SOFlag FlagInfo;
    public int ProgressAmount = 1;

    public override void Execute(ManagerQuest manager)
    {
        manager.MapState.Flags.AddProgress(FlagInfo.Name, ProgressAmount);
    }
}
