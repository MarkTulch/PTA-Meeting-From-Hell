using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMover : MonoBehaviour
{
    [SerializeField] private IslandController _currentIsland;
    [SerializeField] private LayerMask _islandMask;

    private float _moveIslandRate = 2f;
    private float _runningTimer;

    private Transform _waypoint;

    void Update()
    {
        _runningTimer += Time.deltaTime;

        TryGetNewWaypoint();
        MoveEnemy();
    }

    private void MoveEnemy()
    {
        if (_waypoint == null) { return; }

        if(!CheckIfAtDestination())
        {
            MoveEnemyTowardsWaypoint();
        }
    }

    private bool CheckIfAtDestination()
    {
        var distDelta = Mathf.Abs(Vector3.Distance(transform.position, _waypoint.position));

        if (distDelta < 0.5f)
        {
            return true;
        }
        return false;
    }

    private void MoveEnemyTowardsWaypoint()
    {
        transform.position = Vector3.MoveTowards(transform.position, _waypoint.position, Time.deltaTime * 10);
    }
    private void TryGetNewWaypoint()
    {
        if(_currentIsland == null) { return; }

        if (_runningTimer > _moveIslandRate)
        {
            _runningTimer = 0;

            if (_currentIsland.BridgeAvailabe)
            {
                _waypoint = _currentIsland.GetEnemyWaypoint();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var col = collision.gameObject;
        if(col.layer == 24)
        {
            var newIsland = col.GetComponent<IslandController>();
            if (_currentIsland != newIsland)
            {
                _currentIsland = newIsland;
                _currentIsland.StartTimer();
            }
        }
    }
}
