using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static IStatsProvider StatsProvider = new StatsFromResourcesProvider();
    public static IBattleCalculator BattleCalculator = new BattleCalculator();
    
    [SerializeField] private GameModel settings;
    [SerializeField] private Player[] players;
    [SerializeField] private PlayerUI[] playersUI;
    [SerializeField] private HealthBar[] healthBars;
    
    #region Unity events

    private void Start()
    {
        Restart(true);
    }

    private void OnEnable()
    {
        for (var i = 0; i < players.Length; i++)
        {
            players[i].onDeath += GameOver;
            players[i].onPlayerAttack += Attack;
        }
    }

    private void OnDisable()
    {
        for (var i = 0; i < players.Length; i++)
        {
            players[i].onDeath -= GameOver;
            players[i].onPlayerAttack -= Attack;
        }
    }

    #endregion

    #region Private

    private void Attack(Player attacker)
    {
        var defender = players.Single(x => x != attacker);
        defender.DefendFrom(attacker);
    }

    private void GameOver() => ActivatePlayerUI(false);

    private void ActivatePlayerUI(bool value)
    {
        for (var i = 0; i < playersUI.Length; i++)
        {
            playersUI[i].enabled = value;
        }
    }

    private void ActivateHealthBars(bool value)
    {
        for (var i = 0; i < healthBars.Length; i++)
        {
            healthBars[i].gameObject.SetActive(value);
        }
    }

    private void ApplyBuffs(Stat[] stats, Buff[] buffs)
    {
        var statMap = stats.ToDictionary(x => x.id, y => y);

        for (var i = 0; i < buffs.Length; i++)
        {
            for (var j = 0; j < buffs[i].stats.Length; j++)
            {
                var buffStat = buffs[i].stats[j];
                statMap[buffStat.statId].value += buffStat.value;
            }
        }
    }

    #endregion

    public void Restart(bool allowBuffs)
    {
        ActivatePlayerUI(true);
        ActivateHealthBars(true);

        for (var i = 0; i < players.Length; i++)
        {
            var stats = StatsProvider.GetStats();

            Buff[] buffs = null;
            if (allowBuffs)
            {
                var tempBuffs = StatsProvider.GetBuffs();
                var buffsCount = Mathf.Min(tempBuffs.Length, Random.Range(settings.buffCountMin, settings.buffCountMax));
                if (settings.allowDuplicateBuffs)
                {
                    buffs = new Buff[buffsCount];
                    for (var j = 0; j < buffsCount; j++)
                    {
                        buffs[j] = tempBuffs[Random.Range(0, tempBuffs.Length)];
                    }
                }
                else
                {
                    var buffsSet = new HashSet<int>();
                    for (var j = 0; j < buffsCount; j++)
                    {
                        // Need optimization
                        var random = Random.Range(0, tempBuffs.Length);
                        while (buffsSet.Contains(random))
                            random = Random.Range(0, tempBuffs.Length);
                        buffsSet.Add(random);
                    }

                    buffs = buffsSet
                        .Select(index => tempBuffs[index])
                        .ToArray();
                }
                
                ApplyBuffs(stats, buffs);
            }

            players[i].UpdateStats(stats, buffs);
        }
    }
}
