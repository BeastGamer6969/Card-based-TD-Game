using System;
using UnityEngine;
using UnityEngine.InputSystem;
[CreateAssetMenu(menuName = "InputReader")]
public class InputReader : ScriptableObject, Controls.IPlayerActions
{
    private Controls controls;

    public event Action<Vector3> MoveEvent;
    public event Action<float> ZoomEvent;
    public event Action<bool> LeftClick;
    public event Action<bool> RightClick;
    public event Action SpaceBar;

    void OnEnable()
    {
        if(controls == null)
        {
            controls = new Controls();
            controls.Player.SetCallbacks(instance:this);
            SetPlayer();
        }
    }

    void OnDisable()
    {
        if (controls != null)
        {
            controls.Player.Disable();
            controls.Disable();  
            controls = null;
        }
    }

    public void SetPlayer()
    {
        controls.Player.Enable();
    }


    public void OnMovement(InputAction.CallbackContext context)
    {
        MoveEvent?.Invoke(context.ReadValue<Vector3>());
    }

    public void OnZoom(InputAction.CallbackContext context)
    {
        float zoomValue = context.ReadValue<float>();
        ZoomEvent?.Invoke(zoomValue);
    }

    public void OnMouseButton(InputAction.CallbackContext context)
    {
        if(context.control == Mouse.current.leftButton)
        {
            if(context.started) LeftClick?.Invoke(true);
            if(context.canceled) LeftClick?.Invoke(false);
        }

        if(context.control == Mouse.current.rightButton)
        {
            if(context.started) RightClick?.Invoke(true);
            if(context.canceled) RightClick?.Invoke(false);
        }
    }

    public void OnSpaceBar(InputAction.CallbackContext context)
    {
        if(context.started) SpaceBar?.Invoke();
    }

}
