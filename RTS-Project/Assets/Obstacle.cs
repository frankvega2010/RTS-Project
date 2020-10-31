using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public List<Node> childNodes = new List<Node>();

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            childNodes.Add(transform.GetChild(i).GetComponent<Node>());
        }

        /*Grid grid = Grid.Get();
        grid.nodes.d*/
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
