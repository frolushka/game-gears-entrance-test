using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Combat
{
    public static Player GetDefender(List<Player> players, Player attacker)
    {
        return players.FirstOrDefault(x => x != attacker);
    }
    public static void ApplyAttack(Player defender, Player attacker)
    {
        var defenderHealthStat = defender.stats.FirstOrDefault(x => x.id == StatsId.LIFE_ID);
        var attackerHealthStat = attacker.stats.FirstOrDefault(x => x.id == StatsId.LIFE_ID);
        var armorStat = defender.stats.FirstOrDefault(x => x.id == StatsId.ARMOR_ID);
        var damageStat = attacker.stats.FirstOrDefault(x => x.id == StatsId.DAMAGE_ID);
        var lifestealStat = attacker.stats.FirstOrDefault(x => x.id == StatsId.LIFE_STEAL_ID);
        if (defenderHealthStat == null || attackerHealthStat == null || armorStat == null || damageStat == null || lifestealStat == null)
        {
            Debug.LogWarning("One of important stats doesn't present");
            return;
        }

        var damage = (100 - armorStat.value) / 100f * damageStat.value;
        defenderHealthStat.value -= damage;
        attackerHealthStat.value += damage * lifestealStat.value / 100f;    
    }
}
