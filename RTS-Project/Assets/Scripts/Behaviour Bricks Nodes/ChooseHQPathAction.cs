using UnityEngine;

using Pada1.BBCore;           // Code attributes
using Pada1.BBCore.Tasks;     // TaskStatus
using Pada1.BBCore.Framework; // BasePrimitiveAction

[Action("Frank/ChooseHQPath")]
[Help("Chooses a path given a start and finish node.")]
public class ChooseHQPathAction : BasePrimitiveAction
{
    [InParam("MinerGO")]
    public GameObject minerGO;

    private Miner miner;
    private Animator animator;

    public override void OnStart()
    {
        miner = minerGO.GetComponent<Miner>();
        animator = miner.animator;
        miner.UIComp.UpdateIcon(NPCUI.NPCStates.Return);
        base.OnStart();
    }

    // Main class method, invoked by the execution engine.
    public override TaskStatus OnUpdate()
    {
        if(miner.SearchNewPathToHQ())
        {
            return TaskStatus.COMPLETED;
        }
        else
        {
            return TaskStatus.FAILED;
        }
    } // OnUpdate

} // class ChooseHQPathAction