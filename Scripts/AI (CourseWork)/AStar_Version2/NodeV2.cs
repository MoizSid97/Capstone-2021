using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//https://www.youtube.com/watch?v=nhiFx28e7JY&list=PLFt_AvWsXl0cq5Umv3pMC9SPnKjfp9eGW&index=2&ab_channel=SebastianLague

public class NodeV2 
{
    public bool walkable;
    public Vector3 worldPos;

    public int grid_X;
    public int grid_Y;

    public int gCost;
    public int hCost;

    public NodeV2 parent;


    public NodeV2(bool _walkable, Vector3 _worldPos, int _x, int _y)
    {
        walkable = _walkable;
        worldPos = _worldPos;

        grid_X = _x;
        grid_Y = _y;

    }

    public int fCost { get { return gCost + hCost; } }
}
