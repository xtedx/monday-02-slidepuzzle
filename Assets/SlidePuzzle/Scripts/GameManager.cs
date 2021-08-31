using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<Box> boxList;
    [SerializeField] private int boxesPerRow = 3;
    [SerializeField] private float testpos;
    [SerializeField] private bool isLerp;
    /// <summary>
    /// movement speed
    /// </summary>
    [SerializeField] private float speed;
    [SerializeField] private GameObject menuGUI;
    
    /// <summary>
    /// contains the positions of the grid 0-8
    /// </summary>
    private List<Vector3> boxPosList;
    /// <summary>
    /// to set screen frame rate and resolution
    /// </summary>
    private Settings settings;
    
    // Start is called before the first frame update
    
    
    void Start()
    {
        init();
        sanityCheck();
        smoketest();
        Debug.Log(isSolved());
        for (int i = 1; i < 9; i++)
        {
        } 
    }

    // Update is called once per frame
    void Update()
    {
        UIKeyPress();
    }

    public void UIKeyPress()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleMenu();
        }
    }
    
    /// <summary>
    /// just a smoke test function to delete later
    /// </summary>
    private void smoketest()
    {
        shuffleBoxes();
        // moveBoxToDown(boxList[5]);
        // moveBoxToUp((boxList[5]));
        // moveBoxToRight((boxList[7]));
        // moveBoxToRight((boxList[6]));
        // moveBoxToLeft((boxList[6]));
        // moveBoxToLeft((boxList[7]));
        // moveBoxToLeft((boxList[2]));
    }


    public void init()
    {
        boxPosList = new List<Vector3>(9);
        boxPosList.Add(new Vector3(0, 0, 4)); //1
        boxPosList.Add(new Vector3(2, 0, 4)); //2
        boxPosList.Add(new Vector3(4, 0, 4)); //3
        boxPosList.Add(new Vector3(0, 0, 2)); //4
        boxPosList.Add(new Vector3(2, 0, 2)); //5
        boxPosList.Add(new Vector3(4, 0, 2)); //6
        boxPosList.Add(new Vector3(0, 0, 0)); //7
        boxPosList.Add(new Vector3(2, 0, 0)); //8
        boxPosList.Add(new Vector3(4, 0, 0)); //9

        settings = new Settings();
    }

    /// <summary>
    /// check if the puzzle is solved, i.e. pos 1 contains box 1
    /// </summary>
    /// <returns></returns>
    public bool isSolved()
    {
        //loops through the list of boxes, if any one is false, then the puzzle is not sovled.
        foreach (var box in boxList)
        {
            if (!box.isCorrectPos()) return false;
        }
        return true;
    }

    /// <summary>
    /// move box to the position in index
    /// </summary>
    /// <param name="index"></param>
    /// <param name="isLerp"></param>
    public void moveBoxToIndex(Box box, int index, bool isLerp, float speed)
    {
        box.moveToPos(boxPosList[index], isLerp, speed);
        box.gridIndex = index;
    }

    /// <summary>
    /// shuffle boxes positions
    /// </summary>
    public void shuffleBoxes()
    {
        var list = getRandomIntList(boxList.Count);
        foreach (var box in boxList)
        {
            //need to use boxnum -1 because the box num starts from 1 instead of 0 like the array index
            moveBoxToIndex(box, list[box.boxNum-1], isLerp, speed);
            // Debug.Log($"boxnum {box.boxNum}");
        }
    }

    public List<int> getRandomIntList(int size)
    {
        List<int> list = new List<int>(size);
        for (var i = 0; i < size; i++) 
        {
            list.Add(i);
        }
        // Debug.Log($"list before rand");
        // debugList(list);
        var rng = new Random();
        while (size > 1)
        {
            size--;
            int j = rng.Next(size + 1);
            //swap using csharp deconstruction
            (list[j], list[size]) = (list[size], list[j]);
        }
        // Debug.Log($"list after rand");
        // debugList(list);
        return list;
    }
    
    /// <summary>
    /// check if all variables are set.
    /// </summary>
    /// <exception cref="Exception"></exception>
    public void sanityCheck()
    {
        if (null == boxList)
        {
            throw new Exception("boxes not initialised in the correct positions");
        }
        foreach (var box in boxList)
        {
            if (!box.isCorrectPos())
            {
                throw new Exception("boxes not initialised in the correct positions");
            }
        }
    }

    /// <summary>
    /// move box down 1 grid
    /// </summary>
    /// <param name="box"></param>
    /// <exception cref="Exception"></exception>
    public void moveBoxToDown(Box box)
    {
        int nextIndex = box.gridIndex;
        //no need +1 for non modulo
        if (box.gridIndex < 6)
        {
            nextIndex = box.gridIndex + boxesPerRow;
            Debug.Log($"moving from {box.gridIndex} to {nextIndex}");
            moveBoxToIndex(box, nextIndex, isLerp, speed);
        }
        else
        {
            throw new Exception($"can't go down anymore from {box.gridIndex}");
        }
    }
    
    /// <summary>
    /// move box up 1 grid
    /// </summary>
    /// <param name="box"></param>
    /// <exception cref="Exception"></exception>
    public void moveBoxToUp(Box box)
    {
        int nextIndex = 0;
        //no need +1 for non modulo
        if (box.gridIndex > 2)
        {
            nextIndex = box.gridIndex - boxesPerRow;
            moveBoxToIndex(box, nextIndex, isLerp, speed);
        }
        else
        {
            throw new Exception($"can't go down anymore from {box.gridIndex}");
        }
    }
    
    /// <summary>
    /// move box left 1 grid if not 1 4 7 (column 1)
    /// </summary>
    /// <param name="box"></param>
    /// <exception cref="Exception"></exception>
    public void moveBoxToLeft(Box box)
    {
        int nextIndex = 0;
        //+1 to use 1 based array for modulo calculation
        if ((box.gridIndex+1) % boxesPerRow != 1)
        {
            nextIndex = box.gridIndex - 1;
            moveBoxToIndex(box, nextIndex, isLerp, speed);
        }
        else
        {
            throw new Exception($"can't go left anymore from {box.gridIndex}");
        }
    }
    
    /// <summary>
    /// move box right 1 grid if not from 3 6 9 (col 3)
    /// </summary>
    /// <param name="box"></param>
    /// <exception cref="Exception"></exception>
    public void moveBoxToRight(Box box)
    {
        int nextIndex = 0;
        //+1 to use 1 based array for modulo calculation
        if ((box.gridIndex+1) % boxesPerRow != 0) //not 3 6 9
        {
            nextIndex = box.gridIndex + 1;
            moveBoxToIndex(box, nextIndex, isLerp, speed);
        }
        else
        {
            throw new Exception($"can't go right anymore from {box.gridIndex}");
        }
    }

    /// <summary>
    /// toggles the display of menu
    /// </summary>
    public void ToggleMenu()
    {
        var current = menuGUI.activeSelf;
        menuGUI.SetActive(!current);
    }

    /// <summary>
    /// Quit the game application
    /// </summary>
    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }
    
    public void debugList(List<int> list)
    {
        foreach (var i in list)
        {
            Debug.Log(i);
        } 
    }

}
