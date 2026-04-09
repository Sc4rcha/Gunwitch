using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Ability", menuName = "Combat/Ability")]
public class SOAbility : ScriptableObject
{
    public string Name;
    public int Damage;
    [Range (1,100)]
    public int Hit;
}
