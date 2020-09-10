using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [SerializeField] private StatsWrapper statsWrapper;
    [SerializeField] private BuffsWrapper buffsWrapper;
    
    [SerializeField] private GameModel settings;
    [SerializeField] private List<Player> players;
    [SerializeField] private List<PlayerUI> playersUI;

    private void Awake()
    {
        players.ForEach(player =>
        {
            player.onDeath += GameOver;
            player.onPlayerAttack += Attack;
        });
    }

    private void Start()
    {
        Restart(true);
    }
    
    public void Restart(bool allowBuffs)
    {
        playersUI.ForEach(x => x.attackButton.interactable = true);

        players.ForEach(player =>
        {
            var stats = statsWrapper.stats
                .Select(x => new Stat
                {
                    icon = x.icon,
                    id = x.id,
                    title = x.title,
                    value = x.value
                })
                .ToList();

            List<Buff> buffs = null;

            if (allowBuffs)
            {
                var buffsCount = Mathf.Min(buffsWrapper.buffs.Count, Random.Range(settings.buffCountMin, settings.buffCountMax));
                var buffsSet = new HashSet<int>();
                for (var i = 0; i < buffsCount; i++)
                {
                    var random = Random.Range(0, buffsWrapper.buffs.Count);
                    while (!settings.allowDuplicateBuffs && buffsSet.Contains(random))
                        random = Random.Range(0, buffsWrapper.buffs.Count);
                    buffsSet.Add(random);
                }

                buffs = buffsSet
                    .Select(index => buffsWrapper.buffs[index])
                    .ToList();
                ApplyBuffs(stats, buffs);
            }

            player.UpdateStats(stats, buffs);
        });
    }

    private void Attack(Player attacker)
    {
        var defender = Combat.GetDefender(players, attacker);
        if (defender == null)
        {
            Debug.LogWarning("defender == null");
            return;
        }
        
        Combat.ApplyAttack(defender, attacker);
        defender.UpdateHealth();
        attacker.UpdateHealth();
    }

    private void GameOver()
    {
        playersUI.ForEach(x => x.attackButton.interactable = false);
    }

    private void ApplyBuffs(List<Stat> stats, IEnumerable<Buff> buffs)
    {
        var statMap = stats.ToDictionary(x => x.id, y => y);
        
        foreach (var buff in buffs)
        {
            foreach (var buffStat in buff.stats)
            {
                statMap[buffStat.statId].value += buffStat.value;
            }
        }
    }
}
