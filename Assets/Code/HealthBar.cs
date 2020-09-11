using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    #region Unity events

    private void OnEnable()
    {
        player.onStatsUpdated += UpdateStats;
        player.onHealthUpdated += UpdateHealthBarFillAmount;
        
        player.onDeath += Hide;
    }

    private void OnDisable()
    {
        player.onStatsUpdated -= UpdateStats;
        player.onHealthUpdated -= UpdateHealthBarFillAmount;
        
        player.onDeath -= Hide;
    }

    private void FixedUpdate()
    {
        UpdatePosition();
    }

    #endregion

    #region Private

    private void Hide() => gameObject.SetActive(false);

    private void UpdateStats(Stat[] stats, Buff[] buffs)
    {
        _maxHealth = stats.Single(x => x.id == StatsId.LIFE_ID).value;
        image.fillAmount = 1;
        
        UpdatePosition();
    }
    
    private void UpdatePosition()
    {
        if (!player)
            return;
        
        var screenPosition = camera.WorldToScreenPoint(player.transform.position + Vector3.up * height);
        transform.position = screenPosition;
    }
    
    private void UpdateHealthBarFillAmount(float prevValue, float curValue)
    {
        var delta = curValue - prevValue;
        if (!Mathf.Approximately(delta, 0))
            InstantiateDeltaText(delta);
            
        image.fillAmount = curValue / _maxHealth;
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

    #endregion
}
