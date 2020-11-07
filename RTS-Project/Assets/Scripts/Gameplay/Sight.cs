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

    [HideInInspector]
    public bool objectInSight;
    [HideInInspector]
    public List<GameObject> objectsFound;
    // Start is called before the first frame update
    void Awake()
    {
        objectsFound = new List<GameObject>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == tagToFind)
        {
            objectInSight = false;
            //Agregar los que entren en el trigger, y despues chequear si estan dentro del angulo, cuando el npc tenga que buscar.

            if (!objectsFound.Contains(other.transform.gameObject))
            {
                objectsFound.Add(other.transform.gameObject);
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
