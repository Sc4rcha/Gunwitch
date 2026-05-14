using GameInfo;
using UnityEngine;

[CreateAssetMenu(fileName = "Flag Check Progress", menuName = "Event/Flag Check/Progress", order = 12)]
public class SOEventFlagProgress : SOEventFlag
{
    public SOFlag Flag;
    public int Value;

    public override bool Check(FlagList flags)
    {
        return flags.GetInt(Flag.Name) >= Value;
    }
}
