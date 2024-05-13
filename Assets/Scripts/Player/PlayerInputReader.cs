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


        public void OnTotalMovement(InputAction.CallbackContext context)
        {
            if (!IsInputEnable)
            {
                _player.SetMoveDirection(Vector2.zero);
            }
            else
            {
                var direction = context.ReadValue<Vector2>();
                _player.SetMoveDirection(direction);
            }
        }


        public void OnJumping(InputAction.CallbackContext context)
        {
            if (!IsInputEnable) return;
            
            var jumpVector = context.ReadValue<Vector2>();
            _groundCheck.SetIsPressingJump(jumpVector);

        }


        public void OnInteract(InputAction.CallbackContext context)
        {
            if (!IsInputEnable) return;

            if (context.started)
                _player.Interact();
        }


        public void OnAttack(InputAction.CallbackContext context)
        {
            if (!IsInputEnable) return;

            if (context.started)
                _player.Attack();
        }


        public void OnThrow(InputAction.CallbackContext context)
        {
            if (!IsInputEnable) return;

            if (context.started)
                _player.UseInventory();
        }


        public void OnSuperThrow(InputAction.CallbackContext context)
        {
            if (!IsInputEnable) return;

            if (context.started)
            {
                _player.IsSuperThrowAvailable(true);
                _player.Throw();
            }
        }


        public void OnNextItem(InputAction.CallbackContext context)
        {
            if (!IsInputEnable) return;

            if (context.started)
                _player.NextItem();
        }


        public void OnUsePerk(InputAction.CallbackContext context)
        {
            if (!IsInputEnable) return;

            if (context.started)
                _player.UsePerk();
        }


        public void OnUseLantern(InputAction.CallbackContext context)
        {
            if (!IsInputEnable) return;

            if (context.started)
                _player.UseLantern();
        }


        public void OnMousePress(InputAction.CallbackContext context)
        {
            Debug.Log("AAA");
        }
    }
}

