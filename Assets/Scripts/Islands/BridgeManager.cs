using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BridgeDecision
{
    [SerializeField] public List<BridgeController> option1;
    [SerializeField] public List<BridgeController> option2;
}

public class BridgeManager : MonoBehaviour
{
    //private List<BridgeController> _bridges;
    //private void OnEnable()
    //{
    //    BridgeController.OnBridgeCreated += OnBridgeCreated;
    //}

    //private void OnDisable()
    //{
    //    BridgeController.OnBridgeCreated -= OnBridgeCreated;
    //}

    //private void OnBridgeCreated(BridgeController bridgeController)
    //{
    //    foreach (var bridge in _bridges)
    //    {
    //        if(bridge == bridgeController) { continue; }

    //        bridge.DisableBridge();
    //    }
    //}

    //private void Awake()
    //{
    //    _bridges = new List<BridgeController>(FindObjectsOfType<BridgeController>());
    //}

    [SerializeField] private List<BridgeDecision> _bridgeDecisions;

    private void Start()
    {
        DisableAllDecisionBridges();

        foreach (var decision in _bridgeDecisions)
        {
            var rand = UnityEngine.Random.Range(0.0f, 1.0f);

            if (rand >= 0.5f)
            {
                foreach (var bridge in decision.option1)
                {
                    bridge.EnableBridge();
                }
            }
            else
            {
                foreach (var bridge in decision.option2)
                {
                    bridge.EnableBridge();
                }
            }
        }
        AstarPath.active.Scan();
    }

    private void DisableAllDecisionBridges()
    {
        foreach (var decision in _bridgeDecisions)
        {
            foreach (var bridge in decision.option1)
            {
                bridge.DisableBridge();
            }
            foreach (var bridge in decision.option2)
            {
                bridge.DisableBridge();
            }
        }
    }
}
