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
            oreMine = SearchRandomNodeFromFOV();
            if (!isNodeOnUnavailableList(oreMine) && oreMine != null)
            {
                OreMine o = oreMine.GetComponent<OreMine>();
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
}
