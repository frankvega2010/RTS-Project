using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PathfindingBehaviour))]
public class Villager : NPC
{
    [Header("Villager Config")]
    public float extractTime;
    public float extractAmount;
    public float goldCapacity;


    public float goldHolding;
    public float extractTimer;
    [HideInInspector]
    public HQ hq;
    public Node hqSpawnNode;
    public bool doOnceMine;
    
    // Start is called before the first frame update
    protected new void Start()
    {
        base.Start();

        mineSeen = false;
        hq = GameManager.Get().hq;
        hqSpawnNode = hq.villagerSpawnPoint.GetComponent<Node>();
        //Invoke("InitialSearch", 0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        /*if (Input.GetMouseButtonDown(0))
        {
            SearchNewPath();
        }

        if (pathFinding.choosenPath.Count > 0)
        {
            foreach (Node n in pathFinding.choosenPath)
            {
                for (int c = 0; c < n.nodesParent.Count; c++)
                {
                    if (n.nodesParent[c].villagerID == ID)
                    {
                        if(n.nodesParent[c].parent)
                        {
                            Debug.DrawLine(n.transform.position, n.nodesParent[c].parent.transform.position, color);
                        }
                    }
                }
            }

            SearchForMine();

            if (pathFinding.canGo)
            {
                transform.position = Vector3.MoveTowards(transform.position, pathFinding.choosenPath[pathFinding.waypointIndex].transform.position, Time.deltaTime * walkSpeed);
                transform.LookAt(pathFinding.choosenPath[pathFinding.waypointIndex].transform);

                if (Vector3.Distance(transform.position, pathFinding.choosenPath[pathFinding.waypointIndex].transform.position) <= pathFinding.changeWaypointDistance)
                {
                    pathFinding.waypointIndex--;
                    if (pathFinding.waypointIndex < 0)
                    {
                        pathFinding.waypointIndex = 0;
                        pathFinding.canGo = false;
                        if (pathFinding.choosenPath[pathFinding.waypointIndex] == oreMine)
                        {
                            mineSeen = false;
                            oreMine = null;
                        }
                        if (autoFindPath)
                        {
                            if(!SearchForMine())
                            {
                                SearchNewPath();
                            }
                        }
                    }
                }
            }
        }*/
    }

    public override bool SearchForMine()
    {
        if (!mineSeen)
        {
            pathFinding.start = pathFinding.finish;
            Node originalNode = pathFinding.start;
            oreMine = SearchRandomNodeFromFOV();
            if (!isNodeOnUnavailableList(oreMine) && oreMine != null)
            {
                OreMine oreMineComp = oreMine.GetComponent<OreMine>();
                if(oreMineComp.isMarked)
                {
                    Node startNode = null;

                    if (pathFinding.choosenPath[pathFinding.waypointIndex])
                    {
                        startNode = pathFinding.choosenPath[pathFinding.waypointIndex];
                    }
                    else
                    {
                        startNode = pathFinding.finish;
                    }

                    if (pathFinding.Find(startNode, oreMine, ID))
                    {
                        Debug.Log("FOUND MINE BOYS");
                        pathFinding.start = startNode;
                        pathFinding.finish = oreMine;
                        mineSeen = true;
                        pathFinding.canGo = true;
                        pathFinding.waypointIndex = pathFinding.choosenPath.Count - 1;
                        unavailableNodes.Clear();
                        unavailableNodes.Add(oreMine);
                        return true;
                    }
                    else
                    {
                        Debug.Log("couldnt find path FOR MINES");
                        unavailableNodes.Add(oreMine);
                        pathFinding.finish = originalNode;
                        mineSeen = false;
                    }
                }
            }
        }

        return false;
    }

    public bool SearchForMine(Node newOreMine)
    {
        if (mineSeen)
        {
            pathFinding.start = pathFinding.finish;
            Node originalNode = pathFinding.start;
            oreMine = newOreMine;
            if (!isNodeOnUnavailableList(oreMine) && oreMine != null)
            {
                OreMine oreMineComp = oreMine.GetComponent<OreMine>();
                if (oreMineComp.isMarked)
                {
                    Node startNode = null;

                    if (pathFinding.choosenPath[pathFinding.waypointIndex])
                    {
                        startNode = pathFinding.choosenPath[pathFinding.waypointIndex];
                    }
                    else
                    {
                        startNode = pathFinding.finish;
                    }

                    if (pathFinding.Find(startNode, oreMine, ID))
                    {
                        Debug.Log("FOUND MINE BOYS");
                        pathFinding.start = startNode;
                        pathFinding.finish = oreMine;
                        mineSeen = true;
                        pathFinding.canGo = true;
                        pathFinding.waypointIndex = pathFinding.choosenPath.Count - 1;
                        unavailableNodes.Clear();
                        unavailableNodes.Add(oreMine);
                        return true;
                    }
                    else
                    {
                        Debug.Log("couldnt find path FOR MINES");
                        unavailableNodes.Add(oreMine);
                        pathFinding.finish = originalNode;
                        mineSeen = false;
                    }
                }
            }
        }

        return false;
    }

    public bool SearchNewPathToHQ()
    {
        pathFinding.start = pathFinding.finish;
        pathFinding.finish = hqSpawnNode;
        Node originalNode = pathFinding.start;
        if (pathFinding.Find(pathFinding.start, pathFinding.finish, ID))
        {
            pathFinding.canGo = true;
            pathFinding.waypointIndex = pathFinding.choosenPath.Count - 1;
            return true;
        }
        else
        {
            Debug.Log("couldnt find path");
            pathFinding.canGo = false;
            transform.rotation *= Quaternion.Euler(0, 180, 0);
            pathFinding.finish = originalNode;
        }

        return false;
    }
}
