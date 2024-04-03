﻿using Creatures.Player;
using System.Collections;
using UnityEngine;

namespace General.Components.Interactions
{
    public class TeleportComponent : MonoBehaviour
    {
        [SerializeField] private Transform _destTransform;
        [SerializeField] private float _alphaTime = 1;
        [SerializeField] private float _moveTime = 1;
        [SerializeField] private ParticleSystem[] _particles;
        [SerializeField] private Collider2D _collider;

        private PlayerController _player;
        private InputEnableComponent _inputEnabler;
        private int _particleSysInPlayerGOIndex = 0;


        private void Start()
        {
            _player = FindObjectOfType<PlayerController>();
            _inputEnabler = FindObjectOfType<InputEnableComponent>();
        }


        public void TeleportPlayer()
        {
            StartCoroutine(AnimateTeleport(_player.gameObject));
            var particle = _player.transform.GetChild(_particleSysInPlayerGOIndex);
            particle.gameObject?.SetActive(false);
        }


        private IEnumerator AnimateTeleport(GameObject target)
        {
            TeleportAntibagColliderToggle(false);

            var sprite = target.GetComponent<SpriteRenderer>();

            yield return SetAlpha(sprite, 0);
            target.SetActive(false);

            yield return MoveAnimation(target);

            target.SetActive(true);
            yield return SetAlpha(sprite, 1);

            TeleportAntibagColliderToggle(true);
            _inputEnabler?.SetInputActivationStatus(true);
            _inputEnabler?.SetInputEnabled();
        }


        private IEnumerator MoveAnimation(GameObject target)
        {
            var moveTime = 0f;

            while (moveTime < _moveTime)
            {
                moveTime += Time.deltaTime;
                var progress = moveTime / _moveTime;
                target.transform.position = Vector3.Lerp(target.transform.position, _destTransform.position, progress);

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


        private void TeleportAntibagColliderToggle(bool value)
        {
            _collider.enabled = value;
        }
    }
}
