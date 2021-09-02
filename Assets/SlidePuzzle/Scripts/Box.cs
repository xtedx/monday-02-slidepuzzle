using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class Box : MonoBehaviour
{
    /// <summary>
    /// the id of each box, to be set in unity
    /// </summary>
    [SerializeField] public int boxNum;
    /// <summary>
    /// position of box in the game grid
    /// </summary>
    [SerializeField] public int gridIndex;
    /// <summary>
    /// howFar should the player be infront of the box for detection
    /// </summary>
    [SerializeField] public float howFar;
    
    /// <summary>
    /// reference to gamemanager
    /// </summary>
    [SerializeField] private GameManager _gameManager;

    public Vector3 correctPos;
    public Dictionary<string, GameObject> dictQuad;
    // Start is called before the first frame update
    void Start()
    {
        sanityCheck();
        setCorrectPos();
        getQuads();
    }

    private void checkQuadsRaycast()
    {
        foreach (var pair in dictQuad)
        {
            var dir = pair.Key switch
            {
                "zp" => Vector3.forward,
                "zn" => Vector3.back,
                "xp" => Vector3.right,
                "xn" => Vector3.left,
                _ => throw new Exception($"{pair.Key} is invalid side in switch")
            };
            dir += pair.Value.transform.position;
            getRaycastHit(pair.Key, pair.Value.transform.position, dir, howFar);
            // Debug.Log($"create ray from {boxNum}.{pair.Key} {pair.Value.transform.position} to {dir}");
        }
    }

    /// <summary>
/// get the quads and store in in a dictionary
/// </summary>
/// <exception cref="Exception"></exception>
    private void getQuads()
    {
        //need to get transform first because not allowed to get GameObject directly
        var gObjs = GetComponentsInChildren<Transform>();
        Transform[] quads = null;
        foreach (var g in gObjs)
        {
            if (g.gameObject.name == "quads")
            {
                quads = g.GetComponentsInChildren<Transform>();
                break;
            }
        }
        //add to dictionary
        dictQuad = new Dictionary<string, GameObject>();
        foreach (var quad in quads)
        {
            if (quad.gameObject.name.StartsWith("z") || quad.gameObject.name.StartsWith("x"))
            {
                dictQuad.Add(quad.gameObject.name, quad.gameObject);
            }
        }
        if (dictQuad.Count != 4)
        {
            throw new Exception($"must have 4 side objects here, but has {dictQuad.Count}");
        }
    }

/// <summary>
/// check if ray cast from quad to player hits
/// </summary>
/// <param name="name">quad name</param>
/// <param name="from">position of ray source</param>
/// <param name="howFar">how far from the source</param>
/// <returns></returns>
    public bool getRaycastHit(string name, Vector3 from, Vector3 to, float howFar)
    {
        RaycastHit hit;
        // Bit shift the index of the layer to get a bit mask
        // This would cast rays only against colliders in layer player.
        var layerMask = 1 << LayerMask.NameToLayer("Player");
        //if (Physics.Raycast(from, Vector3.up, out hit, howFar))
        if (Physics.Raycast(from, to, out hit, howFar, layerMask))
        {
            Debug.Log($"raycast {boxNum}.{name} hits object {hit.collider.gameObject.name}");
            Debug.DrawLine(from, hit.point, Color.green);
        }
        else
        {
            // Debug.Log($"raycast {boxNum}.{name} from {from} no hits");
            // Debug.DrawLine(from, to, Color.blue);
        }
        return true;
    }
    /// <summary>
    /// set starting position of this box as the correct position
    /// </summary>
    private void setCorrectPos()
    {
        correctPos = transform.position;
        boxNum = Convert.ToInt16(gameObject.name.Substring(4, 1));
        gridIndex = boxNum - 1;
        //Debug.Log($"object name {gameObject.name.Substring(4,1)}");
    }

    // Update is called once per frame
    void Update()
    {
        checkQuadsRaycast();
    }

    /// <summary>
    /// call coroutine to move box
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="isLerp"></param>
    /// <param name="speed"></param>
    public void moveToPos(Vector3 pos, bool isLerp, float speed)
    {
       StartCoroutine(moveToXZ(pos.x,pos.z, isLerp, speed)); 
    }

    /// <summary>
    /// move box to x z position, keeping y the same
    /// </summary>
    /// <param name="x"></param>
    /// <param name="z"></param>
    /// <param name="isLerp">true to Lerp, false to MoveTo</param>
    /// <param name="speed">movement speed</param>
    public IEnumerator moveToXZ(float x, float z, bool isLerp, float speed)
    {
        var next = new Vector3(x, transform.position.y, z);
        while (true)
        {
            if (isLerp)
            {
                transform.position = Vector3.Lerp(transform.position, next, Time.deltaTime * speed);
                if (Vector3.Distance(transform.position, next) < 0.1f)
                {
                    //use set position to be exact
                    transform.SetPositionAndRotation(next, transform.rotation);
                    yield break; // from loop
                }
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, next, Time.deltaTime * speed);
                if (transform.position == next)
                {
                    yield break;
                }
            }
            //continue running
            yield return null;
        }
    }
    // public void moveToXZ(float x, float z)
    // {
    //     var next = new Vector3(x, transform.position.y, z);
    //     transform.position = Vector3.MoveTowards(transform.position, next, Time.deltaTime * speed);
    // }

    public void sanityCheck()
    {
        if (null == _gameManager)
        {
            throw new Exception("Please set GameManager reference in Unity Editor");
        }
    }


    public bool isCorrectPos()
    {
        return (transform.position == correctPos);
    }
    
    // public void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.cyan;
    //     Gizmos.DrawRay(new Vector3(1.0f, 1.0f, -4.1f), transform.right * howFar);
    //     Gizmos.DrawRay(new Vector3(1.0f, 1.0f, -4.1f), transform.up * howFar);
    //     Gizmos.DrawRay(new Vector3(1.0f, 1.0f, -4.1f), transform.forward * howFar);
    //
    // }
}
