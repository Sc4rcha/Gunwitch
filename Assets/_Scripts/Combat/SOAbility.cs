using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Ability", menuName = "Combat/Ability")]
public class SOAbility : ScriptableObject
{
    public string Name;
    public int Damage;
    [Range (1,200)]
    public int Hit;
}
