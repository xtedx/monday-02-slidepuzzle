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

    private float GetAxisRightClick(string axisName)
    {
        if (axisName.StartsWith("Mouse") && !Input.GetMouseButton((int)MouseButton.RightMouse))
        {
            return 0;
        }
        return Input.GetAxis(axisName);
    }
}
