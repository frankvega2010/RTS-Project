using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonobehaviourSingleton<Grid>
{
    
    public GameObject nodesParent;
    public Node[] nodes;

    // Start is called before the first frame update
    void Start()
    {
        nodes = new Node[nodesParent.transform.childCount];
        for (int i = 0; i < nodes.Length; i++)
        {
            nodes[i] = nodesParent.transform.GetChild(i).GetComponent<Node>();
            
        }

        for (int i = 0; i < nodes.Length; i++)
        {
            nodes[i].SearchNeighbours();
        }
    }

    public void CleanParents()
    {
        for (int i = 0; i < nodes.Length; i++)
        {
            nodes[i].nodeParent = null;
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
    // Update is called once per frame
   /* void Update()
    {
        
    }*/
}
