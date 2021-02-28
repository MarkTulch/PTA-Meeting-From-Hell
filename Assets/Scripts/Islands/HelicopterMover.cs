using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelicopterMover : MonoBehaviour
{
    public Transform _target;

    [SerializeField] private float _speed;

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, _target.position, Time.deltaTime * _speed);
    }
}
