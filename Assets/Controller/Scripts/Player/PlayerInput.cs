using UnityEngine;
using UnityEngine.InputSystem;


namespace PlayerController
{
    public class PlayerInput : MonoBehaviour
    {
        private PlayerInputActions _actions;
        private InputAction _move, _jump, _dash, _attack, _ability, _utility;

        private void Awake()
        {
            _actions = new PlayerInputActions();
            _move = _actions.Player.Move;
            _jump = _actions.Player.Jump;
            _dash = _actions.Player.Dash;
            _attack = _actions.Player.Attack;
            _ability = _actions.Player.Ability;
            _utility = _actions.Player.Utility;
        }

        private void OnEnable() => _actions.Enable();

        private void OnDisable() => _actions.Disable();

        public FrameInput Gather()
        {
            return new FrameInput
            {
                JumpDown = _jump.WasPressedThisFrame(),
                JumpHeld = _jump.IsPressed(),
                DashDown = _dash.WasPressedThisFrame(),
                AttackDown = _attack.IsPressed(),
                AbilityDown = _ability.IsPressed(),
                UtilityDown = _utility.IsPressed(),
                Mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition),
                Move = _move.ReadValue<Vector2>()
            };
        }
    }

    public struct FrameInput
    {
        public Vector2 Move;
        public Vector2 Mouse;
        public bool JumpDown;
        public bool JumpHeld;
        public bool DashDown;
        public bool AttackDown;
        public bool AbilityDown;
        public bool UtilityDown;
    }
}