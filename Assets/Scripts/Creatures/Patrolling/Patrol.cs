using System.Collections;
using UnityEngine;

namespace Creatures.Patrolling
{
    public abstract class Patrol : MonoBehaviour
    {
        public abstract IEnumerator DoPatrol();
    }
}
