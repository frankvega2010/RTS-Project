using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OreMine : MonoBehaviour
{
    public delegate void OnOreMineAction(GameObject oreMineNode);
    public static OnOreMineAction OnOreMineDisabled;

    public float maxGold;
    public bool cantBeMine;

    //[HideInInspector]
    public float currentGold;
    public bool isMarked;
    public GameObject mineModel;
    public GameObject currentFlag;
    // Start is called before the first frame update
    void Start()
    {
        currentGold = maxGold;
    }

    private void OnEnable()
    {
        // Mine Activated
        if(!cantBeMine)
        {
            //Debug.Log("Mine Activated");
            gameObject.tag = "Mine";
            mineModel.SetActive(true);
            currentGold = maxGold;
            //mesh.material.color = Color.red;
            isMarked = false;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float ExtractGold(float amount)
    {
        if(currentGold > 0)
        {
            if (amount <= currentGold)
            {
                currentGold -= amount;
                return amount;
            }
            else
            {
                float goldToGive = currentGold;
                currentGold = 0;

                if (OnOreMineDisabled != null)
                {
                    OnOreMineDisabled(gameObject);
                }

                return goldToGive;
            }
        }

        return 0;
    }

    private void OnDisable()
    {
        if (!cantBeMine)
        {
            // Mine Destroyed
            //Debug.Log("Mine Destroyed");
            gameObject.tag = "Node";
            mineModel.SetActive(false);
            isMarked = false;
            currentGold = 0;
            //mesh.material.color = Color.black;
            //isMarked = false;
            Destroy(currentFlag);
            if(OnOreMineDisabled != null)
            {
                OnOreMineDisabled(gameObject);
            }
        }
            
    }
}
