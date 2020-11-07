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

    [InParam("MinerGO")]
    public GameObject minerGO;

    private OreMine oreMine;
    private Miner miner;
    private Animator animator;

    public override void OnStart()
    {
        miner = minerGO.GetComponent<Miner>();
        animator = miner.animator;
        miner.unavailableNodes.Clear();
        oreMine = miner.oreMineNode.GetComponent<OreMine>();
        animator.SetBool("Patrol2", false);
        animator.SetBool("Mine2", true);
        animator.SetBool("Return2", false);
        miner.UIComp.UpdateIcon(NPCUI.NPCStates.Mine);
        base.OnStart();
    }

    // Main class method, invoked by the execution engine.
    public override TaskStatus OnUpdate()
    {
        miner.extractTimer += Time.deltaTime;

        if(miner.extractTimer >= miner.extractTime)
        {
            miner.extractTimer = 0;
            if(miner.goldHolding < miner.goldCapacity)
            {
                float amountExtracted = oreMine.ExtractGold(miner.extractAmount);
                miner.goldHolding += amountExtracted;
                miner.UIComp.UpdateText("x"+miner.goldHolding);
                if (amountExtracted < miner.extractAmount)
                {
                    //Mine is empty, returns to base
                    if(OnDestroyMine != null)
                    {
                        OnDestroyMine(miner.oreMineNode.gameObject);
                    }
 
                    miner.mineSeen = false;
                    miner.oreMineNode = null;
                    miner.isReturning = true;
                    return TaskStatus.COMPLETED;
                }
            }
            else
            {
                //Capacity is full, returns to base
                miner.isReturning = true;
                return TaskStatus.COMPLETED;
            }
            
        }

        return TaskStatus.RUNNING;
    } // OnUpdate

} // class MiningAction
