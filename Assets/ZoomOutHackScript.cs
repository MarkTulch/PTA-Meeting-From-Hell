using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ZoomOutHackScript : MonoBehaviour
{
    /// the name of the axis to use to catch input and trigger a swap on press
    [Tooltip("the name of the axis to use to catch input and trigger a swap on press")]
    public string SwapButtonName = "Player1_SwapCharacter";
    /// The name of the axis to use to catch input and zoom out/deselect units
    public string ZoomOutButtonName = "ZoomOut";
    // Start is called before the first frame update
    protected bool _isZoomedOut = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    /// <summary>
    /// If the user presses the Swap button, we swap characters
    /// </summary>
    protected virtual void HandleInput()
    {
        if (Input.GetButtonDown(ZoomOutButtonName))
        {
            if (_isZoomedOut)
            {
                ZoomIn();
            }
            else
            {
                ZoomOut();
            }
        }
    }

    private void ZoomIn()
    {
        FindObjectOfType<CinemachineVirtualCamera>().m_Lens.OrthographicSize = 10f;
        _isZoomedOut = false;
    }

    private void ZoomOut()
    {
        Debug.Log("ZOOMING OUT");
        FindObjectOfType<CinemachineVirtualCamera>().m_Lens.OrthographicSize = 20f;
        _isZoomedOut = true;
    }
}
