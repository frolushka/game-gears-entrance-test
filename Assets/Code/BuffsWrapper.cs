using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Buffs Wrapper", menuName = "Buffs Wrapper")]
public class BuffsWrapper : ScriptableObject
{
    public List<Buff> buffs;
}
