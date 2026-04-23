using GameInfo;
using UnityEngine;

[CreateAssetMenu(fileName = "Flag Check Bool", menuName = "Event/Flag Check/Bool", order = 11)]
public class SOEventFlagBool : SOEventFlag
{
    public SOFlag Flag;
    public bool Value;

    public override bool Check(Flags flags)
    {
        return flags.GetBool(Flag.Name) == Value;
    }
}
