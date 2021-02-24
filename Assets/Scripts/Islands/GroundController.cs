using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundController : MonoBehaviour
{
    [SerializeField] BoxCollider2D _topCollider;
    [SerializeField] BoxCollider2D _bottomCollider;

    [SerializeField] CircleCollider2D _topLeftCollider;
    [SerializeField] CircleCollider2D _bottomLeftCollider;
    [SerializeField] CircleCollider2D _topRightCollider;
    [SerializeField] CircleCollider2D _bottomRightCollider;

    private void Update()
    {
        var spriteSize = GetComponent<SpriteRenderer>().size;

        _topCollider.offset = new Vector2(spriteSize.x/2, spriteSize.y / 2);
        _topCollider.size = new Vector2(spriteSize.x, 0.04f);

        _bottomCollider.offset = new Vector2(_topCollider.offset.x, -spriteSize.y / 2);
        _bottomCollider.size = new Vector2(spriteSize.x, 0.04f);

        _topLeftCollider.offset = new Vector2(_topLeftCollider.offset.x, spriteSize.y / 2);
        _bottomLeftCollider.offset = new Vector2(_bottomLeftCollider.offset.x, -spriteSize.y / 2);

        _topRightCollider.offset = new Vector2(spriteSize.x, spriteSize.y / 2);
        _bottomRightCollider.offset = new Vector2(spriteSize.x, - spriteSize.y / 2);
    }
}
