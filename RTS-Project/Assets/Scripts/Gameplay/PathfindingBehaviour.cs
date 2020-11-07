using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingBehaviour : MonoBehaviour
{
    [Header("Config")]
    public LayerMask mask;
    public float raycastDistance;
    public float speed;
    public float changeWaypointDistance;


    [Header("View Current Properties"), Space]
    public bool canGo;
    public int waypointIndex;
    public Node start;
    public Node finish;
    private List<Node> openNodes = new List<Node>();
    private List<Node> closedNodes = new List<Node>();
    [HideInInspector]
    public List<Node> choosenPath = new List<Node>();
    private Grid grid;
    // Start is called before the first frame update
    void Start()
    {
        grid = Grid.Get();
    }

    public bool Find(Node start, Node finish, int ID)
    {
        //List<Node> finalPath = new List<Node>();
        if ((start && finish) && start != finish)
        {
            grid.CleanCosts();
            grid.CleanParents(ID);
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
                            current.SetgCost(n);
                            float gCost = current.gCost + Vector3.Distance(current.transform.position, n.transform.position);
                            if (gCost < n.gCost || !openNodes.Contains(n))
                            {
                                //Chequear si hay un obstaculo entre medio del current y el neighbour.
                                Vector3 direction = n.transform.position - current.transform.position;
                                RaycastHit hit;
                                bool hitObstacle = false;

                                if (Physics.Raycast(current.transform.position, direction.normalized, out hit, raycastDistance, mask))
                                {
                                    if (hit.transform.gameObject.tag == "Wall")
                                    {
                                        hitObstacle = true;
                                    }
                                }

                                //Debug.DrawRay(current.transform.position, direction.normalized * raycastDistance, Color.white);

                                if (!hitObstacle)
                                {
                                    n.gCost = gCost;
                                    n.SethCost(finish);
                                    n.SetfCost();

                                    if (n.GetParent(ID).villagerID != -1)
                                    {
                                        n.GetParent(ID).parent = current.gameObject;
                                    }
                                    else
                                    {
                                        Node.nodeParent parent = new Node.nodeParent();
                                        parent.villagerID = ID;
                                        parent.parent = current.gameObject;
                                        n.nodesParent.Add(parent);
                                    }

                                    /*n.nodeParent = current.gameObject;*/
                                    if (!openNodes.Contains(n))
                                    {
                                        openNodes.Add(n);
                                    }
                                }
                                
                            }
                        }
                    }
                }
            }

            //Get Final Path with all the parents
            while (current.GetParent(ID).parent)
            {
                choosenPath.Add(current);
                current = current.GetParent(ID).parent.GetComponent<Node>();
            }

            choosenPath.Add(current);

            return couldFindPath;
        }
        else
        {
            return false;
        }
    }

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
