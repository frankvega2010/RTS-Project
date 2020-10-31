using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingBehaviour : MonoBehaviour
{
    public float speed;
    public float changeWaypointDistance;
    public bool canGo;
    public int waypointIndex;
    public Node start;
    public Node finish;
    public List<Node> openNodes = new List<Node>();
    public List<Node> closedNodes = new List<Node>();
    public List<Node> choosenPath = new List<Node>();
    private Grid grid;
    // Start is called before the first frame update
    void Start()
    {
        grid = Grid.Get();
    }

    // Update is called once per frame
    void Update()
    {
        

        if (Input.GetMouseButtonDown(2))
        {
            if(grid)
            {
                if(start && finish)
                {
                   /* choosenPath =*/ Find(start, finish);
                    canGo = true;
                    waypointIndex = choosenPath.Count-1;
                }
                
            }
        }
    }


    public bool Find(Node start, Node finish)
    {
        //List<Node> finalPath = new List<Node>();
        if (start && finish)
        {
            grid.CleanCosts();
            grid.CleanParents();
            choosenPath.Clear();
            openNodes.Clear();
            closedNodes.Clear();
            openNodes.Add(start);
            Node current = null;
            bool couldFindPath = false;

            while (openNodes.Count > 0)
            {
                current = GetLowestFCost();
                openNodes.Remove(current);
                closedNodes.Add(current);

                if (current == finish)
                {
                    openNodes.Clear();
                    couldFindPath = true;
                    break;
                }

                //current.SethCost(finish);

                foreach (GameObject g in current.neighbours)
                {
                    Node n = g.GetComponent<Node>();
                    if (n != current)
                    {
                        if (n.isWalkable && !closedNodes.Contains(n))
                        {
                            //Check if new path is shorter
                            float gCost = current.gCost + Vector3.Distance(current.transform.position, n.transform.position);
                            if (gCost < n.gCost || !openNodes.Contains(n))
                            {
                                //Chequear si hay un obstaculo entre medio del current y el neighbour.
                                //raycast?

                                n.gCost = gCost;
                                n.SethCost(finish);
                                n.SetfCost();
                                n.nodeParent = current.gameObject;
                                if (!openNodes.Contains(n))
                                {
                                    openNodes.Add(n);
                                }
                            }
                        }
                    }
                }
            }


            //Get Final Path with all the parents
            while (current.nodeParent)
            {
                choosenPath.Add(current);
                current = current.nodeParent.GetComponent<Node>();
            }

            choosenPath.Add(current);

            return couldFindPath;
        }
        else
        {
            return false;
        }
            

        
        //return finalPath;
    }

    /*public GetFinalPath()
    {

    }*/

    public Node GetLowestFCost()
    {
        float lowestFCost = float.MaxValue;
        Node lowestFCostNode = null;

        foreach(Node n in openNodes)
        {
            if(n.fCost <= lowestFCost)
            {
                lowestFCost = n.fCost;
                lowestFCostNode = n;
            }
        }

        return lowestFCostNode;
    }
}
