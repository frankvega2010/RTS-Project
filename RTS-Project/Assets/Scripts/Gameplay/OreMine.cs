using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OreMine : MonoBehaviour
{
    public delegate void OnOreMineAction(GameObject oreMineNode);
    public static OnOreMineAction OnOreMineDisabled;

    public float maxGold;
    public bool cantBeMine;
    public GameObject mineModel;

    [HideInInspector]
    public float currentGold;
    [HideInInspector]
    public bool isMarked;
    [HideInInspector]
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
            gameObject.tag = "Mine";
            mineModel.SetActive(true);
            currentGold = maxGold;
            isMarked = false;
        }
        
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
            gameObject.tag = "Node";
            mineModel.SetActive(false);
            isMarked = false;
            currentGold = 0;
            Destroy(currentFlag);
            if(OnOreMineDisabled != null)
            {
                OnOreMineDisabled(gameObject);
            }
        }
            
    }
}
