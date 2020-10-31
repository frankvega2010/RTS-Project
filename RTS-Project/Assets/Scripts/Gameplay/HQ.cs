using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HQ : MonoBehaviour
{
    public delegate void OnGoldAction();
    public static OnGoldAction OnGoldDeposit;
    public static OnGoldAction OnGoldUsed;

    public GameObject villagerPrefab;
    public float villagerCost;
    public Transform villagerSpawnPoint;
    private GameManager gm;
    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.Get();
    }

    // Update is called once per frame
    /*void Update()
    {
        
    }*/

    public void DepositGold(float amount)
    {
        gm.gold += amount;
        if(OnGoldDeposit != null)
        {
            OnGoldDeposit();
        }
    }

    public void SpawnVillager()
    {
        if(gm.gold >= villagerCost)
        {
            gm.gold -= villagerCost;

            //Spawn Villager on spawn point
            GameObject newVillager = Instantiate(villagerPrefab, villagerSpawnPoint.transform.position, villagerSpawnPoint.transform.rotation);

            if (OnGoldUsed != null)
            {
                OnGoldUsed();
            }
        }
    }
}
