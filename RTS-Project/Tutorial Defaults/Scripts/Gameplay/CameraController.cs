using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(Input.GetAxis("Vertical") * speed * Time.deltaTime, 0, Input.GetAxis("Horizontal") * -speed * Time.deltaTime);
    }
}
