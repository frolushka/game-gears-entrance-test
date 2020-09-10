using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Stats Wrapper", menuName = "Stats Wrapper")]
public class StatsWrapper : ScriptableObject
{
    public List<Stat> stats;
}
