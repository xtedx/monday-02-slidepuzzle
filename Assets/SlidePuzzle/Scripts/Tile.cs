using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Tile : MonoBehaviour
{
    /// <summary>
    /// position of box in the game grid
    /// </summary>
    [SerializeField] public int gridIndex;

    public float howFar = 10;
    public float boxSize = 2f;
    
    public bool getRaycastHit(Vector3 from, float howFar)
    {
        RaycastHit hit;
        // Bit shift the index of the layer to get a bit mask
        // This would cast rays only against colliders in layer box.
        var layerMask = 1 << LayerMask.NameToLayer("Box");
        //if (Physics.Raycast(from, Vector3.up, out hit, howFar))
        if (Physics.Raycast(from, Vector3.up, out hit, howFar, layerMask))
        {
            // Debug.Log($"raycast {gridIndex} hits object {hit.collider.gameObject.name}");
            Debug.DrawLine(from, hit.point, Color.green);
        }
        else
        {
            // Debug.DrawRay(from, Vector3.up * howFar, Color.red);
        }
        return true;
    }

    // public void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawRay(transform.position, transform.forward * howFar);
    //
    // }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        getRaycastHit(transform.position, howFar);
    }
}
