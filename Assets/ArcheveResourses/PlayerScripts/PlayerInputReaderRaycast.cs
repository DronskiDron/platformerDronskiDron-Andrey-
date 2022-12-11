using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Creatures
{
    public class PlayerInputReaderRaycast : MonoBehaviour
    {
        [SerializeField] private PlayerControllerRaycast _player;
        [SerializeField] private PlayerJumpCheckerRaycast _playerJumpChecker;


        private void OnTotalMovement(InputValue context)
        {
            var direction = context.Get<Vector2>();

            _player.SetMoveDirection(direction);
        }

        private void OnJumping(InputValue context)
        {
            var jumpVector = context.Get<Vector2>();
            _playerJumpChecker.SetIsJumping(jumpVector);
        }


        private void OnSayingSomething(InputValue context)
        {
            _player.SaySomething();
        }
    }
}

