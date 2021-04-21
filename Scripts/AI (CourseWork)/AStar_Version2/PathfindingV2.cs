using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingV2 : MonoBehaviour
{
    GridV2 grid;
    public Transform seeker;
    public Transform target;

    private void Awake()
    {
        grid = GetComponent<GridV2>();
    }

    private void Update()
    {
        FindPath(seeker.position, target.position);
    }

    void FindPath(Vector3 startPos, Vector3 targetPos)
    {
        NodeV2 startNode = grid.NodeFromWorldPoint(startPos);
        NodeV2 targetNode = grid.NodeFromWorldPoint(targetPos);

        //Set of nodes to be evaluated
        List<NodeV2> openSet = new List<NodeV2>();
        //Set of nodes already evaluated
        HashSet<NodeV2> closeSet = new HashSet<NodeV2>();

        //Add the start node to open
        openSet.Add(startNode);

        //Loop
        while (openSet.Count > 0)
        {
            //Current = node in open with the lowest fCost
            NodeV2 currentNode = openSet[0];
            for(int i = 1; i < openSet.Count; i++)
            {
                //lowest of the 2
                if(openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost)
                {
                    currentNode = openSet[i];
                }
            }

            //remove currentNNode from open
            openSet.Remove(currentNode);
            //add currentNode to closed
            closeSet.Add(currentNode);

            //If current is the target node then path has been found
            if(currentNode == targetNode)
            {
                RetracePath(startNode, targetNode);
                return;
            }

            //Foreach neighbour of the current node
            foreach(NodeV2 neighbour in grid.GetNeighbours(currentNode))
            {
                //if neighbour is not traversable or neigbour is in closedset
                if(!neighbour.walkable || closeSet.Contains(neighbour))
                {
                    continue;
                }

                //skip to the next neighbour
                int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                //if new path to neighbour is shorter OR neighbour is not in open
                if(newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    //set fcost of neighbour
                    neighbour.gCost = newMovementCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    //set parent of neighbour to currentnode
                    neighbour.parent = currentNode;
                    //if neighbour is not in open
                    if(!openSet.Contains(neighbour))
                    {
                        //add neighbour to open
                        openSet.Add(neighbour);
                    }

                }

            }
        }
    }

    void RetracePath(NodeV2 startNode, NodeV2 endNode)
    {
        List<NodeV2> path = new List<NodeV2>();
        NodeV2 currentNode = endNode;

        while(currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        path.Reverse();

        grid.path = path;
    }

    //hCost (guess work)
    int GetDistance (NodeV2 nodeA, NodeV2 nodeB)
    {
        int distanceX = Mathf.Abs(nodeA.grid_X - nodeB.grid_X);
        int distanceY = Mathf.Abs(nodeA.grid_Y - nodeB.grid_Y);

        if(distanceX > distanceY)
        {
            return 14 * distanceY + 10 * (distanceX - distanceY);
        }

        return 14 * distanceX + 10 * (distanceY - distanceX);
    }
}
