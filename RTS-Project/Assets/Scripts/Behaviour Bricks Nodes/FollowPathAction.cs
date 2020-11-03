using UnityEngine;

using Pada1.BBCore;           // Code attributes
using Pada1.BBCore.Tasks;     // TaskStatus
using Pada1.BBCore.Framework; // BasePrimitiveAction

[Action("Frank/FollowPath")]
[Help("Chooses a path given a start and finish node.")]
public class FollowPathAction : BasePrimitiveAction
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
        base.OnStart();
    }

    // Main class method, invoked by the execution engine.
    public override TaskStatus OnUpdate()
    {
        if (villager.pathFinding.choosenPath.Count > 0)
        {
            foreach (Node n in villager.pathFinding.choosenPath)
            {
                for (int c = 0; c < n.nodesParent.Count; c++)
                {
                    if (n.nodesParent[c].villagerID == villager.ID)
                    {
                        if (n.nodesParent[c].parent)
                        {
                            Debug.DrawLine(n.transform.position, n.nodesParent[c].parent.transform.position, villager.color);
                        }
                    }
                }
            }

            if(villager.SearchForMine())
            {
                //Pasar a Mining, se reinicia  el behaviour tree y da resultado SUCCESS
               // animator.SetTrigger("GoToMine");
                return TaskStatus.COMPLETED;
            }
            else
            {
                //Keep patrolling
                if (villager.pathFinding.canGo)
                {
                    villager.transform.position = Vector3.MoveTowards(villager.transform.position, villager.pathFinding.choosenPath[villager.pathFinding.waypointIndex].transform.position, Time.deltaTime * villager.walkSpeed);
                    villager.transform.LookAt(villager.pathFinding.choosenPath[villager.pathFinding.waypointIndex].transform);

                    if (Vector3.Distance(villager.transform.position, villager.pathFinding.choosenPath[villager.pathFinding.waypointIndex].transform.position) <= villager.pathFinding.changeWaypointDistance)
                    {
                        villager.pathFinding.waypointIndex--;
                        if (villager.pathFinding.waypointIndex < 0)
                        {
                            villager.pathFinding.waypointIndex = 0;
                            villager.pathFinding.canGo = false;
 
                            if (villager.autoFindPath)
                            {
                                if (villager.SearchForMine())
                                {
                                    //Pasar a Mining,
                                    //animator.SetTrigger("Patrol");
                                }

                                // se reinicia  el behaviour tree y da resultado SUCCESS
                                return TaskStatus.COMPLETED;
                            }
                        }
                    }
                }
            }
        }

        return TaskStatus.RUNNING;
    } // OnUpdate

} // class FollowPathAction
