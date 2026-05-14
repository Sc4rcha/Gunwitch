using GameInfo;
using UnityEngine;

public abstract class SOEventFlag : ScriptableObject
{
    public abstract bool Check(FlagList flags);
}
