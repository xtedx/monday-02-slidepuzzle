using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    /// reference to gamemanager
    /// </summary>
    [SerializeField] private GameManager _gameManager;

    public Vector3 correctPos;
    // Start is called before the first frame update
    void Start()
    {
        sanityCheck();
        setCorrectPos();

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
}
