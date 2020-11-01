using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Sight : MonoBehaviour
{
    [Header("Main Config")]
    [Tooltip("The tag this object needs to find")]
    public string tagToFind;
    public float fovAngle;
    public LayerMask mask;
    public SphereCollider detectionCol;

    [Header("Private Variables")] //Hide in inspector later
    public bool objectInSight;
    public List<GameObject> objectsFound;
    // Start is called before the first frame update
    void Awake()
    {
        objectsFound = new List<GameObject>();
    }

    private void OnTriggerStay(Collider other)
    {
       // Debug.Log("messi");
        //Debug.Log(other.gameObject.tag);

        if (other.gameObject.tag == tagToFind)
        {
            objectInSight = false;

            Vector3 direction = other.transform.position - transform.position;
            float angle = Vector3.Angle(direction, transform.forward);

            if (angle < fovAngle * 0.5f)
            {
                RaycastHit hit;

                if (Physics.Raycast(transform.position, direction.normalized, out hit, detectionCol.radius * 1.2f, mask))
                {
                    if (hit.transform.gameObject.tag == "Wall")
                    {
                        //?
                    }
                    else if (hit.transform.gameObject.tag == tagToFind)
                    {
                        objectInSight = true;
                        if(!objectsFound.Contains(hit.transform.gameObject))
                        {
                            objectsFound.Add(hit.transform.gameObject);
                        }
                        //Debug.Log(other.gameObject.name);
                    }
                }
            }
            else
            {
                if (objectsFound.Contains(other.transform.gameObject))
                {
                    objectsFound.Remove(other.transform.gameObject);
                }

                objectInSight = false;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == tagToFind)
        {
            if (objectsFound.Contains(other.transform.gameObject))
            {
                objectsFound.Remove(other.transform.gameObject);
            }

            objectInSight = false;
        }
    }

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        angleInDegrees += transform.eulerAngles.y;

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
