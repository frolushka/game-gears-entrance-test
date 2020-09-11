using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStatsProvider
{
    Stat[] GetStats();
    Buff[] GetBuffs();
}
