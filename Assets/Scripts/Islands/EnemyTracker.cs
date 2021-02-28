using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyTracker : MonoBehaviour
{
    public static Action OnEnemyDisabled;
    public static Action OnEnemyReachedDestination;

    private void OnDisable()
    {
        OnEnemyDisabled?.Invoke();
    }

    private void Update()
    {
        var target = GetComponent<AIDestinationSetter>().target;
        var remainingDistance = Mathf.Abs(Vector3.Distance(target.position, transform.position));

        if(remainingDistance < 1.0f)
        {
            OnEnemyReachedDestination?.Invoke();
            Destroy(gameObject);
        }
    }
}
