using UnityEngine;

using Pada1.BBCore;           // Code attributes
using Pada1.BBCore.Tasks;     // TaskStatus
using Pada1.BBCore.Framework; // BasePrimitiveAction

[Action("Frank/FollowPath")]
[Help("Chooses a path given a start and finish node.")]
public class FollowPathAction : BasePrimitiveAction
{
    [InParam("MinerGO")]
    public GameObject minerGO;

    private Miner miner;
    private Animator animator;

    public override void OnStart()
    {
        miner = minerGO.GetComponent<Miner>();
        animator = miner.animator;
        miner.UIComp.UpdateIcon(NPCUI.NPCStates.Patrol);
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

            if(miner.SearchForMine())
            {
                return TaskStatus.COMPLETED;
            }
            else
            {
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
                            miner.unavailableNodes.Clear();
                            miner.pathFinding.waypointIndex = 0;
                            miner.pathFinding.canGo = false;
 
                            if (miner.autoFindPath)
                            {
                                miner.SearchForMine();

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
