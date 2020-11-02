using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolBehaviour : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Change Icon
        Debug.Log("Patrolling");
        Explorer explorer = animator.GetComponent<Explorer>();
        if(!explorer.doOnce)
        {
            explorer.InitialSearch();
            explorer.doOnce = true;
        }
        else
        {
            explorer.SearchNewPath();
        }
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

            if(explorer.SearchForMine())
            {
                //Pasar a Marking
                animator.SetTrigger("Mark");
            }
            else
            {
                //Keep Patrolling
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

                            if (explorer.autoFindPath)
                            {
                                if (!explorer.SearchForMine())
                                {
                                    explorer.SearchNewPath();
                                }
                                else
                                {
                                    //Pasar a Marking
                                    animator.SetTrigger("Mark");
                                }
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
