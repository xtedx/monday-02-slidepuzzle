using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UIElements;

/// <summary>
/// Sets the cinemachine free look camera to only function when mouse right click is down.
/// </summary>
public class FreelookRightMouse : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        CinemachineCore.GetInputAxis = GetAxisRightClick;
    }

    /// <summary>
    /// overwrites the built in function to get input axis.
    /// this custom function makes it possible to only move the camera with right mouse click
    /// </summary>
    /// <param name="axisName"></param>
    /// <returns></returns>
    private float GetAxisRightClick(string axisName)
    {
        if (axisName.StartsWith("Mouse") && !Input.GetMouseButton((int)MouseButton.RightMouse))
        {
            return 0;
        }
        return Input.GetAxis(axisName);
    }
}
