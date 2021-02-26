using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeManager : MonoBehaviour
{
    private List<BridgeController> _bridges;
    private void OnEnable()
    {
        BridgeController.OnBridgeCreated += OnBridgeCreated;
    }

    private void OnDisable()
    {
        BridgeController.OnBridgeCreated -= OnBridgeCreated;
    }

    private void OnBridgeCreated(BridgeController bridgeController)
    {
        foreach (var bridge in _bridges)
        {
            if(bridge == bridgeController) { continue; }

            bridge.DisableBridge();
        }
    }

    private void Awake()
    {
        _bridges = new List<BridgeController>(FindObjectsOfType<BridgeController>());
    }
}
