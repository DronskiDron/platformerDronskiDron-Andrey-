using UnityEngine;

namespace Creatures.Boss
{
    public class BossFloodState : StateMachineBehaviour
    {
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            var spawner = animator.GetComponent<FloodController>();
            spawner.StartFlooding();
        }
    }
}
