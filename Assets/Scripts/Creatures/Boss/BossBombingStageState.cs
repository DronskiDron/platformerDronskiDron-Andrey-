using System.Collections;
using System.Collections.Generic;
using Creatures.Boss.Bombs;
using UnityEngine;

public class BossBombingStageState : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var bombController = animator.GetComponent<BombsController>();
        bombController.StartBombing();
    }
}
