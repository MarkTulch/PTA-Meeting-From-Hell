using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class IslandController : MonoBehaviour
{
    [SerializeField] private List<BridgeController> _connectedBridges;

    public bool BridgeAvailabe => _connectedBridges.Any(bridge => bridge.Enabled);

    private bool _startTimer = false;

    private float _runningTimer;

    private float _bridgeCreationRate = 5f;

    private void Update()
    {
        if(_startTimer)
        {
            _runningTimer += Time.deltaTime;

            if(_runningTimer > _bridgeCreationRate)
            {
                _startTimer = false;

                var nextBridge = GetDisabledBridge();
                DisableEnabledBridge();
                if(nextBridge != null)
                {
                    nextBridge.EnableBridge();
                }
            }
        }
    }

    public Transform GetEnemyWaypoint()
    {
        foreach (var bridge in _connectedBridges)
        {
            if(bridge.Enabled)
            {
                return bridge.EnemyWaypoint;
            }
        }
        return null;
    }

    public void StartTimer()
    {
        _startTimer = true;
    }

    private BridgeController GetDisabledBridge()
    {
        foreach (var bridge in _connectedBridges)
        {
            if(!bridge.Enabled)
            {
                return bridge;
            }
        }

        return null;
    }

    private void DisableEnabledBridge()
    {
        foreach (var bridge in _connectedBridges)
        {
            if (bridge.Enabled)
            {
                bridge.DisableBridge();
            }
        }
    }
}
