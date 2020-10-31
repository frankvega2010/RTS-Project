using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PathfindingBehaviour))]
public class Villager : MonoBehaviour
{
    public float sightRadius;
    public float walkSpeed;
    public bool autoFindPath;
    public Sight sight;
    public bool mineSeen;
    public Node oreMine;
    private PathfindingBehaviour pathFinding;
    public List<Node> unavailableNodes;
    

    // Start is called before the first frame update
    void Start()
    {
        pathFinding = GetComponent<PathfindingBehaviour>();
        mineSeen = false;
        unavailableNodes = new List<Node>();
        Invoke("InitialSearch", 0.1f);
        
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
        }
    }

    public bool SearchForMine()
    {
        if (!mineSeen)
        {
            oreMine = SearchRandomNodeFromFOV();
            if (!isNodeOnUnavailableList(oreMine) && oreMine != null)
            {
                Node startNode = null;

                if(pathFinding.choosenPath[pathFinding.waypointIndex])
                {
                    startNode = pathFinding.choosenPath[pathFinding.waypointIndex];
                }
                else
                {
                    startNode = pathFinding.finish;
                }

                if (pathFinding.Find(startNode, oreMine))
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
                    mineSeen = false;
                }
            }
        }

        return false;
    }

    public void SearchNewPath()
    {
        
        pathFinding.start = pathFinding.finish;
        pathFinding.finish = SearchRandomNodeFromRadius();
        Node originalNode = pathFinding.start;
        if (pathFinding.Find(pathFinding.start, pathFinding.finish))
        {
            pathFinding.canGo = true;
            pathFinding.waypointIndex = pathFinding.choosenPath.Count - 1;
        }
        else
        {
            Debug.Log("couldnt find path I think");
            pathFinding.canGo = false;
            transform.rotation *= Quaternion.Euler(0, 180, 0);
            pathFinding.finish = originalNode;
            SearchNewPath();
        }
    }

    public void InitialSearch()
    {
        pathFinding.finish = SearchRandomNodeFromRadius();
        transform.position = pathFinding.finish.transform.position;
        if (autoFindPath)
        {
            SearchNewPath();
        }
    }

    public bool isNodeOnUnavailableList(Node currentNode)
    {
        foreach (Node n in unavailableNodes)
        {
            if(n == currentNode)
            {
                return true;
            }
        }

        return false;
    }

    public Node SearchRandomNodeFromFOV()
    {
        if (sight.objectsFound.Count > 0)
        {
            List<Node> pointsFound = new List<Node>();

            foreach (GameObject g in sight.objectsFound)
            {
                Node n = g.GetComponent<Node>();
                if (n != pathFinding.start || n != pathFinding.finish && n.isWalkable)
                {
                    pointsFound.Add(n);
                }
            }

            if (pointsFound.Count > 0)
            {
                int randomIndex = Random.Range(0, pointsFound.Count - 1);

                return pointsFound[randomIndex];
            }
            else
            {
                return null;
            }

        }

        return null;
    }

    public Node SearchRandomNodeFromRadius()
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
                        if(n.gameObject.tag != sight.tagToFind && n.isWalkable)
                        {
                            pointsFound.Add(n);
                        }

                        //pointsFound.Add(n);

                    }
                }
            }

            int randomIndex = Random.Range(0, pointsFound.Count);

            return pointsFound[randomIndex];
        }

        return null;
    }
}
