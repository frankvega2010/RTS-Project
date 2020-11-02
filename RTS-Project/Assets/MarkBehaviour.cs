using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkBehaviour : StateMachineBehaviour
{
    public delegate void OnMarkAction(GameObject node);
    public static OnMarkAction OnMarkDone;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Change Icon
        Debug.Log("Marking");
        Explorer explorer = animator.GetComponent<Explorer>();
        //explorer.SearchNewPath();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Explorer explorer = animator.GetComponent<Explorer>();

        if (explorer.pathFinding.choosenPath.Count > 0)
        {
            foreach (Node n in explorer.pathFinding.choosenPath)
            {
                for (int c = 0; c < n.nodesParent.Count; c++)
                {
                    if (n.nodesParent[c].villagerID == explorer.ID)
                    {
                        if (n.nodesParent[c].parent)
                        {
                            Debug.DrawLine(n.transform.position, n.nodesParent[c].parent.transform.position, explorer.color);
                        }
                    }
                }
            }

            //Go To Mine
            if (explorer.pathFinding.canGo)
            {
                explorer.transform.position = Vector3.MoveTowards(explorer.transform.position, explorer.pathFinding.choosenPath[explorer.pathFinding.waypointIndex].transform.position, Time.deltaTime * explorer.walkSpeed);
                explorer.transform.LookAt(explorer.pathFinding.choosenPath[explorer.pathFinding.waypointIndex].transform);

                if (Vector3.Distance(explorer.transform.position, explorer.pathFinding.choosenPath[explorer.pathFinding.waypointIndex].transform.position) <= explorer.pathFinding.changeWaypointDistance)
                {
                    explorer.pathFinding.waypointIndex--;
                    if (explorer.pathFinding.waypointIndex < 0)
                    {
                        explorer.pathFinding.waypointIndex = 0;
                        explorer.pathFinding.canGo = false;

                        if (explorer.pathFinding.choosenPath[explorer.pathFinding.waypointIndex] == explorer.oreMine)
                        {
                            //Plantar banderin
                            GameObject flag = Instantiate(explorer.flagPrefab, explorer.oreMine.transform.position, explorer.oreMine.transform.rotation);
                            explorer.oreMine.GetComponent<OreMine>().currentFlag = flag;
                            explorer.oreMine.GetComponent<OreMine>().isMarked = true;

                            if(OnMarkDone != null)
                            {
                                OnMarkDone(explorer.oreMine.gameObject);
                            }

                            explorer.mineSeen = false;
                            explorer.oreMine = null;
                        }

                        if (explorer.autoFindPath)
                        {
                            if (!explorer.SearchForMine())
                            {
                                //Pasar a patrolling
                                animator.SetTrigger("Patrol");
                            }
                        }
                    }
                }
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
