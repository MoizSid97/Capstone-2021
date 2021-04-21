using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridV2 : MonoBehaviour
{
    public LayerMask obstacle;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    //public Transform testPlayer;
    NodeV2[,] grid;

    private float nodeDiameter;
    private int sizeX;
    private int sizeY;

    private void Start()
    {
        nodeDiameter = nodeRadius * 2;
        
        sizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        sizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);

        CreateGrid();
    }

    void CreateGrid()
    {
        grid = new NodeV2[sizeX, sizeY];

        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;

        for(int x = 0; x < sizeX; x ++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, obstacle));
                grid[x, y] = new NodeV2(walkable, worldPoint, x, y);
            }
        }
    }

    public List<NodeV2> GetNeighbours(NodeV2 node)
    {
        List<NodeV2> neighbours = new List<NodeV2>();

        for(int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if(x == 0 && y == 0)
                {
                    continue;
                }

                int checkX = node.grid_X + x;
                int checkY = node.grid_Y + y;

                if(checkX >= 0 && checkX < sizeX && checkY >= 0 && checkY < sizeY)
                {
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }

        return neighbours;
    }

    public NodeV2 NodeFromWorldPoint(Vector3 worldPosition)
    {
        float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPosition.z + gridWorldSize.y / 2) / gridWorldSize.y;

        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((sizeX - 1) * percentX);
        int y = Mathf.RoundToInt((sizeY - 1) * percentY);

        return grid[x, y];
    }

    public List<NodeV2> path;

    private void OnDrawGizmos()
    {
        //Draw world grid
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));

        //Visualize our code in void CreateGrid()
        if(grid != null)
        {
            //Gizmo for player
            //NodeV2 playerNode = NodeFromWorldPoint(testPlayer.position);

            foreach(NodeV2 n in grid)
            {
                Gizmos.color = (n.walkable) ? Color.white : Color.red;

                //if(playerNode == n)
                //{
                   // Gizmos.color = Color.cyan;
                //}

                if(path != null)
                {
                    if(path.Contains(n))
                    {
                        Gizmos.color = Color.green;
                    }
                }

                Gizmos.DrawCube(n.worldPos, Vector3.one * (nodeDiameter - .1f));
            }
        }
    }
}
