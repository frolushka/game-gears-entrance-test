using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class UpdatePlayerEvent : UnityEvent<List<Stat>, List<Buff>> { }
[Serializable]
public class PlayerEvent : UnityEvent<Player> { }
[Serializable]
public class UpdateHealthEvent : UnityEvent<float> { }

[RequireComponent(typeof(Animator))]
public class Player : MonoBehaviour
{
    private const string HealthName = "Health";
    private const string AttackTrigger = "Attack";

    [HideInInspector] public List<Stat> stats;
    [HideInInspector] public List<Buff> buffs;
    
    private Animator _animator;

    public PlayerEvent onPlayerCreated;
    public UpdatePlayerEvent onStatsUpdated;
    public UpdateHealthEvent onHealthUpdated;
    public UnityEvent onDeath;
    public PlayerEvent onPlayerAttack;
    
    public Stat HealthStat { get; private set; }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        onPlayerCreated?.Invoke(this);
    }

    public void UpdateStats(List<Stat> stats, List<Buff> buffs)
    {
        // костыль
        _animator.Play("Entry");
        
        this.stats = stats;
        this.buffs = buffs;
        HealthStat = stats.FirstOrDefault(x => x.id == StatsId.LIFE_ID);

        onStatsUpdated?.Invoke(stats, buffs);
        
        UpdateHealth();
    }

    public void UpdateHealth()
    {
        _animator.SetInteger(HealthName, (int)HealthStat.value);
        
        onHealthUpdated?.Invoke(HealthStat.value);

        if (HealthStat.value <= 0)
            onDeath?.Invoke();
    }

    public void Attack()
    {
        _animator.SetTrigger(AttackTrigger);

        onPlayerAttack?.Invoke(this);
    }
}
