using UnityEngine;

using Pada1.BBCore;           // Code attributes
using Pada1.BBCore.Tasks;     // TaskStatus
using Pada1.BBCore.Framework; // BasePrimitiveAction

[Action("Frank/Mining")]
[Help("Extracts gold from a mine until it reaches its capacity or the mine is out of gold.")]
public class MiningAction : BasePrimitiveAction
{
    // Define the input parameter "bullet" (the prefab to be cloned).
    [InParam("VillagerGO")]
    public GameObject villagerGO;

    private OreMine oreMine;
    private Villager villager;
    private Animator animator;

    // Initialization method. If not established, we look for the shooting point.
    public override void OnStart()
    {
        villager = villagerGO.GetComponent<Villager>();
        animator = villagerGO.GetComponent<Animator>();
        villager.unavailableNodes.Clear();
        oreMine = villager.oreMine.GetComponent<OreMine>();
        Debug.Log("MINING");
        base.OnStart();
    }

    // Main class method, invoked by the execution engine.
    public override TaskStatus OnUpdate()
    {
        villager.extractTimer += Time.deltaTime;

        if(villager.extractTimer >= villager.extractTime)
        {
            villager.extractTimer = 0;
            if(villager.goldHolding < villager.goldCapacity)
            {
                float amountExtracted = oreMine.ExtractGold(villager.extractAmount);
                villager.goldHolding += amountExtracted;
                if (amountExtracted < villager.extractAmount)
                {
                    //Mine is empty, returns to base
                    villager.mineSeen = false;
                    villager.oreMine = null;
                    return TaskStatus.COMPLETED;
                }
            }
            else
            {
                //Capacity is full, returns to base
                return TaskStatus.COMPLETED;
            }
            
        }

        //return TaskStatus.SUSPENDED;
        return TaskStatus.RUNNING;
        //return TaskStatus.FAILED;
    } // OnUpdate

} // class MiningAction
