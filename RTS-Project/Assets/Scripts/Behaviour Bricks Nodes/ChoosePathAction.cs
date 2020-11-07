using UnityEngine;

using Pada1.BBCore;           // Code attributes
using Pada1.BBCore.Tasks;     // TaskStatus
using Pada1.BBCore.Framework; // BasePrimitiveAction

[Action("Frank/ChoosePath")]
[Help("Chooses a path given a start and finish node.")]
public class ChoosePathAction : BasePrimitiveAction
{
    [InParam("MinerGO")]
    public GameObject minerGO;

    private Miner miner;
    private Animator animator;

    public override void OnStart()
    {
        miner = minerGO.GetComponent<Miner>();
        animator = miner.animator;
        animator.SetBool("Patrol2", true);
        animator.SetBool("Mine2", false);
        animator.SetBool("Return2", false);
        miner.UIComp.UpdateIcon(NPCUI.NPCStates.Patrol);
        base.OnStart();
    }

    // Main class method, invoked by the execution engine.
    public override TaskStatus OnUpdate()
    {
        if (miner.doOnce)
        {
            if (miner.SearchNewPathOnce())
            {
                return TaskStatus.COMPLETED;
            }
            else
            {
                return TaskStatus.FAILED;
            }
        }
        else
        {
            miner.InitialSearch();
            //animator.SetTrigger("Patrol");
            miner.doOnce = true;
            return TaskStatus.COMPLETED;
        }
    } // OnUpdate

} // class ChoosePathAction