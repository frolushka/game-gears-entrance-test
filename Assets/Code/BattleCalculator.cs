using System.Linq;

public class BattleCalculator : IBattleCalculator
{
    public void Battle(Stat[] attackerStats, Stat[] defenderStats)
    {
        var defenderHealthStat = defenderStats.Single(x => x.id == StatsId.LIFE_ID);
        var attackerHealthStat = attackerStats.Single(x => x.id == StatsId.LIFE_ID);
        var defenderArmorStat = defenderStats.Single(x => x.id == StatsId.ARMOR_ID);
        var attackerDamageStat = attackerStats.Single(x => x.id == StatsId.DAMAGE_ID);
        var attackerLifestealStat = attackerStats.Single(x => x.id == StatsId.LIFE_STEAL_ID);

        var damage = (100 - defenderArmorStat.value) / 100f * attackerDamageStat.value;
        defenderHealthStat.value -= damage;
        attackerHealthStat.value += damage * attackerLifestealStat.value / 100f;  
    }
}