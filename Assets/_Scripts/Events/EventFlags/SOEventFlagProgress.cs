using GameInfo;
using UnityEngine;

[CreateAssetMenu(fileName = "Flag Check Progress", menuName = "Event/Flag Check/Progress")]
public class SOEventFlagProgress : SOEventFlag
{
    public SOFlag Flag;
    public int Progress;

    public override bool Check(Flags flags)
    {
        return flags.GetInt(Flag.Name) >= Progress;
    }
}
