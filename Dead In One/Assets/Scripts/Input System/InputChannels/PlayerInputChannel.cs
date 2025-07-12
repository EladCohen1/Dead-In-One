using System;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "Event Channels/Player Input Channel")]
public class PlayerInputChannel : ScriptableObject, InputSystem_Actions.IPlayerActions
{
    public event Action<Vector2Int> OnMoveEvent;

    private InputSystem_Actions input;


    private void OnEnable()
    {
        if (input == null)
        {
            input = new InputSystem_Actions();
            input.Player.SetCallbacks(this);
        }

        input.Player.Enable();
    }

    private void OnDisable()
    {
        if (input != null)
            input.Player.Disable();
    }

    public void OnMoveUpKeyboard(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Performed)
            return;
        OnMoveEvent?.Invoke(Vector2Int.up);
    }

    public void OnMoveDownKeyboard(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Performed)
            return;
        OnMoveEvent?.Invoke(Vector2Int.down);
    }

    public void OnMoveLeftKeyboard(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Performed)
            return;
        OnMoveEvent?.Invoke(Vector2Int.left);
    }

    public void OnMoveRightKeyboard(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Performed)
            return;
        OnMoveEvent?.Invoke(Vector2Int.right);
    }

    public void OnLook(InputAction.CallbackContext context)
    {

    }

    public void OnAttack(InputAction.CallbackContext context)
    {

    }

    public void OnInteract(InputAction.CallbackContext context)
    {

    }

    public void OnCrouch(InputAction.CallbackContext context)
    {

    }

    public void OnJump(InputAction.CallbackContext context)
    {

    }

    public void OnPrevious(InputAction.CallbackContext context)
    {

    }

    public void OnNext(InputAction.CallbackContext context)
    {

    }

    public void OnSprint(InputAction.CallbackContext context)
    {

    }
}