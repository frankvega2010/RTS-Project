using UnityEngine;

using Pada1.BBCore;           // Code attributes
using Pada1.BBCore.Tasks;     // TaskStatus
using Pada1.BBCore.Framework; // BasePrimitiveAction

[Action("Frank/GoToMine")]
[Help("Goes to the found mine.")]
public class GoToMineAction : BasePrimitiveAction
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
        villager.unavailableNodes.Clear();
        villager.UIComp.UpdateIcon(NPCUI.NPCStates.Mine);
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

                        if (villager.pathFinding.choosenPath[villager.pathFinding.waypointIndex] == villager.oreMineNode)
                        {
                            /*villager.unavailableNodes.Clear();
                            villager.unavailableNodes.Add(villager.oreMineNode);*/
                            /*if (OnMarkDone != null)
                            {
                                OnMarkDone(explorer.oreMineNode.gameObject);
                            }*/

                            //animator.SetTrigger("Mine");

                            return TaskStatus.COMPLETED;
                        }
                    }
                }
            }
        }

        return TaskStatus.RUNNING;
    } // OnUpdate

} // class GoToMinehAction
