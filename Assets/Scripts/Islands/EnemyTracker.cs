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

    private AIPath _agent;

    private float _originalSpeed; 

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Bridge")
        {
            if (collision.GetComponent<BridgeController>().Slows)
            {
                _originalSpeed = _agent.maxSpeed;
                _agent.maxSpeed = _agent.maxSpeed / 2;
            }
        } else if (collision.tag == "ProjectileShush")
        {
            _agent.maxSpeed *= 0.8f;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Bridge")
        {
            if(collision.GetComponent<BridgeController>().Slows)
            {
                _agent.maxSpeed = _originalSpeed;
                _agent.maxSpeed = _agent.maxSpeed / 2;
            }
        }
    }
}
