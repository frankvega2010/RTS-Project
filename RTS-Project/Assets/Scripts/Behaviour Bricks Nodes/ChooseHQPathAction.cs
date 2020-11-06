using UnityEngine;

using Pada1.BBCore;           // Code attributes
using Pada1.BBCore.Tasks;     // TaskStatus
using Pada1.BBCore.Framework; // BasePrimitiveAction

[Action("Frank/ChooseHQPath")]
[Help("Chooses a path given a start and finish node.")]
public class ChooseHQPathAction : BasePrimitiveAction
{
    // Define the input parameter "bullet" (the prefab to be cloned).
    [InParam("VillagerGO")]
    public GameObject villagerGO;

    private Villager villager;
    private Animator animator;

    // Initialization method. If not established, we look for the shooting point.
    public override void OnStart()
    {
        villager = villagerGO.GetComponent<Villager>();
        animator = villager.animator;
        //villager.unavailableNodes.Clear();
        villager.UIComp.UpdateIcon(NPCUI.NPCStates.Return);
        base.OnStart();
    }

    // Main class method, invoked by the execution engine.
    public override TaskStatus OnUpdate()
    {
        if(villager.SearchNewPathToHQ())
        {
            // The action is completed. We must inform the execution engine.
            //animator.SetTrigger("Return");
            return TaskStatus.COMPLETED;
        }
        else
        {
            // The action is completed. We must inform the execution engine.
            return TaskStatus.FAILED;
        }
    } // OnUpdate

} // class ChooseHQPathAction