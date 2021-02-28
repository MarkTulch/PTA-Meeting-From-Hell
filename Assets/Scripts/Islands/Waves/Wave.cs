using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpawnPoint
{
    North,
    South,
    East,
    West,
    Random
}

public enum ParentType
{
    Tiger,
    Seeker
}

[Serializable]
public class Spawn
{
    public SpawnPoint spawnPoint;
    public ParentType parentType;
    public int amount;
}

[CreateAssetMenu(fileName = "Wave", menuName = "Waves/Wave", order = 1)]
public class Wave : ScriptableObject
{
    public List<Spawn> spawns;
    public bool shuffleBridges;
    public float delay;
}

