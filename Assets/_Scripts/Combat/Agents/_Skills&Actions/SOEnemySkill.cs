using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Skill", menuName = "Combat/Action/Skill")]
public class SOEnemySkill : ScriptableObject
{
    public string Name;
    [Space]
    public float Hit;
    public float Power;
    [Space]
    public bool IsMagic;
}
