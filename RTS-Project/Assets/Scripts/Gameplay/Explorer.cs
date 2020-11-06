using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explorer : NPC
{
    [Header("Explorer Config")]
    public GameObject flagPrefab;
    

    // Start is called before the first frame update
    protected new void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        
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
                OreMine o = oreMineNode.GetComponent<OreMine>();
                if(!o.isMarked)
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
}
