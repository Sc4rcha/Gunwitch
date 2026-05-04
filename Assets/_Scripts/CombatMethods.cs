using GameInfo;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class CombatMethods
{
    // --- GENERIC ---
    public static float Saturation(float stat, float constant)
    {
        return stat / (stat + constant);
    }

    public static float RandomPercent()
    {
        return Random.Range(0f, 100f);
    }

    // =========================
    // PLAYER STATS
    // =========================

    public static float Health(ActorPlayer player, SOCombatConfig config)
    {
        return player.Body * config.bodyHealthMultiplier + config.bodyBaseHealth;
    }

    public static float Mana(ActorPlayer player, SOCombatConfig config)
    {
        return player.Mind * config.mindManaMultiplier + config.mindBaseMana;
    }

    public static float PhysicalDefense(ActorPlayer player, SOCombatConfig config)
    {
        return player.Body * config.bodyDefenseWeight +
               player.Mind * (1f - config.bodyDefenseWeight);
    }

    public static float MagicalDefense(ActorPlayer player, SOCombatConfig config)
    {
        return player.Body * (1f - config.mindDefenseWeight) +
               player.Mind * config.mindDefenseWeight;
    }

    public static float Dodge(ActorPlayer player, SOCombatConfig config)
    {
        return Saturation(player.Dexterity, config.dodgeConst) * 100f;
    }

    // =========================
    // LUCK
    // =========================

    public static float LuckyMoment(ActorPlayer player, SOCombatConfig config)
    {
        return Saturation(player.Luck, config.luckyConstant);
    }

    public static bool LuckyDodge(ActorPlayer player, SOCombatConfig config)
    {
        float chance = LuckyMoment(player, config) * config.weightDodge * 100f;
        return RandomPercent() <= chance;
    }

    public static bool LuckyHit(ActorPlayer player, SOCombatConfig config)
    {
        float chance = LuckyMoment(player, config) * config.weightHit * 100f;
        return RandomPercent() <= chance;
    }

    public static bool LuckyCrit(ActorPlayer player, SOCombatConfig config)
    {
        float chance = LuckyMoment(player, config) * config.weightCrit * 100f;
        return RandomPercent() <= chance;
    }

    public static int ExtraLootRolls(ActorPlayer player, SOCombatConfig config)
    {
        return 1 + Mathf.FloorToInt(player.Luck / config.dropLuckConstant);
    }

    // =========================
    // CHARISMA
    // =========================

    public static float SpellDamageBonus(ActorPlayer player, SOCombatConfig config)
    {
        return Saturation(player.Charisma, config.spellDamageConst);
    }

    // =========================
    // HIT / DODGE SYSTEM
    // =========================

    public static bool CheckHit(ActorPlayer player, ActorEnemy enemy, SOEnemySkill ability, SOCombatConfig config)
    {
        float hitChance = enemy.Accuracy * ability.Hit;

        float roll = RandomPercent();

        if (roll > hitChance)
        {
            // MISS ? try lucky hit
            return LuckyHit(player, config);
        }

        // Check dodge
        float dodge = Dodge(player, config);
        roll = RandomPercent();

        if (roll <= dodge)
        {
            // DODGE ? try lucky dodge
            return LuckyDodge(player, config) == false; // false = avoid damage
        }

        return true; // hit lands
    }

    // =========================
    // DAMAGE
    // =========================

    public static float DamageVariance(SOCombatConfig config)
    {
        return Random.Range(config.varianceMin, config.varianceMax);
    }

    public static float DefenseReduction(float defense, float constant)
    {
        return defense / (defense + constant);
    }

    public static float ApplyDefense(float damage, float defense, float constant)
    {
        float reduction = DefenseReduction(defense, constant);
        return damage * (1f - reduction);
    }

    public static float EnemyToPlayerDamage(
        ActorPlayer player,
        ActorEnemy enemy,
        SOEnemySkill ability,
        SOCombatConfig config)
    {
        float baseDamage = ability.IsMagic
            ? enemy.Magic * ability.Power
            : enemy.Strength * ability.Power;

        float variance = DamageVariance(config);
        float damage = baseDamage * variance;

        float defense = ability.IsMagic
            ? MagicalDefense(player, config)
            : PhysicalDefense(player, config);

        return ApplyDefense(damage, defense, config.defenseConstant);
    }

    public static float PlayerDamageRegular(float bulletDamage, SOCombatConfig config)
    {
        return bulletDamage * DamageVariance(config);
    }

    public static float PlayerDamageMagic(ActorPlayer player, float baseDamage, SOCombatConfig config)
    {
        float bonus = SpellDamageBonus(player, config);
        float damage = baseDamage * (1f + bonus);
        return damage * DamageVariance(config);
    }

    // =========================
    // PLAYER MOMENTUM (EXTRA BULLETS)
    // =========================

    public static int CalculateExtraBullets(
        ref float momentum,
        float dexterity,
        float fullRevolverScore,
        int magSize)
    {
        // Gain momentum
        momentum += dexterity;

        float fraction = momentum / fullRevolverScore;
        int extraBullets = Mathf.FloorToInt(fraction * magSize);

        float spent = (extraBullets / (float)magSize) * fullRevolverScore;
        momentum -= spent;

        return extraBullets;
    }

    // =========================
    // ENEMY MOMENTUM (EXTRA TURNS)
    // =========================

    public static bool EnemyExtraTurn(ref float momentum, float enemySpeed, float threshold)
    {
        momentum += enemySpeed;

        if (momentum >= threshold)
        {
            momentum -= threshold;
            return true;
        }

        return false;
    }

    // =========================
    // ENEMY ACTION SELECTION
    // =========================

    public static SOEnemyAction SelectByRating(List<SOEnemyAction> actions)
    {
        int maxRating = actions.Max(a => a.Priority);

        // -3 is the max distance between max prio and min prio
        List<SOEnemyAction> filtered = actions.Where(a => a.Priority >= maxRating - 3).ToList();

        int totalWeight = filtered.Sum(a => a.Priority);

        int roll = Random.Range(0, totalWeight);

        int cumulative = 0;

        foreach (var action in filtered)
        {
            cumulative += action.Priority;
            if (roll < cumulative)
                return action;
        }

        return filtered[0];
    }
}
