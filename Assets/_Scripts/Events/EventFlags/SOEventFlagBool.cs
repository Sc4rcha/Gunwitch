using GameInfo;
using UnityEngine;

[CreateAssetMenu(fileName = "Flag Check Bool", menuName = "Event/Flag Check/Bool")]
public class SOEventFlagBool : SOEventFlag
{
    public SOFlag Flag;

    public override bool Check(Flags flags)
    {
        return flags.GetBool(Flag.Name);
    }
}
