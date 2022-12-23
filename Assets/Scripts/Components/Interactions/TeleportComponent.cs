using BackGround;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace General.Components.Interactions
{
    public class TeleportComponent : MonoBehaviour
    {
        [SerializeField] private Transform _destTransform;
        [SerializeField] private float _alphaTime = 1;
        [SerializeField] private float _moveTime = 1;
        [SerializeField] private ParticleSystem[] _particles;
        [SerializeField] private BackGroundFollowScript _backGround;


        public void Teleport(GameObject target)
        {
            StartCoroutine(AnimateTeleport(target));
            _particles[0].gameObject.SetActive(false);
        }


        private IEnumerator AnimateTeleport(GameObject target)
        {
            var sprite = target.GetComponent<SpriteRenderer>();

            var input = target.GetComponent<PlayerInput>();
            SetLockInput(input, true);

            yield return SetAlpha(sprite, 0);
            target.SetActive(false);

            yield return MoveAnimation(target);

            target.SetActive(true);
            yield return SetAlpha(sprite, 1);
            SetLockInput(input, false);
        }


        private void SetLockInput(PlayerInput input, bool isLocked)
        {
            if (input != null)
            {
                input.enabled = !isLocked;
            }
        }


        private IEnumerator MoveAnimation(GameObject target)
        {
            var moveTime = 0f;

            while (moveTime < _moveTime)
            {
                moveTime += Time.deltaTime;
                var progress = moveTime / _moveTime;
                target.transform.position = Vector3.Lerp(target.transform.position, _destTransform.position, progress);
                _backGround.SetObjectStartPosition();

                yield return null;
            }
        }


        private IEnumerator SetAlpha(SpriteRenderer sprite, float destAlpha)
        {
            var time = 0f;
            var spriteAlpha = sprite.color.a;

            while (time < _alphaTime)
            {
                time += Time.deltaTime;
                var progress = time / _alphaTime;
                var tmpAlpha = Mathf.Lerp(spriteAlpha, destAlpha, progress);
                var color = sprite.color;
                color.a = tmpAlpha;
                sprite.color = color;

                yield return null;
            }
        }
    }
}
