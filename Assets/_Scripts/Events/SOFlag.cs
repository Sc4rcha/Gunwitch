using UnityEngine;

[CreateAssetMenu(fileName = "Flag", menuName = "Event/Flag")]
public class SOFlag : ScriptableObject
{
    public string Name;
    [TextArea]
    public string Description;
}
