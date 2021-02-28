using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentTracker : MonoBehaviour
{
    private bool onBridge = false;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Bridge")
        {
            Debug.Log("OnBridge");
            onBridge = true;
            collision.gameObject.transform.parent.GetChild(0).GetComponent<BridgeController>().OnBridgeDisabled += DestroyParent;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Bridge")
        {
            Debug.Log("OffBridge");
            onBridge = false;
            collision.gameObject.transform.parent.GetChild(0).GetComponent<BridgeController>().OnBridgeDisabled -= DestroyParent;

        }
    }

    private void DestroyParent()
    {
        Destroy(gameObject);
    }
}
