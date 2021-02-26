using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeController : MonoBehaviour
{
    [SerializeField] private BoxCollider2D _topNode;
    [SerializeField] private BoxCollider2D _bottomNode;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    public static Action<BridgeController> OnBridgeCreated;

    public void EnableBridge()
    {
        _topNode.enabled = false;
        _bottomNode.enabled = false;
        _spriteRenderer.enabled = true;

        OnBridgeCreated?.Invoke(this);
    }

    public void DisableBridge()
    {
        _topNode.enabled = true;
        _bottomNode.enabled = true;
        _spriteRenderer.enabled = false;
    }
}
