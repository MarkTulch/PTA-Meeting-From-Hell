using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Pathfinding;
using MoreMountains.TopDownEngine;

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
    [SerializeField] private GameObject _helicopterEnemyPF;

    [SerializeField] private Transform _seekerTarget;
    [SerializeField] private Transform _tigerTarget;

    [SerializeField] private BridgeManager _bridgeManager;

    private float _runningTimer;
    private int currentRound = 0;
    private int aliveEnemies;

    public static Action OnPrepTimeStarted;
    public static Action OnRoundStarted;
    public static Action<SpawnPoint> OnParentsSpawned;

    private List<GameObject> _parents;

    private void OnEnable()
    {
        EnemyTracker.OnEnemyDisabled += DecrementEnemyCounter;
    }


    private void OnDisable()
    {
        EnemyTracker.OnEnemyDisabled -= DecrementEnemyCounter;
    }

    private void Start()
    {
        _parents = new List<GameObject>(GameObject.FindGameObjectsWithTag("Player"));
    }

    private void Update()
    {
        _runningTimer += Time.deltaTime;

        if(_runningTimer > _timeBetweenRounds && aliveEnemies == 0)
        {
            _runningTimer = 0;

            StartCoroutine(StartNextRound());
        }
    }

    private IEnumerator StartNextRound()
    {
        if(currentRound >= _rounds.Count) { yield return null; }

        OnPrepTimeStarted?.Invoke();
        yield return new WaitForSeconds(_prepTime);

        _bridgeManager.ShuffleBridges();
        OnRoundStarted?.Invoke();

        foreach (var wave in _rounds[currentRound].waves)
        {
            if(wave.delay > 0)
            {
                yield return new WaitForSeconds(wave.delay);
            }

            if (wave.shuffleBridges && aliveEnemies == 0)
            {
                _bridgeManager.ShuffleBridges();
            }

            foreach (var spawn in wave.spawns)
            {
                if(spawn.parentType == ParentType.Seeker)
                {
                    SpawnParents(spawn, _seekerEnemyPF);
                }
                else if(spawn.parentType == ParentType.Seeker)
                {
                    SpawnParents(spawn, _tigerEnemyPF);
                }
                else
                {
                    SpawnParents(spawn, _helicopterEnemyPF);
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
            case SpawnPoint.Random:
                var rand = UnityEngine.Random.Range(0, 5);
                var spawnPoint = (rand > 0.5) ? ((rand > 0.75) ? _northSpawnPoint : _southSpawnPoint) : ((rand < 0.25) ? _eastSpawnPoint : _westSpawnPoint);
                StartCoroutine(Spawn(_spawn, spawnPoint, parent));
                break;
        }
    }

    private IEnumerator Spawn(Spawn _spawn, Transform spawnPoint, GameObject parent)
    {
        for (int i = 0; i < _spawn.amount; i++)
        {
            var parentGO = Instantiate(parent, spawnPoint.position, Quaternion.identity);
            SetParentTarget(parentGO, _spawn.parentType);
            //parentGO.GetComponent<AIDestinationSetter>().target = (_spawn.parentType == ParentType.Seeker) ? _seekerTarget : _tigerTarget;
            aliveEnemies += 1;

            yield return new WaitForSeconds(_timeBetweenSpawns);
        }
    }

    private void SetParentTarget(GameObject parentGO, ParentType type)
    {
        if(type == ParentType.Helicopter)
        {
            parentGO.GetComponent<HelicopterMover>()._target = _seekerTarget;
        }
        else
        {
            parentGO.GetComponent<AIDestinationSetter>().target = _seekerTarget;
        }
    }

    private void DecrementEnemyCounter()
    {
        aliveEnemies -= 1;
    }
}
