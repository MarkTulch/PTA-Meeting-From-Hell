using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandClickDetector: MonoBehaviour
{
    public static Action<Vector3> OnIslandClicked;

    private void OnMouseDown()
    {
        Debug.Log("Collider clicked!");
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;
        var position = Camera.main.ScreenToWorldPoint(mousePos);
        OnIslandClicked?.Invoke(position);
    }
}
