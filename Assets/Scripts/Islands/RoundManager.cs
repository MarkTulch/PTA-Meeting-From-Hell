using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Pathfinding;

[Serializable]
public class Round
{
    public List<Wave> waves;
}

public class RoundManager : MonoBehaviour
{
    [SerializeField] private List<Round> _rounds;

    [SerializeField] private float _timeBetweenRounds;
    [SerializeField] private float _timeBetweenSpawns;
    [SerializeField] private float _prepTime;

    [SerializeField] private Transform _northSpawnPoint;
    [SerializeField] private Transform _southSpawnPoint;
    [SerializeField] private Transform _eastSpawnPoint;
    [SerializeField] private Transform _westSpawnPoint;

    [SerializeField] private GameObject _seekerEnemyPF;
    [SerializeField] private GameObject _tigerEnemyPF;

    [SerializeField] private Transform _seekerTarget;
    [SerializeField] private Transform _tigerTarget;

    [SerializeField] private BridgeManager _bridgeManager;

    private float _runningTimer;
    private int currentRound = 0;
    private int aliveEnemies;

    public static Action<SpawnPoint> OnParentsSpawned;

    private void OnEnable()
    {
        EnemyTracker.OnEnemyDisabled += DecrementEnemyCounter;
    }


    private void OnDisable()
    {
        EnemyTracker.OnEnemyDisabled -= DecrementEnemyCounter;
    }

    private void Update()
    {
        _runningTimer += Time.deltaTime;

        if(_runningTimer > _timeBetweenRounds && aliveEnemies == 0)
        {
            _runningTimer = 0;

            StartNextRound();
        }
    }

    private void StartNextRound()
    {
        if(currentRound >= _rounds.Count) { return; }

        _bridgeManager.ShuffleBridges();

        foreach (var wave in _rounds[currentRound].waves)
        {
            if(wave.shuffleBridges && aliveEnemies == 0)
            {
                _bridgeManager.ShuffleBridges();
            }

            foreach (var spawn in wave.spawns)
            {
                if(spawn.parentType == ParentType.Seeker)
                {
                    SpawnParents(spawn, _seekerEnemyPF);
                }
                else
                {
                    SpawnParents(spawn, _tigerEnemyPF);
                }
                OnParentsSpawned?.Invoke(spawn.spawnPoint);
            }
        }

        currentRound += 1;
    }

    private void SpawnParents(Spawn _spawn, GameObject parent)
    {
        switch (_spawn.spawnPoint)
        {
            case SpawnPoint.North:
                StartCoroutine(Spawn(_spawn, _northSpawnPoint, parent));
                break;
            case SpawnPoint.South:
                StartCoroutine(Spawn(_spawn, _southSpawnPoint, parent));
                break;
            case SpawnPoint.East:
                StartCoroutine(Spawn(_spawn, _eastSpawnPoint, parent));
                break;
            case SpawnPoint.West:
                StartCoroutine(Spawn(_spawn, _westSpawnPoint, parent));
                break;
        }
    }

    private IEnumerator Spawn(Spawn _spawn, Transform spawnPoint, GameObject parent)
    {
        for (int i = 0; i < _spawn.amount; i++)
        {
            var parentGO = Instantiate(parent, spawnPoint.position, Quaternion.identity);
            parentGO.GetComponent<AIDestinationSetter>().target = (_spawn.parentType == ParentType.Seeker) ? _seekerTarget : _tigerTarget;
            aliveEnemies += 1;

            yield return new WaitForSeconds(_timeBetweenSpawns);
        }
    }

    private void DecrementEnemyCounter()
    {
        aliveEnemies -= 1;
    }
}
