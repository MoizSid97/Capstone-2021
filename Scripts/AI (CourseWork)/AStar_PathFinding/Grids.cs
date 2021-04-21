using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grids : MonoBehaviour
{
    //https://www.youtube.com/watch?v=AKKpPmxx07w&t=403s&ab_channel=Daniel

    public Transform startPositon;
    public LayerMask wallMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    public float distance;

    Node[,] grid;

    public List<Node> FinalPath;

    private float nodeDiameter;

    private int gridSizeX;
    private int gridSizeY;

    // Start is called before the first frame update
    void Start()
    {
        nodeDiameter = nodeRadius * 2;

        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeX = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);

        CreateGrid();
    }

    // Update is called once per frame
    void Update()
    {
      
    }

    void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];
        Vector3 bottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;
        for(int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = bottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                
                bool wall = true;

                if(Physics.CheckSphere(worldPoint, nodeRadius, wallMask))
                {
                    //Object is not wall
                    wall = false;
                }

                //Create a new node in the array
                grid[x, y] = new Node(wall, worldPoint, x, y);
            }
        }
    }

    //function that gets the neighboring nodes 
    public List<Node> GetNeighbouringNodes(Node a_node)
    {
        List<Node> neighbouringNodes = new List<Node>();

        int yCheck;
        int xCheck;

        //Right side
        xCheck = a_node.gridX + 1;
        yCheck = a_node.gridY;

        if (xCheck >= 0 && xCheck < gridSizeX)
        {
            if (yCheck >= 0 && yCheck < gridSizeY)
            {
                neighbouringNodes.Add(grid[xCheck, yCheck]);
            }
        }

        //Left side
        xCheck = a_node.gridX - 1;
        yCheck = a_node.gridY;

        if (xCheck >= 0 && xCheck < gridSizeX)
        {
            if (yCheck >= 0 && yCheck < gridSizeY)
            {
                neighbouringNodes.Add(grid[xCheck, yCheck]);
            }
        }

        //Top side
        xCheck = a_node.gridX;
        yCheck = a_node.gridY + 1;

        if (xCheck >= 0 && xCheck < gridSizeX)
        {
            if (yCheck >= 0 && yCheck < gridSizeY)
            {
                neighbouringNodes.Add(grid[xCheck, yCheck]);
            }
        }

        ////Bottom side
        xCheck = a_node.gridX;
        yCheck = a_node.gridY - 1;

        if (xCheck >= 0 && xCheck < gridSizeX)
        {
            if (yCheck >= 0 && yCheck < gridSizeY)
            {
                neighbouringNodes.Add(grid[xCheck, yCheck]);
            }
        }

        return neighbouringNodes;
    }


    public Node NodeFromWorldPoint(Vector3 a_worldPos)
    {
        float xPoint = ((a_worldPos.x + gridWorldSize.x / 2) / gridWorldSize.x);
        float yPoint = ((a_worldPos.y + gridWorldSize.y / 2) / gridWorldSize.y);

        xPoint = Mathf.Clamp01(xPoint);
        yPoint = Mathf.Clamp01(yPoint);

        int x = Mathf.RoundToInt((gridSizeX - 1) * xPoint);
        int y = Mathf.RoundToInt((gridSizeY - 1) * yPoint);

        return grid[x, y];
    }

    
    private void OnDrawGizmos()
    {
       // Debug.Log("Called OnDrawGizmos");

        //Draw wire cube with given dimensions
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));

        //If the grid is not empty
        if(grid != null)
        {
            Debug.Log("Loop is working");

            //Loop through every node in grid
            foreach(Node n in grid)
            {
                Debug.Log("Foreach");

                //If current node is a wall node
                if(n.isWall)
                {
                    Debug.Log("current node working");

                    //Set color of node to white
                    Gizmos.color = Color.white;
                }
                else
                {
                    //Set color of node to yellow
                    Gizmos.color = Color.yellow;

                }

                //If final path is not empty
                if(FinalPath != null)
                {
                    //if the current node is in the final path
                    if(FinalPath.Contains(n))
                    {
                        //Set colour to red
                        Gizmos.color = Color.red;
                    }
                }

                //Draw node at the position of node
                Gizmos.DrawCube(n.position, Vector3.one * (nodeDiameter - distance));
            }
        }
    }
}
