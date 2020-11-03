using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [Header("NPC Config")]
    public float sightRadius;
    public float walkSpeed;
    public bool autoFindPath;

    [Header("Component Config")]
    public Sight sight;

    public static int NPCCount;
    public int ID;
    public Color color;
    public bool mineSeen;
    public Node oreMine;
    public bool doOnce;

    public PathfindingBehaviour pathFinding;
    public List<Node> unavailableNodes;

    // Start is called before the first frame update
    protected void Start()
    {
        NPCCount++;
        ID = NPCCount;
        pathFinding = GetComponent<PathfindingBehaviour>();
        unavailableNodes = new List<Node>();
    }

    /*// Update is called once per frame
    void Update()
    {
        
    }*/

    public virtual bool SearchForMine()
    {
        Debug.Log("Shouldnt get here");
        return false;
    }

    public void SearchNewPath()
    {
        pathFinding.start = pathFinding.finish;
        pathFinding.finish = SearchRandomNodeFromRadius();
        Node originalNode = pathFinding.start;
        if (pathFinding.Find(pathFinding.start, pathFinding.finish, ID))
        {
            pathFinding.canGo = true;
            pathFinding.waypointIndex = pathFinding.choosenPath.Count - 1;
        }
        else
        {
            Debug.Log("couldnt find path");
            pathFinding.canGo = false;
            transform.rotation *= Quaternion.Euler(0, 180, 0);
            pathFinding.finish = originalNode;
            SearchNewPath();
        }
    }

    public bool SearchNewPathOnce()
    {
        pathFinding.start = pathFinding.finish;
        pathFinding.finish = SearchRandomNodeFromRadius();
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
            //SearchNewPath();
        }

        return false;
    }

    public void InitialSearch()
    {
        pathFinding.finish = GameManager.Get().hq.villagerSpawnPoint.GetComponent<Node>();
        //pathFinding.finish = SearchRandomNodeFromRadius();
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
            if (n == currentNode)
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
                        if (n.gameObject.tag != sight.tagToFind && n.isWalkable)
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
