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
    public bool isReturning;

    // Start is called before the first frame update
    protected new void Start()
    {
        base.Start();

        mineSeen = false;
        hq = GameManager.Get().hq;
        hqSpawnNode = hq.villagerSpawnPoint.GetComponent<Node>();
        MiningAction.OnDestroyMine += DeleteCurrentMineNode;
        //Invoke("InitialSearch", 0.1f);
    }

    public override bool SearchForMine()
    {
        if (!mineSeen)
        {
            pathFinding.start = pathFinding.finish;
            Node originalNode = pathFinding.start;
            oreMineNode = SearchRandomNodeFromFOV();
            if (!isNodeOnUnavailableList(oreMineNode) && oreMineNode != null)
            {
                OreMine oreMineComp = oreMineNode.GetComponent<OreMine>();
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

                    if (pathFinding.Find(startNode, oreMineNode, ID))
                    {
                        //Debug.Log("FOUND MINE BOYS");
                        pathFinding.start = startNode;
                        pathFinding.finish = oreMineNode;
                        mineSeen = true;
                        pathFinding.canGo = true;
                        pathFinding.waypointIndex = pathFinding.choosenPath.Count - 1;
                        unavailableNodes.Clear();
                        unavailableNodes.Add(oreMineNode);
                        return true;
                    }
                    else
                    {
                       // Debug.Log("couldnt find path FOR MINES");
                        unavailableNodes.Add(oreMineNode);
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
            oreMineNode = newOreMine;
            if (!isNodeOnUnavailableList(oreMineNode) && oreMineNode != null)
            {
                OreMine oreMineComp = oreMineNode.GetComponent<OreMine>();
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

                    if (pathFinding.Find(startNode, oreMineNode, ID))
                    {
                        //Debug.Log("FOUND MINE BOYS");
                        pathFinding.start = startNode;
                        pathFinding.finish = oreMineNode;
                        mineSeen = true;
                        pathFinding.canGo = true;
                        pathFinding.waypointIndex = pathFinding.choosenPath.Count - 1;
                        unavailableNodes.Clear();
                        unavailableNodes.Add(oreMineNode);
                        return true;
                    }
                    else
                    {
                        //Debug.Log("couldnt find path FOR MINES");
                        unavailableNodes.Add(oreMineNode);
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
            //Debug.Log("couldnt find path");
            pathFinding.canGo = false;
            transform.rotation *= Quaternion.Euler(0, 180, 0);
            pathFinding.finish = originalNode;
        }

        return false;
    }

    public void DeleteCurrentMineNode(GameObject mineNodeObject)
    {
        if(oreMineNode)
        {
            if (oreMineNode == mineNodeObject.GetComponent<Node>())
            {
                oreMineNode = null;
                mineSeen = false;
            }
        }
    }

    private void OnDestroy()
    {
        MiningAction.OnDestroyMine -= DeleteCurrentMineNode;
    }
}
