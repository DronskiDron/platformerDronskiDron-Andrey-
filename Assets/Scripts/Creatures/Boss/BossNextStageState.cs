﻿using General.Components.ColliderBased;
using UnityEngine;

namespace Creatures.Boss
{
    public class BossNextStageState : StateMachineBehaviour
    {
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            var spawner = animator.GetComponent<CircularProjectileSpawner>();
            spawner.Stage++;

            var changeLight = animator.GetComponent<ChangeLightsComponent>();
            changeLight.SetColor();
        }
    }
}
