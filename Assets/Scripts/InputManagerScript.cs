using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class InputManagerScript : MonoBehaviour
{
    public static InputManagerScript instance;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Found more than one Input Manager in the scene");
        }
        instance = this;
      
    }

    [SerializeField] public InputAction.CallbackContext left { get; private set; }
    [SerializeField] public InputAction.CallbackContext right { get; private set; }
    [SerializeField] public InputAction.CallbackContext up { get; private set; }
    [SerializeField] public InputAction.CallbackContext down { get; private set; }
    [SerializeField] public InputAction.CallbackContext jump { get; private set; }
    [SerializeField] public InputAction.CallbackContext interact { get; private set; }
    [SerializeField] public InputAction.CallbackContext roll { get; private set; }

    private float horizontal;
    private float vertical;
    private Vector2 speed;

    public void LeftPressed(InputAction.CallbackContext c)
    {
        left = c;
    }
    public void RightPressed(InputAction.CallbackContext c)
    {
        right = c;
    }
    public void UpPressed(InputAction.CallbackContext c)
    {
        up = c;
    }
    public void DownPressed(InputAction.CallbackContext c)
    {
        down = c;
    }
    public void InteractPressed(InputAction.CallbackContext c)
    {
        interact = c;
    }
    public void JumpPressed(InputAction.CallbackContext c)
    {
        jump = c;
    }
    public void RollPressed(InputAction.CallbackContext c)
    {
        roll = c;
    }

    public void Move(InputAction.CallbackContext context)
    {
        speed = context.ReadValue<Vector2>();
        //horizontal = context.ReadValue<Vector2>().x;
        //vertical = context.ReadValue<Vector2>().y;
    }
    public Vector2 Mover()
    {
        return speed;
    }
}
