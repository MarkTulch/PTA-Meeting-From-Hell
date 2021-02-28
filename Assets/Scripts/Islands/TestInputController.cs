using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MoreMountains.TopDownEngine;
public class TestInputController : MonoBehaviour
{
    private Vector3 lastPosition;

    [SerializeField] private GameObject _groundGO;

    private Stack<Vector3> islandPositions;

    public LayerMask _bridgeMask;

    private void OnEnable()
    {
        IslandClickDetector.OnIslandClicked += OnIslandClicked;
        islandPositions = new Stack<Vector3>();
    }

    private void OnDisable()
    {
        IslandClickDetector.OnIslandClicked -= OnIslandClicked;
    }

    private void OnIslandClicked(Vector3 position)
    {
        islandPositions.Push(position);

        if(islandPositions.Count == 2)
        {
            CreateBridge();
        }
    }

    private void CreateBridge()
    {
        var startPos = islandPositions.Pop();
        var endPos = islandPositions.Pop();
        var deltaX = Mathf.Abs(startPos.x - endPos.x);

        var ground = Instantiate(_groundGO, startPos, Quaternion.identity);
        var groundSpriteRenderer = ground.GetComponent<SpriteRenderer>();

        groundSpriteRenderer.size = new Vector2(deltaX, 2);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            var collider = Physics2D.OverlapCircle(transform.position, 1f, _bridgeMask);
            if(collider != null)
            {
                collider.gameObject.GetComponent<BridgeController>().EnableBridge();
            }
        }
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.DrawSphere(transform.position, 1f);
    //}
}
