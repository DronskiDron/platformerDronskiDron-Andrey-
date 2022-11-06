using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerInputReader : MonoBehaviour
    {
        [SerializeField] private PlayerController _player;
        [SerializeField] private PlayerJumpChecker _playerJumpChecker;


        private void OnTotalMovement(InputValue context)
        {
            var direction = context.Get<Vector2>();

            _player.SetMoveDirection(direction);
        }


        private void OnJumping(InputValue context)
        {
            var jumpVector = context.Get<Vector2>();
            _playerJumpChecker.SetIsPressingJump(jumpVector);
        }


        private void OnSayingSomething(InputValue context)
        {
            _player.SaySomething();
        }


        private void OnInteract(InputValue context)
        {
            _player.Interact();
        }
    }
}

