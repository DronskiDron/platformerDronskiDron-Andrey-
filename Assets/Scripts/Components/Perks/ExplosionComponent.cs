using Creatures;
using UnityEngine;

namespace General.Components.Perks
{
    public class ExplosionComponent : MonoBehaviour
    {
        [SerializeField] private int _velosity = 3;


        public void BlowUp(GameObject gameObject)
        {
            var collider = gameObject.GetComponent<Collider2D>();

            if (collider == null)
                return;
            var rb = collider.attachedRigidbody;
            if (rb)
            {
                var xDelta = collider.gameObject.transform.position.x > this.gameObject.transform.position.x ? 1 : -1;
                var yDelta = collider.gameObject.transform.position.y > this.gameObject.transform.position.y ? 1 : -1;
                var force = new Vector2(xDelta * _velosity, yDelta * _velosity);
                rb.AddForce(force, ForceMode2D.Impulse);
            }
        }


        public void MultiBlowUp(GameObject[] goArray)
        {
            foreach (var go in goArray)
            {
                var collider = go.GetComponent<Collider2D>();

                if (collider == null)
                    return;
                var rb = collider.attachedRigidbody;
                if (rb)
                {
                    var xDelta = collider.gameObject.transform.position.x > this.gameObject.transform.position.x ? 1 : -1;
                    var yDelta = collider.gameObject.transform.position.y > this.gameObject.transform.position.y ? 1 : -1;
                    var force = new Vector2(xDelta * _velosity, yDelta * _velosity);
                    rb.AddForce(force, ForceMode2D.Impulse);
                }
            }
        }


        public void UsePowerWave(GameObject gameObject)
        {
            var mobAI = gameObject.GetComponent<MobAI>();
            var creature = gameObject.GetComponent<Creature>();
            var collider = gameObject.GetComponent<Collider2D>();

            if (collider == null)
                return;
            var rb = collider.attachedRigidbody;
            if (rb)
            {
                var xDelta = collider.gameObject.transform.position.x > this.gameObject.transform.position.x ? 1 : -1;
                var yDelta = collider.gameObject.transform.position.y - this.gameObject.transform.position.y;
                var force = new Vector2(xDelta * _velosity, yDelta * _velosity);
                mobAI?.OnChangeSpriteDirection();
                creature?.SetMoveDirection(force);
                rb.AddForce(force, ForceMode2D.Impulse);
            }
        }
    }
}
