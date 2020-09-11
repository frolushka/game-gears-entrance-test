using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    private const string IconsFolder = "Icons";

    [SerializeField] private Player player;
    
    [SerializeField] private GameObject statPrefab;
    
    [SerializeField] private Button attackButton;
    [SerializeField] private Transform statsPanel;

    private StatUI _healthStatUI;

    #region Unity events

    private void Awake()
    {
        attackButton.onClick.AddListener(player.RequestAttack);
    }

    private void OnEnable()
    {
        player.onStatsUpdated += UpdateStats;
        player.onHealthUpdated += UpdateHealthStatUI;
        
        attackButton.enabled = true;
    }

    private void OnDisable()
    {
        player.onStatsUpdated -= UpdateStats;
        player.onHealthUpdated -= UpdateHealthStatUI;
        
        attackButton.enabled = false;
    }

    #endregion

    #region Private

    private void UpdateStats(Stat[] stats, Buff[] buffs)
    {
        _healthStatUI = null;
        foreach (Transform statUI in statsPanel)
            Destroy(statUI.gameObject);

        if (stats != null)
        {
            for (var i = 0; i < stats.Length; i++)
            {
                var stat = stats[i];
                var statUI = AddStatUI(stat.icon, stat.value.ToString());
                if (stat.id == StatsId.LIFE_ID)
                {
                    _healthStatUI = statUI;
                }
            }
        }

        if (buffs != null)
        {
            for (var i = 0; i < buffs.Length; i++)
            {
                var buff = buffs[i];
                AddStatUI(buff.icon, buff.title);
            }
        }
    }

    private void UpdateHealthStatUI(float prevValue, float currentValue)
    {
        _healthStatUI.title.text = currentValue.ToString("f0");
    }

    private StatUI AddStatUI(string iconName, string text)
    {
        var icon = Resources.Load<Sprite>(Path.Combine(IconsFolder, iconName));
        var statUI = Instantiate(statPrefab, statsPanel)
            .GetComponent<StatUI>();
        
        statUI.icon.sprite = icon;
        statUI.title.text = text;
        return statUI;
    }

    #endregion
}
