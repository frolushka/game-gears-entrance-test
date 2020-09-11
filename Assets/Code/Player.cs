using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
public class Player : MonoBehaviour
{
    public delegate void StatsUpdateDelegate(Stat[] stats, Buff[] buffs);
    public delegate void HealthUpdateDelegate(float prevHealth, float curHealth);

    public delegate void DeathDelegate();

    public delegate void PlayerDelegate(Player player);
        
    private const string HealthName = "Health";
    private const string AttackTrigger = "Attack";
    
    public event StatsUpdateDelegate onStatsUpdated;
    public event HealthUpdateDelegate onHealthUpdated;
    
    public event DeathDelegate onDeath;
    
    public event PlayerDelegate onPlayerAttack;
    
    private Stat[] _stats;
    private Buff[] _buffs;
    
    private Animator _animator;

    private float _prevHealth;
    private Stat _healthStat;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void UpdateStats(Stat[] stats, Buff[] buffs)
    {
        // To reset animator to default state
        _animator.Play(null);
        
        _stats = stats;
        _buffs = buffs;
        
        _healthStat = stats.SingleOrDefault(x => x.id == StatsId.LIFE_ID);
        _prevHealth = _healthStat.value;

        onStatsUpdated?.Invoke(stats, buffs);
        
        UpdateHealth();
    }

    public void UpdateHealth()
    {
        _animator.SetInteger(HealthName, (int)_healthStat.value);
        
        onHealthUpdated?.Invoke(_prevHealth, _healthStat.value);
        _prevHealth = _healthStat.value;

        if (_healthStat.value <= 0)
            onDeath?.Invoke();
    }

    public void RequestAttack()
    {
        _animator.SetTrigger(AttackTrigger);

        onPlayerAttack?.Invoke(this);
    }
    
    public void DefendFrom(Player attacker)
    {
        GameManager.BattleCalculator.Battle(attacker._stats, _stats);
        
        UpdateHealth();
        attacker.UpdateHealth();
    }
}
