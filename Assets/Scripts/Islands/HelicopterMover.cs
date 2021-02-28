using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelicopterMover : MonoBehaviour
{
    public Transform _target;

    [SerializeField] private float _speed;

    void Update()
    {
        if(_target == null)
        {
            return;
        }
        transform.position = Vector3.MoveTowards(transform.position, _target.position, Time.deltaTime * _speed);
    }
}
