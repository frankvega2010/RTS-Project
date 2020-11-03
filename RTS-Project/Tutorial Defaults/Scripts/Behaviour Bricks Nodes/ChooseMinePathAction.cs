﻿using UnityEngine;

using Pada1.BBCore;           // Code attributes
using Pada1.BBCore.Tasks;     // TaskStatus
using Pada1.BBCore.Framework; // BasePrimitiveAction

[Action("Frank/ChooseMinePath")]
[Help("Chooses a path given a start and finish node.")]
public class ChooseMinePathAction : BasePrimitiveAction
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
        animator = villagerGO.GetComponent<Animator>();
        base.OnStart();
    }

    // Main class method, invoked by the execution engine.
    public override TaskStatus OnUpdate()
    {
        if(!villager.doOnceMine)
        {
            villager.doOnceMine = true;
            if (villager.SearchForMine(villager.oreMine))
            {
                animator.SetTrigger("GoToMine");
                // The action is completed. We must inform the execution engine.
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