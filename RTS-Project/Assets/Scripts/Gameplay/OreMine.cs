using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OreMine : MonoBehaviour
{
    public bool isExplored;
    public MeshRenderer mesh;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        // Mine Activated
        Debug.Log("Mine Activated");
        mesh.material.color = Color.red;
        isExplored = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
