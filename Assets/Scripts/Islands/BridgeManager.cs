using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.TopDownEngine;

[Serializable]
public class BridgeDecision
{
    [SerializeField] public List<BridgeController> option1;
    [SerializeField] public List<BridgeController> option2;
}

public class BridgeManager : MonoBehaviour
{
    [SerializeField] private List<BridgeDecision> _bridgeDecisions;

    private void Start()
    {
        DisableAllDecisionBridges();

        ShuffleBridges();
    }

    public void ShuffleBridges()
    {
        //DisableAllDecisionBridges();

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
