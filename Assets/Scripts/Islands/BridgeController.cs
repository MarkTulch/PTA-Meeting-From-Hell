using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeController : MonoBehaviour
{
    [SerializeField] private BoxCollider2D _topNode;
    [SerializeField] private BoxCollider2D _bottomNode;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    [SerializeField] private Transform _enemyWaypoint;

    public static Action<BridgeController> OnBridgeCreated;

    private bool _isEnabled = false;
    public bool Enabled => _isEnabled;

    public Transform EnemyWaypoint => _enemyWaypoint;

    public Action OnBridgeDisabled;

    private bool _slowEnemy = false;

    public bool Slows => _slowEnemy;

    public void EnableBridge()
    {
        _isEnabled = true;

        _topNode.enabled = false;
        _bottomNode.enabled = false;
        _spriteRenderer.enabled = true;
        gameObject.SetActive(false);

        //OnBridgeCreated?.Invoke(this);
    }

    public void DisableBridge()
    {
        _isEnabled = false;

        _topNode.enabled = true;
        _bottomNode.enabled = true;
        _spriteRenderer.enabled = false;
        gameObject.SetActive(true);
        OnBridgeDisabled?.Invoke();

        _slowEnemy = false;
    }

    public void ActivateSlow()
    {
        _slowEnemy = true;
    }
}
