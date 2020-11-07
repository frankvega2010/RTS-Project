using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PathfindingBehaviour))]
public class Miner : NPC
{
    [Header("Miner Config")]
    public float extractTime;
    public float extractAmount;
    public float goldCapacity;

    [HideInInspector]
    public float goldHolding;
    [HideInInspector]
    public float extractTimer;
    [HideInInspector]
    public HQ hq;
    [HideInInspector]
    public Node hqSpawnNode;
    [HideInInspector]
    public bool doOnceMine;
    [HideInInspector]
    public bool isReturning;

    // Start is called before the first frame update
    protected new void Start()
    {
        base.Start();

        mineSeen = false;
        oreMineNode = null;
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
            if (!isNodeOnUnavailableList(oreMineNode) && oreMineNode)
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
                        oreMineNode = null;
                    }
                }
                else
                {
                    unavailableNodes.Add(oreMineNode);
                    pathFinding.finish = originalNode;
                    mineSeen = false;
                    oreMineNode = null;
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
                        unavailableNodes.Add(oreMineNode);
                        pathFinding.finish = originalNode;
                        mineSeen = false;
                        oreMineNode = null;
                    }
                }
                else
                {
                    unavailableNodes.Add(oreMineNode);
                    pathFinding.finish = originalNode;
                    mineSeen = false;
                    oreMineNode = null;
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
            //TP TO SPAWNPOINT
            pathFinding.choosenPath.Clear();
            pathFinding.choosenPath.Add(hqSpawnNode);
            pathFinding.canGo = true;
            pathFinding.waypointIndex = 0;
            transform.position = hqSpawnNode.transform.position;
            pathFinding.start = hqSpawnNode;
            pathFinding.finish = hqSpawnNode;
        }

        return true;
    }

    public void DeleteCurrentMineNode(GameObject mineNodeObject)
    {
        if(oreMineNode)
        {
            if (oreMineNode == mineNodeObject.GetComponent<Node>())
            {
                Destroy(oreMineNode.GetComponent<OreMine>().currentFlag);
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
