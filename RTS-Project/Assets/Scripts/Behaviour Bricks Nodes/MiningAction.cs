using UnityEngine;

using Pada1.BBCore;           // Code attributes
using Pada1.BBCore.Tasks;     // TaskStatus
using Pada1.BBCore.Framework; // BasePrimitiveAction

[Action("Frank/Mining")]
[Help("Extracts gold from a mine until it reaches its capacity or the mine is out of gold.")]
public class MiningAction : BasePrimitiveAction
{
    public delegate void OnMiningAction(GameObject mine);
    public static OnMiningAction OnDestroyMine;

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
        animator = villager.animator;
        villager.unavailableNodes.Clear();
        oreMine = villager.oreMineNode.GetComponent<OreMine>();
        animator.SetBool("Patrol2", false);
        animator.SetBool("Mine2", true);
        animator.SetBool("Return2", false);
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
                    if(OnDestroyMine != null)
                    {
                        OnDestroyMine(villager.oreMineNode.gameObject);
                    }
                    villager.mineSeen = false;
                    villager.oreMineNode = null;
                    villager.isReturning = true;
                    return TaskStatus.COMPLETED;
                }
            }
            else
            {
                //Capacity is full, returns to base
                villager.isReturning = true;
                return TaskStatus.COMPLETED;
            }
            
        }

        //return TaskStatus.SUSPENDED;
        return TaskStatus.RUNNING;
        //return TaskStatus.FAILED;
    } // OnUpdate

} // class MiningAction
