using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    private const string IconsFolder = "Icons";
    
    public Button attackButton;
    public Transform statsPanel;

    [SerializeField] private GameObject statPrefab;

    private Player _player;
    
    private StatUI _healthStatUI;

    public void RegisterPlayer(Player player)
    {
        _player = player;
        attackButton.onClick.AddListener(_player.Attack);
    }
    
    public void UpdateStats(List<Stat> stats, List<Buff> buffs)
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

    public void UpdateHealth(float value)
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
