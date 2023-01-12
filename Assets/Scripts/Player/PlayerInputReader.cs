using Creatures.Player;
using General.Components.ColliderBased;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Creatures
{
    public class PlayerInputReader : MonoBehaviour
    {
        [SerializeField] private PlayerController _player;
        [SerializeField] private LayerCheckCreatures _groundCheck;


        private void OnTotalMovement(InputValue context)
        {
            var direction = context.Get<Vector2>();

            _player.SetMoveDirection(direction);
        }


        private void OnJumping(InputValue context)
        {
            var jumpVector = context.Get<Vector2>();
            _groundCheck.SetIsPressingJump(jumpVector);
        }


        private void OnInteract(InputValue context)
        {
            _player.Interact();
        }


        private void OnAttack(InputValue context)
        {
            _player.Attack();
        }


        private void OnThrow(InputValue context)
        {
            _player.Throw();
        }


        private void OnSuperThrow(InputValue context)
        {
            _player.IsSuperThrowAvailable(true);
            _player.Throw();
        }


        private void OnUse(InputValue context)
        {
            _player.UsePotion();
        }
    }
}

