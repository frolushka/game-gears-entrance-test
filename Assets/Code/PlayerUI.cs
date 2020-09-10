using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    private const string IconsFolder = "Icons";

    [SerializeField] private Player player;
    
    public Button attackButton;
    public Transform statsPanel;

    [SerializeField] private GameObject statPrefab;
    
    private StatUI _healthStatUI;

    private void Awake()
    {
        player.onStatsUpdated += UpdateStats;
        player.onHealthUpdated += UpdateHealth;
        
        attackButton.onClick.AddListener(player.Attack);
    }
    
    private void UpdateStats(List<Stat> stats, List<Buff> buffs)
    {
        _healthStatUI = null;
        foreach (Transform statUI in statsPanel)
            Destroy(statUI.gameObject);
        
        stats.ForEach(stat =>
        {
            var statUI = AddStatUI(stat.icon, stat.value.ToString());
            if (stat.id == StatsId.LIFE_ID)
            {
                _healthStatUI = statUI;
            }
        });
        buffs?.ForEach(buff => AddStatUI(buff.icon, buff.title));
    }

    private void UpdateHealth(float value)
    {
        _healthStatUI.title.text = value.ToString("f0");
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
}
