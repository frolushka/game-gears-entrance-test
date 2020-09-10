﻿using System;

[Serializable]
public class Data
{
    public CameraModel cameraSettings;
    public GameModel settings;
    public Stat[] stats;
    public Buff[] buffs;
}

[Serializable]
public class CameraModel
{
    public float roundDuration;
    public float roundRadius;
    public float height;
    public float lookAtHeight;
    public float roamingRadius;
    public float roamingDuration;
    public float fovMin;
    public float fovMax;
    public float fovDelay;
    public float fovDuration;
}

[Serializable]
public class GameModel
{
    public int playersCount;
    public int buffCountMin;
    public int buffCountMax;
    public bool allowDuplicateBuffs;
}

[Serializable]
public class Stat
{
    public string title;
    public int id;
    public string icon;
    public float value;
}

[Serializable]
public class BuffStat
{
    public int statId;
    public float value;
}

[Serializable]
public class Buff
{
    public string title;
    public int id;
    public string icon;
    public BuffStat[] stats;
}
