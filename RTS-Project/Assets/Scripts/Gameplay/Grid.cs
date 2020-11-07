using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonobehaviourSingleton<Grid>
{
    
    public GameObject nodesParent;
    //public GameObject obstaclesParent;
    public Node[] nodes;

    // Start is called before the first frame update
    void Start()
    {
        nodes = new Node[nodesParent.transform.childCount];
        for (int i = 0; i < nodesParent.transform.childCount; i++)
        {
            nodes[i] = nodesParent.transform.GetChild(i).GetComponent<Node>();
        }

        for (int i = 0; i < nodes.Length; i++)
        {
            nodes[i].SearchNeighbours();
        }
    }

    public void CleanParents(int ID)
    {
        for (int i = 0; i < nodes.Length; i++)
        {
            for (int c = 0; c < nodes[i].nodesParent.Count; c++)
            {
                if(nodes[i].nodesParent[c].villagerID == ID)
                {
                    nodes[i].nodesParent[c].parent = null;
                }
            }
            
        }
    }

    public void CleanCosts()
    {
        for (int i = 0; i < nodes.Length; i++)
        {
            nodes[i].hCost = float.MaxValue;
            nodes[i].gCost = float.MaxValue;
            nodes[i].fCost = float.MaxValue;
        }
    }
}
