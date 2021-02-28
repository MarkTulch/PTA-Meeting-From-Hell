using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyTracker : MonoBehaviour
{
    public static Action OnEnemyDisabled;
    public static Action OnEnemyReachedDestination;

    private float _selfDestructTime = 120;
    private float _runningTimer;

    private void OnDisable()
    {
        OnEnemyDisabled?.Invoke();
    }

    private void Update()
    {
        _runningTimer += Time.deltaTime;

        var target = GetComponent<AIDestinationSetter>().target;
        var remainingDistance = Mathf.Abs(Vector3.Distance(target.position, transform.position));

        if(remainingDistance < 1.0f)
        {
            OnEnemyDisabled?.Invoke();
            OnEnemyReachedDestination?.Invoke();
            Destroy(gameObject);
        }

        if(_runningTimer > _selfDestructTime)
        {
            OnEnemyDisabled?.Invoke();
            Destroy(gameObject);
        }
    }
}
