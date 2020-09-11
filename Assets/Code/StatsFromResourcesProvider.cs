using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StatsFromResourcesProvider : IStatsProvider
{
    [Serializable]
    public class ArrayWrapper<T>
    {
        public T[] data;
    }
    
    private const string StatsJsonPath = "stats";
    private const string BuffsJsonPath = "buffs";

    public Stat[] GetStats() => GetArray<Stat>(StatsJsonPath);
    public Buff[] GetBuffs() => GetArray<Buff>(BuffsJsonPath);

    private T[] GetArray<T>(string path)
    {
        var jsonFile = Resources.Load<TextAsset>(path);
        return JsonUtility.FromJson<ArrayWrapper<T>>("{\"data\":" + jsonFile.text + "}").data;
    }
}
