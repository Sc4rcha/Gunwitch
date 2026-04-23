using UnityEngine;

[CreateAssetMenu(fileName = "Flag", menuName = "Event/Flag", order = 3)]
public class SOFlag : ScriptableObject
{
    public string Name;
    [TextArea]
    public string Description;
}
