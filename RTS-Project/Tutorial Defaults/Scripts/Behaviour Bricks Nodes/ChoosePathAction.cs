using UnityEngine;

using Pada1.BBCore;           // Code attributes
using Pada1.BBCore.Tasks;     // TaskStatus
using Pada1.BBCore.Framework; // BasePrimitiveAction

[Action("Frank/ChoosePath")]
[Help("Chooses a path given a start and finish node.")]
public class ChoosePathAction : BasePrimitiveAction
{
    // Define the input parameter "bullet" (the prefab to be cloned).
    [InParam("VillagerGO")]
    public GameObject villagerGO;

    private Villager villager;

    // Initialization method. If not established, we look for the shooting point.
    public override void OnStart()
    {
        villager = villagerGO.GetComponent<Villager>();

        base.OnStart();
    }

    // Main class method, invoked by the execution engine.
    public override TaskStatus OnUpdate()
    {
        if (villager.doOnce)
        {
            if (villager.SearchNewPathOnce())
            {
                // The action is completed. We must inform the execution engine.
                return TaskStatus.COMPLETED;
            }
            else
            {
                // The action is completed. We must inform the execution engine.
                return TaskStatus.FAILED;
            }
        }
        else
        {
            villager.InitialSearch();
            villager.doOnce = true;
            return TaskStatus.COMPLETED;
        }
    } // OnUpdate

} // class ChoosePathAction