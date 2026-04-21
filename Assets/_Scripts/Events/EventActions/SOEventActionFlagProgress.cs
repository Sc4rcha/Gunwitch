using GameInfo;
using UnityEngine;

[CreateAssetMenu(fileName = "Flag Progress", menuName = "Event/Actions Functionality/Flag Progress", order = 3)]
public class SOEventActionFlagProgress : SOEventAction
{
    public SOFlag FlagInfo;
    public int ProgressAmount = 1;

    public override void Execute(ManagerEvents manager)
    {
        manager.EventFlags.AddProgress(FlagInfo.Name, ProgressAmount);
    }
}
