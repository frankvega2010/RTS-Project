using UnityEngine;

using Pada1.BBCore;           // Code attributes
using Pada1.BBCore.Tasks;     // TaskStatus
using Pada1.BBCore.Framework; // BasePrimitiveAction

[Action("Frank/ChooseMinePath")]
[Help("Chooses a path given a start and finish node.")]
public class ChooseMinePathAction : BasePrimitiveAction
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
        miner.UIComp.UpdateIcon(NPCUI.NPCStates.Mine);
        base.OnStart();
    }

    // Main class method, invoked by the execution engine.
    public override TaskStatus OnUpdate()
    {
        if(!miner.doOnceMine)
        {
            miner.doOnceMine = true;
            if (miner.SearchForMine(miner.oreMineNode))
            {
                return TaskStatus.COMPLETED;
            }
            else
            {
                return TaskStatus.FAILED;
            }
        }

        return TaskStatus.COMPLETED;

    } // OnUpdate

} // class ChooseMinePathAction