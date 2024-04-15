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

        public static bool IsInputEnable = true;


        private void OnTotalMovement(InputValue context)
        {
            if (!IsInputEnable)
            {
                _player.SetMoveDirection(Vector2.zero);
            }
            else
            {
                var direction = context.Get<Vector2>();
                _player.SetMoveDirection(direction);
            }
        }


        private void OnJumping(InputValue context)
        {
            if (!IsInputEnable) return;

            var jumpVector = context.Get<Vector2>();
            _groundCheck.SetIsPressingJump(jumpVector);
        }


        private void OnInteract(InputValue context)
        {
            if (!IsInputEnable) return;
            _player.Interact();
        }


        private void OnAttack(InputValue context)
        {
            if (!IsInputEnable) return;
            _player.Attack();
        }


        private void OnThrow(InputValue context)
        {
            if (!IsInputEnable) return;
            _player.UseInventory();
        }


        private void OnSuperThrow(InputValue context)
        {
            if (!IsInputEnable) return;
            _player.IsSuperThrowAvailable(true);
            _player.Throw();
        }


        private void OnNextItem(InputValue context)
        {
            if (!IsInputEnable) return;
            _player.NextItem();
        }


        private void OnUsePerk(InputValue context)
        {
            if (!IsInputEnable) return;
            _player.UsePerk();
        }


        private void OnUseLantern(InputValue context)
        {
            if (!IsInputEnable) return;
            _player.UseLantern();
        }
    }
}

