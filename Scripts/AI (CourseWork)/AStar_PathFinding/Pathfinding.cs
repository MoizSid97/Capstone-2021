using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    //https://www.youtube.com/watch?v=AKKpPmxx07w&t=403s&ab_channel=Daniel

    Grids gridRef;
    public Transform startPos;
    public Transform targetPos;

    private void Awake()
    {
        gridRef = GetComponent<Grids>();
    }

    // Update is called once per frame
    void Update()
    {
        FindPath(startPos.position, targetPos.position);
    }

    void FindPath(Vector3 a_startPos, Vector3 a_targetPos)
    {
        Node StartNode = gridRef.NodeFromWorldPoint(a_startPos);
        Node TargetNode = gridRef.NodeFromWorldPoint(a_targetPos);

        //List of nodes for the open lis
        List<Node> OpenList = new List<Node>();
        //Hashset of nodes for the closed list
        HashSet<Node> ClosedList = new HashSet<Node>();

        OpenList.Add(StartNode);

        while(OpenList.Count > 0)
        {
            Node currentNode = OpenList[0];
            for(int i = 1; i < OpenList.Count; i++)
            {
                if (OpenList[i].fCost < currentNode.fCost || OpenList[i].fCost == currentNode.fCost && OpenList[i].hCost < currentNode.hCost)
                {
                    currentNode = OpenList[i];
                }
            }

            OpenList.Remove(currentNode);
            ClosedList.Add(currentNode);

            if(currentNode == TargetNode)
            {
                GetFinalPath(StartNode, TargetNode);
            }

            foreach(Node neightbourNode in gridRef.GetNeighbouringNodes(currentNode))
            {
                if(!neightbourNode.isWall || ClosedList.Contains(neightbourNode))
                {
                    continue;
                }

                int moveCost = currentNode.gCost + GetManhattenDistance(currentNode, neightbourNode);

                if(moveCost < neightbourNode.gCost || !OpenList.Contains(neightbourNode))
                {
                    neightbourNode.gCost = moveCost;
                    neightbourNode.hCost = GetManhattenDistance(neightbourNode, TargetNode);
                    neightbourNode.parent = currentNode;

                    if(!OpenList.Contains(neightbourNode))
                    {
                        OpenList.Add(neightbourNode);
                    }
                }
            }

        }
    }

    void GetFinalPath(Node a_startingNode, Node a_endNode)
    {
        List<Node> FinalPath = new List<Node>();
        Node currentNode = a_endNode;

        while(currentNode != a_startingNode)
        {
            FinalPath.Add(currentNode);
            currentNode = currentNode.parent;
        }

        FinalPath.Reverse();

        gridRef.FinalPath = FinalPath;
    }

    int GetManhattenDistance(Node a_nodeA, Node a_nodeB)
    {
        //x1-x2
        int iX = Mathf.Abs(a_nodeA.gridX - a_nodeB.gridX);
        //y1-y2
        int iY = Mathf.Abs(a_nodeA.gridY - a_nodeB.gridY);

        return iX + iY;
    }
}
