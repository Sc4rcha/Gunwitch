using UnityEngine;

[CreateAssetMenu(fileName = "SOStatusEffect", menuName = "Combat/Status Effect")]
public class SOStatusEffect : ScriptableObject
{
    public GameInfo.StatusEffect StatusEffect;
    public Sprite StatusSprite;
}
