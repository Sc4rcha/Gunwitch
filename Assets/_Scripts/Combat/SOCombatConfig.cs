using UnityEngine;

[CreateAssetMenu(fileName = "Combat Config", menuName = "Combat/Configuration")]
public class SOCombatConfig : ScriptableObject
{
    [Header("Body")]
    public float bodyHealthMultiplier = 20f;
    public float bodyBaseHealth = 100f;
    [Range(0f, 1f)] public float bodyDefenseWeight = 0.75f;

    [Header("Mind")]
    public float mindManaMultiplier = 10f;
    public float mindBaseMana = 50f;
    [Range(0f, 1f)] public float mindDefenseWeight = 0.75f;

    [Header("Dexterity")]
    [Tooltip ("Value at witch dodge chances becomes 50%")]
    public float dodgeConst = 50f;
    [Tooltip ("momentum at witch the player gets a full reload")]
    public float fullRevolverScore = 100f;

    [Header("Defense")]
    [Tooltip ("value at witch damage reduction becomes 50%")]
    public float defenseConstant = 100f;

    [Header("Luck")]
    [Tooltip("luck needed for an extra drop for each drop after combat")]
    public float dropLuckConstant = 50f;
    [Tooltip("value at witch base lucky chance becomes 50%")]
    public float luckyConstant = 50f;
    [Range(0f, 1f)] public float weightDodge = 0.4f;
    [Range(0f, 1f)] public float weightHit = 0.6f;
    [Range(0f, 1f)] public float weightCrit = 0.25f;

    [Header("Charisma")]
    [Tooltip("value at witch spell damage is multiplied by 1.5x")]
    public float spellDamageConst = 100f;

    [Header("Variance")]
    public float varianceMin = 0.8f;
    public float varianceMax = 1.2f;
}