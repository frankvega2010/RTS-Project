using UnityEngine;

using Pada1.BBCore;           // Code attributes
using Pada1.BBCore.Tasks;     // TaskStatus
using Pada1.BBCore.Framework; // BasePrimitiveAction

[Action("Frank/GoToHQ")]
[Help("Goes to the HQ.")]
public class GoToHQAction : BasePrimitiveAction
{
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
        oreMine = villager.oreMineNode.GetComponent<OreMine>();
        villager.unavailableNodes.Clear();
        animator.SetBool("Patrol2", false);
        animator.SetBool("Mine2", false);
        animator.SetBool("Return2", true);
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

                        if (villager.pathFinding.choosenPath[villager.pathFinding.waypointIndex] == villager.hqSpawnNode)
                        {

                            //GOLD IS DELIVERED
                            villager.hq.DepositGold(villager.goldHolding);
                            villager.goldHolding = 0;

                            if (oreMine.currentGold <= 0)
                            {
                                //Debug.Log("Gold mine is empty");
                                villager.mineSeen = false;
                                villager.oreMineNode = null;
                            }
                            else
                            {
                                //Debug.Log("Gold mine is NOT empty");
                            }
                            villager.doOnceMine = false;
                            villager.isReturning = false;
                            return TaskStatus.COMPLETED;
                        }
                    }
                }
            }
        }

        return TaskStatus.RUNNING;
    } // OnUpdate

} // class GoToHQAction
