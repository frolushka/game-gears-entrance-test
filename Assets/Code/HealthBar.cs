using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Player player;
    [Space]
    [SerializeField] private GameObject floatingText;
    [SerializeField] private Camera camera;
    [SerializeField] private Image image;
    [Space] 
    [SerializeField] private float height = 3.5f;
    
    private float _maxHealth;
    private float _currentHealth;

    private void Awake()
    {
        player.onStatsUpdated += UpdateStats;
        player.onHealthUpdated += UpdateHealth;
        
        player.onDeath += () => gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        UpdatePosition();
    }

    private void UpdateStats(List<Stat> stats, List<Buff> buffs)
    {
        _maxHealth = player.HealthStat.value;
        _currentHealth = _maxHealth;
        image.fillAmount = 1;
        
        UpdatePosition();
        gameObject.SetActive(true);
    }
    
    private void UpdateHealth(float value)
    {
        var delta = value - _currentHealth;
        if (!Mathf.Approximately(delta, 0))
            InstantiateDeltaText(delta);
            
        _currentHealth = value;
        image.fillAmount = _currentHealth / _maxHealth;
    }

    private void UpdatePosition()
    {
        if (!player)
            return;
        
        var screenPosition = camera.WorldToScreenPoint(player.transform.position + Vector3.up * height);
        transform.position = screenPosition;
    }

    private void InstantiateDeltaText(float value)
    {
        var deltaText = Instantiate(floatingText, transform.parent);
        deltaText.transform.position = transform.position;
        var barDeltaText = deltaText.GetComponent<HealthBarDeltaText>();
        if (value > 0)
        {
            barDeltaText.text.text = $"+{(int)value}";
            barDeltaText.text.color = Color.green;
        }
        else
        {
            barDeltaText.text.text = $"{(int)value}";
            barDeltaText.text.color = Color.red;
        }
        
        barDeltaText.StartMove();
    }
}
