using UnityEngine;
using GameInfo;

public abstract class SOEventAction : ScriptableObject
{
    public abstract void Execute(ManagerEvents manager);
}
