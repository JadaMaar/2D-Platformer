using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private bool submitPressed = false;
    private bool interactPressed = false;
    private static InputManager instance;

    private void Awake()
    {
        instance = this;
    }

    public void SubmitPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            submitPressed = true;
        }
        else
        {
            submitPressed = false;
        }
    }

    public void OnInteract(InputValue value)
    {
        interactPressed = true;
    }

    public bool GetSubmitPressed()
    {
        return submitPressed;
    }

    public bool GetInteractPressed()
    {
        Debug.Log(interactPressed);
        return interactPressed;
    }

    public static InputManager GetInstance()
    {
        return instance;
    }
}
