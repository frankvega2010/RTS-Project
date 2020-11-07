using UnityEngine;

using Pada1.BBCore;           // Code attributes
using Pada1.BBCore.Tasks;     // TaskStatus
using Pada1.BBCore.Framework; // BasePrimitiveAction

[Action("Frank/GoToHQ")]
[Help("Goes to the HQ.")]
public class GoToHQAction : BasePrimitiveAction
{
    [InParam("MinerGO")]
    public GameObject minerGO;

    private OreMine oreMine;
    private Miner miner;
    private Animator animator;

    public override void OnStart()
    {

        //Needs to fix some bugs here.
        miner = minerGO.GetComponent<Miner>();
        animator = miner.animator;
        if(miner.oreMineNode)
        {
            oreMine = miner.oreMineNode.GetComponent<OreMine>();
        }
        
        miner.unavailableNodes.Clear();
        animator.SetBool("Patrol2", false);
        animator.SetBool("Mine2", false);
        animator.SetBool("Return2", true);
        miner.UIComp.UpdateIcon(NPCUI.NPCStates.Return);
        base.OnStart();
    }

    // Main class method, invoked by the execution engine.
    public override TaskStatus OnUpdate()
    {
        if (miner.pathFinding.choosenPath.Count > 0)
        {
            foreach (Node n in miner.pathFinding.choosenPath)
            {
                for (int c = 0; c < n.nodesParent.Count; c++)
                {
                    if (n.nodesParent[c].villagerID == miner.ID)
                    {
                        if (n.nodesParent[c].parent)
                        {
                            Debug.DrawLine(n.transform.position, n.nodesParent[c].parent.transform.position, miner.color);
                        }
                    }
                }
            }

            //Keep patrolling
            if (miner.pathFinding.canGo)
            {
                miner.transform.position = Vector3.MoveTowards(miner.transform.position, miner.pathFinding.choosenPath[miner.pathFinding.waypointIndex].transform.position, Time.deltaTime * miner.walkSpeed);
                miner.transform.LookAt(miner.pathFinding.choosenPath[miner.pathFinding.waypointIndex].transform);

                if (Vector3.Distance(miner.transform.position, miner.pathFinding.choosenPath[miner.pathFinding.waypointIndex].transform.position) <= miner.pathFinding.changeWaypointDistance)
                {
                    miner.pathFinding.waypointIndex--;
                    if (miner.pathFinding.waypointIndex < 0)
                    {
                        miner.pathFinding.waypointIndex = 0;
                        miner.pathFinding.canGo = false;

                        if (miner.pathFinding.choosenPath[miner.pathFinding.waypointIndex] == miner.hqSpawnNode)
                        {

                            //GOLD IS DELIVERED
                            miner.unavailableNodes.Clear();
                            miner.hq.DepositGold(miner.goldHolding);
                            miner.goldHolding = 0;

                            if (oreMine && oreMine.currentGold <= 0)
                            {
                                miner.mineSeen = false;
                                miner.oreMineNode = null;
                            }
                            else if(!oreMine)
                            {
                                miner.mineSeen = false;
                                miner.oreMineNode = null;
                            }

                            miner.UIComp.UpdateText("x" + miner.goldHolding);
                            miner.doOnceMine = false;
                            miner.isReturning = false;
                            return TaskStatus.COMPLETED;
                        }
                    }
                }
            }
        }

        return TaskStatus.RUNNING;
    } // OnUpdate

} // class GoToHQAction
