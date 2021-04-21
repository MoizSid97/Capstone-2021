using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    //https://www.youtube.com/watch?v=AKKpPmxx07w&t=403s&ab_channel=Daniel

    //X Position in Node Array
    public int gridX;
    //Y Positoin in Node Array
    public int gridY;
    //Cost of moving to the next square
    public int gCost;
    //Distance to the goal from this node
    public int hCost;
    //Get function to add G cost and H cost
    public int fCost { get { return gCost + hCost; } }
    //Tells code if this node is being obstructed
    public bool isWall;
    //World position of the node
    public Vector3 position;
    //For A* alogrithm, will store what node it previously came from so it can trace to the nearest path
    public Node parent;

    //Constructor
    public Node(bool a_isWall, Vector3 a_Pos, int a_gridX, int a_gridY)
    {
        //Tells program if this node is being obstructed
        isWall = a_isWall;
        //World position of the node
        position = a_Pos;
        //X position in the node array
        gridX = a_gridX;
        //Y position in the node array
        gridY = a_gridY;
    }
}
