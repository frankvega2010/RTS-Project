using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    [System.Serializable]
    public class nodeParent 
    {
        public int villagerID;
        public GameObject parent;
    };

    public float neighbourRadius;
    public float gCost = float.MaxValue;
    public float hCost = float.MaxValue;
    public float fCost = float.MaxValue;
    
    public List<GameObject> neighbours = new List<GameObject>();
    public List<nodeParent> nodesParent = new List<nodeParent>();
    public bool isWalkable;

    // Start is called before the first frame update
    void Start()
    {
        //Search for neighbours
        //SearchNeighbours();
    }

    void OnMouseOver()
    {
        //If your mouse hovers over the GameObject with the script attached, output this message
        //Debug.Log("Mouse is over GameObject.");

       // if (Input.GetMouseButtonDown(0)) PathfindingBehaviour.start = this;
       // if (Input.GetMouseButtonDown(1)) PathfindingBehaviour.finish = this;
        //if (Input.GetMouseButtonDown(2)) Debug.Log("Pressed middle click.");
    }

    public void SearchNeighbours()
    {
        Grid grid = Grid.Get();
        if(grid)
        {
            neighbours = new List<GameObject>();
            foreach(Node n in grid.nodes)
            {
                if(n != this)
                {
                    if (Vector3.Distance(transform.position, n.transform.position) <= neighbourRadius)
                    {
                        neighbours.Add(n.gameObject);
                    }
                }
            }
        }
    }

    public void SetgCost(Node current) 
    {
        if(current)
        {
            gCost = Vector3.Distance(current.transform.position, transform.position);
        }
    }

    public void SethCost(Node finish)
    {
        if (finish)
        {
            hCost = Vector3.Distance(transform.position, finish.transform.position);
        }
    }

    public void SetfCost()
    {
        fCost = gCost + hCost;
    }

    public nodeParent GetParent(int ID)
    {
        nodeParent newNode = new nodeParent();
        newNode.villagerID = -1;
        for (int i = 0; i < nodesParent.Count; i++)
        {
            if(nodesParent[i].villagerID == ID)
            {
                newNode = nodesParent[i];
                i = nodesParent.Count;
            }
        }

        return newNode;
    }
    public void CleanParent(int ID)
    {
        for (int i = 0; i < nodesParent.Count; i++)
        {
            if (nodesParent[i].villagerID == ID)
            {
                nodesParent[i].parent = null;
                i = nodesParent.Count;
            }
        }

        
    }
}
