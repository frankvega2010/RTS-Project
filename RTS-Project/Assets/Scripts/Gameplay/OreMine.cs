using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OreMine : MonoBehaviour
{
    public bool cantBeMine;
    public bool isMarked;
    public MeshRenderer mesh;
    public GameObject currentFlag;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        // Mine Activated
        if(!cantBeMine)
        {
            Debug.Log("Mine Activated");
            tag = "Mine";
            mesh.material.color = Color.red;
            isMarked = false;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDisable()
    {
        if (!cantBeMine)
        {
            // Mine Destroyed
            Debug.Log("Mine Destroyed");
            tag = "Node";
            mesh.material.color = Color.black;
            //isMarked = false;
            Destroy(currentFlag);
        }
            
    }
}
