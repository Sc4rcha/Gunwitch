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
        return player.Body * config.defenseMultiplier * config.bodyDefenseWeight +
               player.Mind * config.defenseMultiplier * (1f - config.bodyDefenseWeight);
    }
    public static float MagicalDefense(ActorPlayer player, SOCombatConfig config)
    {
        return player.Body * config.defenseMultiplier * (1f - config.mindDefenseWeight) +
               player.Mind * config.defenseMultiplier * config.mindDefenseWeight;
    }

    public static float Dodge(ActorPlayer player, SOCombatConfig config)
    {
        return Saturation(player.Dexterity, config.dodgeConst);
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
        float chance = LuckyMoment(player, config) * config.weightDodge;
        return RandomPercent() <= chance;
    }
    public static bool LuckyHit(ActorPlayer player, SOCombatConfig config)
    {
        float chance = LuckyMoment(player, config) * config.weightHit;
        return RandomPercent() <= chance;
    }
    public static bool LuckyCrit(ActorPlayer player, SOCombatConfig config)
    {
        float chance = LuckyMoment(player, config) * config.weightCrit;
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

    public static bool CheckHit (float rn, ActorEnemy enemy, SOEnemySkill skill)
    {
        return rn <= enemy.Accuracy * skill.Hit;
    }
    public static bool CheckDodge(float rn, ActorEnemy enemy, SOEnemySkill skill, ActorPlayer player, SOCombatConfig config)
    {
        return rn <= (enemy.Accuracy * skill.Hit) * Dodge(player, config);
    }

    // =========================
    // DAMAGE
    // =========================

    public static float DamageVariance(SOCombatConfig config)
    {
        return Random.Range(config.varianceMin, config.varianceMax);
    }

    public static float ApplyDefense(float damage, float defense, float constant)
    {
        float reduction = Saturation(defense, constant);
        return damage * (1f - reduction);
    }

    public static float DamageEnemyToPlayer(ActorPlayer player, ActorEnemy enemy, SOEnemySkill ability, SOCombatConfig config)
    {
        float baseDamage = ability.IsMagic ? enemy.Magic * ability.Power : enemy.Strength * ability.Power;

        float damage = baseDamage * DamageVariance(config);

        float defense = ability.IsMagic ? MagicalDefense(player, config) : PhysicalDefense(player, config);

        return ApplyDefense(damage, defense, config.defenseConstant);
    }
    public static float DamageEnemyToEnemy(ActorEnemy enemy, ActorEnemy target, SOEnemySkill ability, SOCombatConfig config) 
    {
        float baseDamage = ability.IsMagic ? enemy.Magic * ability.Power : enemy.Strength * ability.Power;
        float damage = baseDamage * DamageVariance(config);

        float defense = ability.IsMagic ? enemy.MagicResist : enemy.Armor;

        return ApplyDefense(damage, defense, config.defenseConstant);
    }

    public static float DamageBullet(ActorPlayer player, ActorEnemy enemy, Bullet bullet, float critMulti, bool isCrit, SOCombatConfig config)
    {
        float baseDamage = bullet.IsMagic ? bullet.Damage * (1f + SpellDamageBonus(player, config)) : bullet.Damage;

        // check CRIT
        if (isCrit)
            baseDamage *= critMulti;

        // check VULNERABLE
        if (enemy.StatusEffects.Contains(StatusEffect.Vulnerable))
        {
            baseDamage *= config.VulnerableMultiplier;
            enemy.StatusEffects.Remove(StatusEffect.Vulnerable);
        }

        float damage = baseDamage * DamageVariance(config);
        float defense = bullet.IsMagic ? enemy.MagicResist : enemy.Armor;

        return ApplyDefense(damage, defense, config.defenseConstant);
    }

    // =========================
    // PLAYER MOMENTUM (EXTRA BULLETS)
    // =========================

    public static int CalculateExtraBullets(ref int momentum, int magSize, ActorPlayer player, SOCombatConfig config)
    {
        float fraction = momentum / config.fullRevolverScore;
        int extraBullets = Mathf.FloorToInt(fraction * magSize);

        float spent = (extraBullets / (float)magSize) * config.fullRevolverScore;
        momentum -= (int)spent;

        return extraBullets;
    }

    // =========================
    // ENEMY MOMENTUM (EXTRA TURNS)
    // =========================

    public static bool EnemyExtraTurn(ref int momentum, ActorEnemy enemy, SOCombatConfig config)
    {
        if (momentum >= config.momentumExtraTurn)
        {
            momentum -= config.momentumExtraTurn;
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
