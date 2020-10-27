using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PathfindingBehaviour))]
public class Villager : MonoBehaviour
{
    public float sightRadius;
    public float walkSpeed;
    public bool autoFindPath;
    private PathfindingBehaviour pathFinding;

    // Start is called before the first frame update
    void Start()
    {
        pathFinding = GetComponent<PathfindingBehaviour>();
        pathFinding.finish = SearchRandomNode();
        transform.position = pathFinding.finish.transform.position;
        if(autoFindPath)
        {
            SearchNewPath();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SearchNewPath();
        }

        if (pathFinding.choosenPath.Count > 0)
        {
            foreach (Node n in pathFinding.choosenPath)
            {
                if (n.nodeParent)
                {
                    Debug.DrawLine(n.transform.position, n.nodeParent.transform.position, Color.green);
                }

            }

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
                        if(autoFindPath)
                        {
                            SearchNewPath();
                        }
                    }
                }
            }
        }
    }

    public void SearchNewPath()
    {
        pathFinding.start = pathFinding.finish;
        pathFinding.finish = SearchRandomNode();
        if (pathFinding.Find(pathFinding.start, pathFinding.finish))
        {
            pathFinding.canGo = true;
            pathFinding.waypointIndex = pathFinding.choosenPath.Count - 1;
        }
    }

    public Node SearchRandomNode()
    {
        Grid grid = Grid.Get();
        if (grid)
        {
            List<Node> pointsFound = new List<Node>();
            foreach (Node n in grid.nodes)
            {
                if (n != pathFinding.start || n != pathFinding.finish)
                {
                    if (Vector3.Distance(transform.position, n.transform.position) <= sightRadius)
                    {
                        pointsFound.Add(n);
                    }
                }
            }

            int randomIndex = Random.Range(0, pointsFound.Count);

            return pointsFound[randomIndex];
        }

        return null;
    }
}
